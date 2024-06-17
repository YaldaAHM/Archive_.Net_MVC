using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    [Authorize(Roles = "Administrators,AdminClient")]
    public class PermissionGroupController : Controller
    {
        // GET: PermissionGroup
        PermissionClaimsGroupService _PermissionClaimsGroupService;
        private UserManagementService _UserService;
        private GroupService _groupService;
        public PermissionGroupController()
        {
            _PermissionClaimsGroupService = new PermissionClaimsGroupService();
            _UserService = new UserManagementService();
            _groupService = new GroupService();
        }

      
        public ActionResult IndexGroup(int? groupId)
        {
            if (groupId == null)
                return RedirectToAction("Index", "Group");
          var group=  _groupService.FindById(new GroupFindByIdRequest()
                                   {
                                       Id = groupId.Value
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
                                   Subject = "Group Management",
                                   Url="/Group/Index?isBack=1"
                               
                               },
                               new SiteRouting()

                               {
                                   Subject = " (  Group "+group.entity.Name+" ) Access",
                                   Url="/PermissionGroup/Index"
                                 
                               },
                           };
            ViewData["PermissionGroupSiteMap"] = mapSites;
            TempData["isbackG"] = 1;
            ViewData["GroupId"] = groupId;
            var result = _groupService.FindById(
                 new GroupFindByIdRequest { Id = groupId.Value });
             var user = result.entity; 
            var response = _PermissionClaimsGroupService.FindAll(
                new PermissionClaimsGroupFindAllRequest { Group = new GroupVM() { Id = groupId.Value } });
              return View("Index", response.Entities);
         
        }
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
           
           
            var mapSites = new List<SiteRouting>
                           {
                new SiteRouting()

                               {
                                   Subject = "Home",
                                    Url="/Home/Index"
                                   
                               },
                               new SiteRouting()

                               {
                                   Subject = "Group Management",
                                   Url="/Group/Index?isBack=1"
                                  
                               },
                               new SiteRouting()

                               {
                                   Subject = "Access Group",
                                   Url="/PermissionGroup/Index"
                         
                               },
                           };
            ViewData["PermissionGroupSiteMap"] = mapSites;
            if (TempData["GroupId"] != null)
            {
                var g = TempData["GroupId"].ToString();
                TempData["GroupId"] = g;
                var result = _groupService.FindById(
                    new GroupFindByIdRequest {Id = int.Parse(g)});
                if (result.entity != null)
                {
                    var response = _PermissionClaimsGroupService.FindAll(new PermissionClaimsGroupFindAllRequest
                                                                         {
                                                                             Group=new GroupVM()
                                                                             { Id=int.Parse(g)}
                                                                         });
                    return View(response.Entities);
                }
            }
            return View();
        }
       
        public ActionResult Read([DataSourceRequest] DataSourceRequest request,int groupId)
        {
            var result = _groupService.FindById(
                   new GroupFindByIdRequest { Id = groupId });
            if (result.entity != null)
            {
                var user = result.entity;
                var response = _PermissionClaimsGroupService.FindAll(
                    new PermissionClaimsGroupFindAllRequest { Group = new GroupVM() { Id = groupId } });
                return Json(response.Entities.ToDataSourceResult(request));
                
            }
            return Json(request);
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async System.Threading.Tasks.Task<ActionResult> Edit(
            [Bind(Prefix = "models")] IEnumerable<PermissionClaimsVM> permissions)
        {
            if (permissions != null && ModelState.IsValid)
            {
                var us = permissions.FirstOrDefault().User.UserName;
                var result =  _UserService.FindById(
                    new UserFindByIdRequest {Id = us});
                if (result.entity != null)
                {
                    var user = result.entity;
                    var response = _PermissionClaimsGroupService.Update(
                        new PermissionClaimsGroupUpdateRequest
                        {
                            Entities = permissions,
                            Group = new GroupVM()
                                    {
                                        Id = int.Parse(ViewData["GroupId"].ToString())
                                    }
                        });
                    return View("Index", response.Entities);
                }
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
        public async System.Threading.Tasks.Task<ActionResult> Update(
           [Bind(Prefix = "models")]List<PermissionClaimsVM> permission, int groupId)
        {
            if (permission != null && ModelState.IsValid)
            {
                var response = _PermissionClaimsGroupService.Update(
                    new PermissionClaimsGroupUpdateRequest()
                    {
                        Entities = permission,
                        Group = new GroupVM()
                                {
                                    Id=groupId
                                }
                    });
                 return Json(response.Entities);
            }

            return View();
        }

    }
}