using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.UserManagementMdl.VMs;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize(Roles = "Administrators,AdminClient")]
    public class PermissionController : Controller
    {
        // GET: Permission
        PermissionClaimsService _permissionClaimsService;
        private UserManagementService _userService;
        public PermissionController()
        {
            _permissionClaimsService = new PermissionClaimsService();
            _userService = new UserManagementService();
        }

        public  ActionResult Index(string userId)
        {
            if (userId == null)
                return RedirectToAction("Index", "Users");
           var user= _userService.FindById(new UserFindByIdRequest()
                                  {
                                      Id = userId
                                  });
            var mapSites = new List<SiteRouting>
                           {
                new SiteRouting()

                               {
                                   Subject = "Home",
                                    Url="/Home/Index"
                            
                               },
                               new SiteRouting()

                               {
                                   Subject = "Users Management",
                                   Url="/Users/Index?isBack=1",
                                   
                               },
                               new SiteRouting()

                               {
                                   Subject = " (  User "+user.entity.UserName+" ) Access",
                                   Url="/Permission/Index"
                          
                               },
                           };
            ViewData["PermissionSiteMap"] = mapSites;
            TempData["isbackU"] = 1;
           
            ViewData["UserId"] = userId;
          
            var response = _permissionClaimsService.FindAll(
                new PermissionClaimsFindAllRequest
                {
                    
                  User = new UserVM() { Id=userId}
               });
             return View("Index", response.Entities);


        }
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string userId)
        {
             var result = _userService.FindById(
                   new UserFindByIdRequest { Id = userId });
            if (result.entity != null)
            {
                var user = result.entity; 
                var response = _permissionClaimsService.FindAll(
                    new PermissionClaimsFindAllRequest { User = new UserVM() { Id = userId } });
                return Json(response.Entities.ToDataSourceResult(request));
               
            }
            return Json(request);
            
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async System.Threading.Tasks.Task<ActionResult> Edit(
            [Bind(Prefix = "models")]IEnumerable<PermissionClaimsVM> permissions, string userId)
        {
            if (permissions != null && ModelState.IsValid)
            {
                 var result = _userService.FindById(
                    new UserFindByIdRequest { Id = userId });
                if (result.entity != null)
                {
                    var user = result.entity;
                    var response = _permissionClaimsService.Update(
                        new PermissionClaimsUpdateRequest { Entities = permissions, User = user });
                    return Json(response.Entities);
                   
                }
            }

            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async System.Threading.Tasks.Task<ActionResult> Update(
                   [Bind(Prefix = "models")]List<PermissionClaimsVM> permission, string userId)
        {
            if (permission != null && ModelState.IsValid)
            {
                var response = _permissionClaimsService.Update(
                    new PermissionClaimsUpdateRequest()
                    {
                        Entities = permission,
                        User = new UserVM() { Id=userId}
                        
                    });
                return View("Index", response.Entities);

            }

            return View();
        }

        public ActionResult Intial(string UserName)
        {
            TempData["user"] = UserName;
           
            return View("Index");
        }
        public ActionResult IntialGroupUser(PermissionClaimsVM permissionGroupUserClaims)
        {
            TempData["permissionGroupUserClaims"] = permissionGroupUserClaims;

            return View("Index");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public async System.Threading.Tasks.Task<ActionResult> EditGroupUser(
           [Bind(Prefix = "models")]List<PermissionClaimsVM> permission)
        {
            if (permission != null && ModelState.IsValid)
            {
                    var response = _permissionClaimsService.UpdateGroupUser(
                        new PermissionClaimsGroupUpdateRequest
                        {
                            Entities = permission,
                           
                });
                    return View("Index", response.Entities);
                
            }

            return View();
        }

    }
}