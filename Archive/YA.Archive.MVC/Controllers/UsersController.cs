using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.User;
using YA.Archive.MVC2.Models.UserManagement;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Localization.MessageLocalize;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        private ClientCenterService _clientCenterService;
        private GroupService _groupService;
        private RoleManagementService _roleService;
        private UserManagementService _userService;

        public UsersController()
        {
            _clientCenterService = new ClientCenterService();
            _groupService = new GroupService();
            _roleService = new RoleManagementService();
            _userService = new UserManagementService();
        }

        public List<User> users = new List<User>();

        public List<User> ToUser(List<UserVM> user)
        {
            foreach (var u in user)
            {
                users.Add(new User()
                          {
                               Id = u.Id,
                              UserName = u.UserName,
                           
                              ClientCenterId = u.ClientCenterId,
                    ClientCenterId2 = u.ClientCenterId,
              
                    Groups = GroupsList(u.GroupUsers)
                          });

            }
            return users;
        }

        public User ToUser(UserVM u)
        {

            return new User()
                   {
                       Id = u.Id,
                       UserName = u.UserName,
                       ClientCenterId = u.ClientCenterId,
                ClientCenterId2 = u.ClientCenterId,
                Groups = GroupsList(u.GroupUsers)
                   };


        }

        private MultiSelectList GetTypeOfFile(string[] selectedValues)
        {
            var responseC = _groupService.FindAll(
                new GroupFindAllRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                });

            var t = responseC.Entities.ToList();

            return new MultiSelectList(t, "Id", "Name", selectedValues);
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
                                   Subject = "Users Management",
                                   Url = "/Users/Index"
                               },

                           };
            ViewData["UsersSiteMap"] = mapSites;
             TempData["isbackUU"] = isBack;
            ViewBag.TypeOfFilelist = GetTypeOfFile(null);
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
                    groupList.Add(new GroupVM() {Id = e.Id, Name = e.Name});

                }
                ViewData["Group"] = groupList;
            }
            return View();
        }

        [ClaimsAccess(ClaimType = "User", Value = "CanViewUser")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest dsRequest, int? isbackUU,
            int? sClientCenterId, List<GroupVM> sGroups,
            string sUserName
            , FormCollection form, bool sIsClickSearch)
        {
            int ii = 0;
            int currentPage = dsRequest.Page;
            var gu = new List<GroupUserVM>();
            var users = new List<User>();
          
            if (sUserName == "" && sClientCenterId == null && form["SGroups[0]"] == null && sIsClickSearch == false)
            {

                var oldS = (UserVM) TempData["oldSearchBackU"];

                if (oldS != null)
                {
                    TempData["oldSearchBackU"] = oldS;
                    if (oldS.UserName == "*")
                    {
                        var responseUser = _userService.FindAllPaging(
                            new UserFindAllRequest()
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

                        foreach (var r in responseUser.Entities)
                        {
                             users.Add(ToUser(r));
                           
                        }
                        
                        var resultPaging1 = new DataSourceResult()
                        {
                            Data = users,
                            Total = responseUser.TotalRecords
                        };

                        return Json(resultPaging1);
                    }
                    if (oldS.UserName == "" && oldS.ClientCenterId == 0 && oldS.GroupUsers == null)
                    {
                        return Json(ModelState.ToDataSourceResult());
                    }

                   

                    var groupId1 = oldS.GroupUsers.Select(c => c.GroupId).ToArray();
                    var tp1 = (groupId1.Any()) ? groupId1.ToList().Count() : 0;
                   
                    var responseUser1 = _userService.FindByExpressionPaging(
                        new UserFindByExpressionExpRequest()
                        {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x => 
                                (oldS.UserName == "" || x.UserName.Contains(oldS.UserName)) &&
                                (oldS.ClientCenterId == 0 ||
                                 x.ClientCenterId == oldS.ClientCenterId) &&

                                (tp1 == 0 ||
                                 (x.GroupUsers.Any()
                                  && x.GroupUsers.Count() != 0 &&
                                  x.GroupUsers.Any(s => groupId1.Contains(s.GroupId))))

                                  ,
                            Index = currentPage,
                            Count = 10,
                        });
                    if (!responseUser1.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseUser1.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                    
                    users.AddRange(ToUser(responseUser1.Entities.ToList()));

                    var resultPaging = new DataSourceResult()
                    {
                        Data = users,
                        Total = responseUser1.TotalRecords
                    };

                    return Json(resultPaging);
                }
                return Json(ModelState.ToDataSourceResult());
            }


            if (sUserName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackU"] = new UserVM()
                                             {
                                                 UserName = sUserName,
                                                 ClientCenterId = sClientCenterId ?? 0,
                                                 GroupUsers = new List<GroupUserVM>(),
                                             };
                var responseUser = _userService.FindAllPaging(
                    new UserFindAllRequest()
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

                foreach (var r in responseUser.Entities)
                {
                    
                    users.Add(ToUser(r));
                   
                }
                 var resultPaging1 = new DataSourceResult()
                {
                    Data = users,
                    Total = responseUser.TotalRecords
                };

                return Json(resultPaging1);
                
            }
            
            if (sGroups != null && form["SGroups[0]"] != null)
            {
                foreach (var i in sGroups)
                {
                    gu.Add(new GroupUserVM()
                           {
                               GroupId = int.Parse(form["SGroups[" + ii + "]"]),
                              
                           });
                    ii++;
                }
            }
            TempData["oldSearchBackU"] = new UserVM()
                                             {
                                                 UserName = sUserName,
                                                 ClientCenterId = sClientCenterId ?? 0,
                                                 GroupUsers = gu,
                                             };
                var groupId = gu.Select(c => c.GroupId).ToArray();
            var tp = (groupId.Any()) ? groupId.ToList().Count() : 0;
          
                    var responseUser2 = _userService.FindByExpressionPaging(
                        new UserFindByExpressionExpRequest()
                        {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x =>   
                                              (sUserName == "" || x.UserName.Contains(sUserName)) &&
                                              (sClientCenterId == null || x.ClientCenterId == sClientCenterId) &&
                                             
                                                (tp == 0 ||
                                              (x.GroupUsers.Any()

                                              && x.GroupUsers.Count() != 0 &&
                                              x.GroupUsers.Any(s => groupId.Contains(s.GroupId))))

,
                            Index = currentPage,
                            Count = 10,
                        });
                    if (!responseUser2.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseUser2.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                   
users.AddRange(ToUser(responseUser2.Entities.ToList()));

            var resultPaging2 = new DataSourceResult()
            {
                Data = users,
                Total = responseUser2.TotalRecords
            };

            return Json(resultPaging2);
          
        }

        private List<GroupVM> GroupsList(List<GroupUserVM> gu)
        {
            List<GroupVM> groups = new List<GroupVM>();
            foreach (var g in gu)
            {
                  groups.Add(new GroupVM() {Id = g.GroupId, Name = g.Group.Name});
            }
            return groups;
        }

        [ClaimsAccess(ClaimType = "User", Value = "CanCreateUser")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request,
            User user) 
        {
            try
            {
                if (user != null)
                {
                    if (user.ClientCenterId == null)
                        user.ClientCenterId = 0;
                    if (user.Password.Length < 6)
                    {
                        ModelState.AddModelError("Error", "Password must be more than 6 characters");
                        return Json(ModelState.ToDataSourceResult());
                    }
                    var responseUser = _userService.FindById(new UserFindByIdRequest() {Id = user.Id});
                    //var user = new EditVM();
                    if (responseUser.IsSuccess == false)
                    {
                        List<GroupUserVM> gu = null;
                        if (user.Groups != null)
                        {
                            gu = new List<GroupUserVM>();
                            foreach (var g in user.Groups)
                            {
                                gu.Add(new GroupUserVM()
                                       {
                                           GroupId = g.Id,
                                           UserId = user.Id
                                       });
                            }
                        }
                        var responseCreate = _userService.Insert(
                            new UserInsertRequest
                            {
                                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                entity = new RegistrationVM()
                                         {
                                             Id = user.Id,
                                             UserName = user.UserName,
                                             ClientCenterId = user.ClientCenterId,
                                             Password = user.Password,
                                             GroupUsers = gu
                                         }
                            });

                        if (responseCreate.IsSuccess)
                        {
                           
                            return Json(new[] { ToUser(new UserVM()
                                                       {
                                                           Id=responseCreate.entity.Id,
                                                           ClientCenterId=responseCreate.entity.ClientCenterId,
                                                           UserName=responseCreate.entity.UserName,
                                                           GroupUsers = responseCreate.entity.GroupUsers,
                                                           
                                                       }
                                )}.ToDataSourceResult(request, ModelState));
                        }
                        else
                        {
                            ModelState.AddModelError("Error", responseCreate.Message);
                            return Json(ModelState.ToDataSourceResult());
                        }

                    }
                    RedirectToAction("Index");
                    return Json(new[] {user}.ToDataSourceResult(request, ModelState));
                    //}
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return Json(ModelState.ToDataSourceResult());


        }

        [ClaimsAccess(ClaimType = "User", Value = "CanEditUser")]
        public ActionResult Update([DataSourceRequest] DataSourceRequest dsRequest, User user)
        {
            if (user != null && ModelState.IsValid)
            {
                if (user.Password!=null && user.Password.Length < 6)
                {
                    ModelState.AddModelError("Error", "Password must be more than 6 characters");
                    return Json(ModelState.ToDataSourceResult());
                }
                var gu = new List<GroupUserVM>();
                foreach (var g in user.Groups)
                {
                    gu.Add(new GroupUserVM()
                           {
                               GroupId = g.Id,
                               UserId = user.Id
                           });
                }
                new List<GroupUserVM>();
                var responseCreate = _userService.Update(
                    new UserUpdateRequest
                    {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = new EditVM()
                                 {
                                     ClientCenterId = user.ClientCenterId,
                                     GroupUsers = gu,
                                     Id = user.Id,
                                     UserName = user.UserName,
                                     Password = user.Password
                                 }
                    });
                if (responseCreate.IsSuccess)
                {
                    return Json(new[] {ToUser(new UserVM()
                                                       {
                                                           Id=responseCreate.entity.Id,
                                                           ClientCenterId=responseCreate.entity.ClientCenterId.Value,
                                                           UserName=responseCreate.entity.UserName,
                                                           GroupUsers = responseCreate.entity.GroupUsers,

                                                       })}.ToDataSourceResult(dsRequest, ModelState));
                }
                else
                {
                    ModelState.AddModelError("Error", responseCreate.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [ClaimsAccess(ClaimType = "User", Value = "CanDeleteUser")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, User user)
        {
            var result = _userService.DeleteById(
                
                new UserDeleteByIdRequest {Id = user.Id,
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(), });
            if (result.IsSuccess)
            {
                return Json(new[] {user}.ToDataSourceResult(request));
            }
            else
            {
                ModelState.AddModelError("Error", result.Message);
                return Json(ModelState.ToDataSourceResult());
            }

        }

        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');

            foreach (var id in ids)
            {
                var responseTypeofFile = _userService.DeleteById(
                    new UserDeleteByIdRequest()
                    {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        Id = id
                    });
                if (responseTypeofFile.IsSuccess)
                {

                }
                else
                {
                    m.IsSuccess = false;
                    m.Message = MessageResource.DeletesField;

                }
            }
            return Json(m, JsonRequestBehavior.AllowGet);
        }

    }
}