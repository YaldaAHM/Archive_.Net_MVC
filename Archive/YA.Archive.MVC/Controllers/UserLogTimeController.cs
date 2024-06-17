using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.UserLog;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models.User;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.Messaging;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class UserLogTimeController : Controller
    {
        // GET: UserLogTime
        private GroupService _groupService;
        private UserManagementService _userService;
        private UserLogTimeService _userLogTimeService;
        private ClientCenterService _clientCenterService;

        public UserLogTimeController()
        {
            _clientCenterService = new ClientCenterService();
            _groupService = new GroupService();
            _userService = new UserManagementService();
            _userLogTimeService=new UserLogTimeService();
        }
        public ActionResult Index(int? isBack)
        {
            var mapSites = new List<SiteRouting>
                           {
                               new SiteRouting()

                               {
                                   Subject = "Home",
                                   Url = "/Home/Index"
                               },
                               new SiteRouting()

                               {
                                   Subject = "Users Log",
                                   Url = "/UserLogTime/Index"
                               },

                           };
            ViewData["UsersSiteMap"] = mapSites;
            TempData["isbackULog"] = isBack;
            var responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
               });
            if (responseClientCenter.IsSuccess)
            {
                 ViewData["ClientCenters"] = responseClientCenter.Entities.ToList(); 
            }
            List<GroupVM> groupList = new List<GroupVM>();

            var responseGroup = _groupService.FindAll(
                new GroupFindAllRequest
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                });
            if (responseGroup.IsSuccess)
            {
                foreach (var e in responseGroup.Entities.ToList())
                {
                    groupList.Add(new GroupVM() { Id = e.Id, Name = e.Name });

                }
                ViewData["Group"] = groupList;
            }
            return View();
        }

        [ClaimsAccess(ClaimType = "User", Value = "CanViewUser")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest dsRequest, 
            int? sClientCenterId, List<GroupVM> sGroups,
            string sUserName, string sStartDate, string sEndDate
            , FormCollection form, bool sIsClickSearch)
        {
            int ii = 0;
            var startDate = fpub.convert2miladi(sStartDate);
            var endDate = fpub.convert2miladi(sEndDate);
            var gu = new List<GroupUserVM>();
            var users = new List<UserLogVM>();
              if (sUserName == "" && sClientCenterId == null && form["SGroups[0]"] == null
                && sStartDate=="" && sEndDate=="" && sIsClickSearch == false)
            {
              
              var oldS = (UserVM)TempData["oldSearchBackULog"];

                if (oldS != null)
                {
                    TempData["oldSearchBackULog"] = oldS;
                    if (oldS.UserName == "*")
                    {
                        var responseUser = _userService.FindAll(
                            new UserFindAllRequest()
                            {
                                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            });
                        if (!responseUser.IsSuccess)
                        {
                            ModelState.AddModelError("Error", responseUser.Message);
                            return Json(ModelState.ToDataSourceResult());
                        }

                        foreach (var r in responseUser.Entities)
                        {
                            var ul = _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                            {
                                expression = x => x.UserId == r.Id
                                 && (sStartDate == "" || x.LoginTime >= startDate) &&
                                              (sEndDate == "" || x.LoginTime <= endDate)
                            });
                            foreach (var ull in ul.Entities)
                            {
                                users.Add(ToUser(r, ull));
                            }
                          
                        }
                      
                        var result1 = users.ToDataSourceResult(dsRequest);
                        return Json(result1);
                    }
                   

                    if (oldS.GroupUsers != null)
                    {

                        var groupId = oldS.GroupUsers.Select(c => c.GroupId).ToArray();
                        List<string> tmpList = new List<string>();
                        string[] idF = { };
                        foreach (var ts in groupId)
                        {
                            var responseUser = _userService.FindByExpression(
                                new UserFindByExpressionExpRequest()
                                {

                                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                    expression = x => x.GroupUsers.Any()

                                                      && x.GroupUsers.Count() != 0 &&
                                                      x.GroupUsers.Select(m => m.GroupId).Contains(ts) &&
                                                      (oldS.UserName == "" || x.UserName.Contains(oldS.UserName)) &&
                                                      (oldS.ClientCenterId == 0 ||
                                                       x.ClientCenterId == oldS.ClientCenterId) &&
                                                      !idF.Contains(x.Id)

                                });
                            if (!responseUser.IsSuccess)
                            {
                                ModelState.AddModelError("Error", responseUser.Message);
                                return Json(ModelState.ToDataSourceResult());
                            }
                           
                            foreach (var r in responseUser.Entities)
                            {
                                if (users != null && users.FirstOrDefault(f => f.UserId == r.Id) == null)
                                {
                                    var ul = _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                                    {
                                        expression = x => x.UserId == r.Id
                                         && (sStartDate == "" || x.LoginTime >= startDate) &&
                                              (sEndDate == "" || x.LoginTime <= endDate)
                                    });
                                    foreach (var ull in ul.Entities)
                                    {
                                        users.Add(ToUser(r, ull));
                                    }
                                   
                                }
                            }

                        }
                    }
                    else
                    {
                        var responseUser = _userService.FindByExpression(
                            new UserFindByExpressionExpRequest()
                            {

                                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                expression = x => // x.UserName == "Admin"
                                    (oldS.UserName == "" || x.UserName.Contains(oldS.UserName)) &&
                                    (oldS.ClientCenterId == 0 || x.ClientCenterId == oldS.ClientCenterId)


                            });
                        if (!responseUser.IsSuccess)
                        {
                            ModelState.AddModelError("Error", responseUser.Message);
                            return Json(ModelState.ToDataSourceResult());
                        }

                        foreach (var r in responseUser.Entities)
                        {
                            
                            var ul = _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                            {
                                expression = x => x.UserId == r.Id,
                            });
                            foreach (var ull in ul.Entities)
                            {
                                users.Add(ToUser(r, ull));
                            }
                            
                        }
                       
                    }


                    var result2 = users.ToDataSourceResult(dsRequest);
                    return Json(result2);
                    
                }
                return Json(ModelState.ToDataSourceResult());
            }











            if (sUserName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackULog"] = new UserVM()
                {
                    UserName = sUserName,
                    ClientCenterId = sClientCenterId ?? 0,
                    GroupUsers = null,
                };
                TempData["reportUserLogTimeInitialize"] = new ReportClientUserLogTimeRRequest()
                {
                    RequsetCurrentUserId =System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new UserVM()
                    {
                        UserName = "",
                        ClientCenterId = sClientCenterId ?? 0,
                        GroupUsers = new List<GroupUserVM>(),
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                };
                var responseUser = _userService.FindAll(
                    new UserFindAllRequest()
                    {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    });
                if (!responseUser.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseUser.Message);
                    return Json(ModelState.ToDataSourceResult());
                }

                foreach (var r in responseUser.Entities)
                {
                    //    if (users.FirstOrDefault(f => f.Id == r.Id) != null)
                    //    {
                    var ul=  _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                                                         {
                                                             expression = x => x.UserId == r.Id,
                    });
                    foreach (var ull in ul.Entities)
                    {
                        users.Add(ToUser(r,ull));
                    }
                    
                    //users.AddRange(ToUser(responseUser.Entities.ToList()));
                    //    }
                }
                //return Json(new[] { users }.ToDataSourceResult(dsRequest, ModelState));

                var result1 = users.ToDataSourceResult(dsRequest);
                return Json(result1);
            }
            if (sUserName == "" && sClientCenterId == null && form["SGroups[0]"] == null
                 && sStartDate == "" && sEndDate == "" && sIsClickSearch == false)
            {
                return Json(ModelState.ToDataSourceResult());
            }

            // var ttttt = GetProjects();
            if (sGroups != null && form["SGroups[0]"] != null)
            {
                foreach (var i in sGroups)
                {
                    gu.Add(new GroupUserVM()
                    {
                        GroupId = int.Parse(form["SGroups[" + ii + "]"]),
                        // UserId = 1
                    });
                    ii++;
                }
                TempData["reportUserLogTimeInitialize"] = new ReportClientUserLogTimeRRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new UserVM()
                    {
                        UserName = sUserName,
                        ClientCenterId = sClientCenterId ?? 0,
                        GroupUsers = gu,
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                };
                TempData["oldSearchBackULog"] = new UserVM()
                {
                    UserName = sUserName,
                    ClientCenterId = sClientCenterId ?? 0,
                    GroupUsers = gu,
                };
                var groupId = gu.Select(c => c.GroupId).ToArray();
                List<string> tmpList = new List<string>();
                string[] idF = { };
                foreach (var ts in groupId)
                {
                    var responseUser = _userService.FindByExpression(
                        new UserFindByExpressionExpRequest()
                        {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x => x.GroupUsers.Any()

                                              && x.GroupUsers.Count() != 0 &&
                                              x.GroupUsers.Select(m => m.GroupId).Contains(ts) &&
                                              (sUserName == "" || x.UserName.Contains(sUserName)) &&
                                              (sClientCenterId == null || x.ClientCenterId == sClientCenterId) &&
                                              !idF.Contains(x.Id)

                        });
                    if (!responseUser.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseUser.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                    //tmpList.AddRange(responseUser.Entities.Select(x => x.Id).ToList());

                    //idF = tmpList.ToArray();
                    //users.AddRange(ToUser(responseUser.Entities.ToList()));
                    foreach (var r in responseUser.Entities)
                    {
                        if (users != null && users.FirstOrDefault(f => f.UserId == r.Id) == null)
                        {
                            var ul = _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                            {
                                expression = x => x.UserId == r.Id
                                 && (sStartDate == "" || x.LoginTime >= startDate) &&
                                              (sEndDate == "" || x.LoginTime <= endDate)
                            });
                            foreach (var ull in ul.Entities)
                            {
                                users.Add(ToUser(r, ull));
                            }
                            //users.AddRange(ToUser(responseUser.Entities.ToList()));
                        }
                    }

                }
            }
            else
            {
                TempData["reportUserLogTimeInitialize"] = new ReportClientUserLogTimeRRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new UserVM()
                    {
                        UserName = sUserName,
                        ClientCenterId = sClientCenterId ?? 0,
                        GroupUsers = new List<GroupUserVM>(),
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                };
                TempData["oldSearchBackULog"] = new UserVM()
                {
                    UserName = sUserName,
                    ClientCenterId = sClientCenterId ?? 0,
                    GroupUsers = new List<GroupUserVM>(),
                };
                var responseUser = _userService.FindByExpression(
                    new UserFindByExpressionExpRequest()
                    {

                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        expression = x => // x.UserName == "Admin"
                            (sUserName == "" || x.UserName.Contains(sUserName)) &&
                            (sClientCenterId == null || x.ClientCenterId == sClientCenterId)


                    });
                if (!responseUser.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseUser.Message);
                    return Json(ModelState.ToDataSourceResult());
                }

                foreach (var r in responseUser.Entities)
                {
                    //    if (users.FirstOrDefault(f => f.Id == r.Id) != null)
                    //    {
                    var ul = _userLogTimeService.FindByExpressionExp(new UserLogTimeFindByExpressionExpRequest()
                    {
                        expression = x => x.UserId == r.Id
                      &&  (sStartDate == "" || x.LoginTime >= startDate) &&
                                              (sEndDate == "" || x.LoginTime <= endDate) 
                    });
                    foreach (var ull in ul.Entities)
                    {
                        users.Add(ToUser(r, ull));
                    }
                    //users.AddRange(ToUser(responseUser.Entities.ToList()));
                    //    }
                }
                //return Json(new[] { users }.ToDataSourceResult(dsRequest, ModelState));
            }


            var result = users.ToDataSourceResult(dsRequest);
            return Json(result);
      

        }
        private List<GroupVM> GroupsList(List<GroupUserVM> gu)
        {
            List<GroupVM> groups = new List<GroupVM>();
            foreach (var g in gu)
            {
                groups.Add(new GroupVM() { Id = g.GroupId, Name = g.Group.Name });
            }
            return groups;
        }
        public List<UserLogVM> users = new List<UserLogVM>();

        //public List<UserLogVM> ToUser(List<UserVM> user)
        //{
        //    foreach (var u in user)
        //    {
        //        users.Add(new UserLogVM()
        //        {
        //            //Registration =new RegistrationVM() {
        //            Id = u.Id,
        //            UserName = u.UserName,
        //            // Email = u.Email,
        //            ClientCenterId = u.ClientCenterId,
        //            //FruitIds=,
        //            //  Fruits = PopulateFruitsItem(),
        //            //                    GroupUsers = u.GroupUsers,
        //            //},
        //            Groups = GroupsList(u.GroupUsers)
        //        });

        //    }
        //    return users;
        //}

        public UserLogVM ToUser(UserVM u,UserLogTimeExpVM ul)
        {

            return new UserLogVM()
            {
                //Registration =new RegistrationVM() {
                Id = ul.Id,
                UserId = ul.UserId,
                UserName = u.UserName,
                // Email = u.Email,
                ClientCenterId = u.ClientCenterId,
                //FruitIds=,
                //  Fruits = PopulateFruitsItem(),
                //                    GroupUsers = u.GroupUsers,
                //},
                Groups = GroupsList(u.GroupUsers),
                //LoginTime = ul.LoginTime,
                //LogoutTime=ul.LogoutTime,
                ProviderKey = ul.ProviderKey,
                LoginTimeStr=fpub.convert2shamsiSTime( ul.LoginTime),
                LogoutTimeStr= fpub.convert2shamsiSTime(ul.LogoutTime), 
               
            };


        }
    }
}