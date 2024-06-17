using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.UserLog;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class UserLogTimeUController : Controller
    {
        // GET: UserLogTimeU
        private GroupService _groupService;
        private UserManagementService _userService;
        private UserLogTimeService _userLogTimeService;
        private ClientCenterService _clientCenterService;

        public UserLogTimeUController()
        {
            _clientCenterService = new ClientCenterService();
            _groupService = new GroupService();
            _userService = new UserManagementService();
            _userLogTimeService = new UserLogTimeService();
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
                                   Url = "/UserLogTimeU/Index"
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
                 ViewData["ClientCenters"] = responseClientCenter.Entities.ToList(); // studentList;
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
            int currentPage = dsRequest.Page;
            var startDate = fpub.convert2miladi(sStartDate);
            var endDate = fpub.convert2miladi(sEndDate);
            var gu = new List<GroupUserVM>();
            var users = new List<UserLogVM>();
           if (sUserName == "" && sClientCenterId == null && form["SGroups[0]"] == null
                && sStartDate == "" && sEndDate == "" && sIsClickSearch == false)
            {

                var oldS = (UserVM)TempData["oldSearchBackULog"];

                if (oldS != null)
                {
                    TempData["oldSearchBackULog"] = oldS;
                    if (oldS.UserName == "*")
                    {
                        var responseUser = _userLogTimeService.FindAllPaging(new UserLogTimeFindAllRequest()
                        {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            Index = currentPage,
                            Count = 10,
                        });
                        if (!responseUser.IsSuccess)
                        {
                            ModelState.AddModelError("Error", responseUser.Message);
                            return Json(ModelState.ToDataSourceResult());
                        }

                        var resultPaging = new DataSourceResult()
                        {
                            Data = ToUserLog(responseUser.Entities.ToList()),
                            Total = responseUser.TotalRecords
                        };

                        
                        return Json(resultPaging);
                    }



                        var groupId1 = oldS.GroupUsers.Select(c => c.GroupId).ToArray();
                  
                    var tp1 = (groupId1.Any()) ? groupId1.ToList().Count() : 0;

                    var responseUser2 = _userLogTimeService.FindByExpressionPaging(new UserLogTimeFindByExpressionExpRequest()
                    {

                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        expression = x =>
                        (sUserName == "" || x.User.UserName.Contains(sUserName)) &&
            (sClientCenterId == null || x.User.ClientCenterId == sClientCenterId) &&
            (tp1 == 0 ||
             (x.User.GroupUsers.Any()

              && x.User.GroupUsers.Count() != 0 &&
              x.User.GroupUsers.Any(s => groupId1.Contains(s.GroupId))))
            && (sStartDate == "" || x.LoginTime >= startDate) &&
            (sEndDate == "" || x.LogoutTime <= endDate),
                        Index = currentPage,
                        Count = 10,

                    });
                    if (!responseUser2.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseUser2.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                    var resultPaging2 = new DataSourceResult()
                    {
                        Data = ToUserLog(responseUser2.Entities.ToList()),
                        Total = responseUser2.TotalRecords
                    };

                    return Json(resultPaging2);


                }
                return Json(ModelState.ToDataSourceResult());
            }











            if (sUserName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackULog"] = new UserVM()
                {
                    UserName = sUserName,
                    ClientCenterId = sClientCenterId ?? 0,
                    GroupUsers = new List<GroupUserVM>(),
                };
                TempData["reportUserLogTimeInitialize"] = new ReportClientUserLogTimeRRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new UserVM()
                    {
                        UserName = "",
                        ClientCenterId = sClientCenterId ?? 0,
                        GroupUsers = new List<GroupUserVM>(),
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                };
                var responseUser = _userLogTimeService.FindAllPaging(new UserLogTimeFindAllRequest() { 
                    
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    Index = currentPage,
                    Count = 10,
                });
                if (!responseUser.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseUser.Message);
                    return Json(ModelState.ToDataSourceResult());
                }

                var resultPaging2 = new DataSourceResult()
                {
                    Data = ToUserLog(responseUser.Entities.ToList()),
                    Total = responseUser.TotalRecords
                };

                 return Json(resultPaging2); 
            }
            if (sUserName == "" && sClientCenterId == null && form["SGroups[0]"] == null
                 && sStartDate == "" && sEndDate == "")
            {
                return Json(ModelState.ToDataSourceResult());
            }

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
                var tp = (groupId.Any()) ? groupId.ToList().Count() : 0;
               
                    var responseUser1 = _userLogTimeService.FindByExpressionPaging(new UserLogTimeFindByExpressionExpRequest()
                    {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x =>
                            (sUserName == "" || x.User.UserName.Contains(sUserName)) &&
                (sClientCenterId == null || x.User.ClientCenterId == sClientCenterId) &&
                (tp == 0 ||
                 (x.User.GroupUsers.Any()

                  && x.User.GroupUsers.Count() != 0 &&
                  x.User.GroupUsers.Any(s => groupId.Contains(s.GroupId))))
                && (sStartDate == "" || x.LoginTime >= startDate) &&
                (sEndDate == "" || x.LogoutTime <= endDate),
                                              Index = currentPage,
                            Count = 10,

                        });
                    if (!responseUser1.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseUser1.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
            
                var resultPaging1 = new DataSourceResult()
                {
                    Data = ToUserLog( responseUser1.Entities.ToList()),
                    Total = responseUser1.TotalRecords
                };
                
                return Json(resultPaging1);
            


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

        public UserLogVM ToUser(UserVM u, UserLogTimeExpVM ul)
        {

            return new UserLogVM()
            {
                 Id = ul.Id,
                UserId = ul.UserId,
                UserName = u.UserName,
               
                ClientCenterId = u.ClientCenterId,
               
                Groups = GroupsList(u.GroupUsers),
                
                ProviderKey = ul.ProviderKey,
                LoginTimeStr = fpub.convert2shamsiSTime(ul.LoginTime),
                LogoutTimeStr = fpub.convert2shamsiSTime(ul.LogoutTime),

            };


        }

        public List<UserLogVM> ToUserLog(List<UserLogTimeVM> u)
        {
            users = new List<UserLogVM>();
            foreach (var ul in u)
            {
                users.Add(new UserLogVM
                {
                    
                        Id = ul.Id,
                        UserId = ul.UserId,
                        UserName = ul.User.UserName,
                        
                        ClientCenterId = ul.User.ClientCenterId,
                        
                        Groups = GroupsList(ul.User.GroupUsers),
                       
                        ProviderKey = ul.ProviderKey,
                        LoginTimeStr = ul.LoginTime,
                        LogoutTimeStr = ul.LogoutTime,

                    });

            }
            return users;
        }

     
    }

}