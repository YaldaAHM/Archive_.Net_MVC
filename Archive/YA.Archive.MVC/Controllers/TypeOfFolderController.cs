using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.TypeofFolderMdl.Imps;
using YA.Archive.Service.TypeofFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class TypeOfFolderController : Controller
    {
        // GET: TypeOfFolder
        private TypeofFolderService _typeOfFolderService;

        public TypeOfFolderController()
        {
            _typeOfFolderService = new TypeofFolderService();
        }

        public ActionResult Index()
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
                                   Subject = "Document Type",
                                   Url = "/TypeOfFolder/Index"
                               },

                           };
            ViewData["TypeOfFolderSiteMap"] = mapSites;
            return View();
        }

        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanViewTypeOfFolder")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var responseTypeofFolder = _typeOfFolderService.FindAll(
                new TypeofFolderFindAllRequest {});
            if (responseTypeofFolder.IsSuccess)
                return Json(responseTypeofFolder.Entities
                    .OrderByDescending(x => x.Id).ToDataSourceResult(request));
            return View("Index", responseTypeofFolder.Entities);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanCreateTypeOfFolder")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TypeofFolderVM typeofFolder)
        {
            if (typeofFolder != null && ModelState.IsValid)
            {
                var responseTypeofFolder = _typeOfFolderService.Insert(
                    new TypeofFolderInsertRequest {entity = typeofFolder});
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] {typeofFolder}.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFolder.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }
            else
            {
                return Json(ModelState.ToDataSourceResult());
            }

        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanEditTypeOfFolder")]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, TypeofFolderVM typeofFolder)
        {
            if (typeofFolder != null && ModelState.IsValid)
            {
                var responseTypeofFolder = _typeOfFolderService.Update(
                    new TypeofFolderUpdateRequest {entity = typeofFolder});
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] {typeofFolder}.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFolder.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }
            else
            {
                return Json(ModelState.ToDataSourceResult());
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanDeleteTypeOfFolder")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TypeofFolderVM typeofFolder)
        {
            if (typeofFolder != null)
            {

                var responseTypeofFolder = _typeOfFolderService.Delete(
                    new TypeofFolderDeleteRequest {entity = typeofFolder});
                if (responseTypeofFolder.IsSuccess)
                {
                    return Json(new[] {typeofFolder}.ToDataSourceResult(request));

                }
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFolder.Message);
                    return Json(ModelState.ToDataSourceResult());
                }

            }

            return Json(new[] {typeofFolder}.ToDataSourceResult(request, ModelState));
        }

        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');
            foreach (var id in ids)
            {
                var responseTypeofFolder = _typeOfFolderService.DeleteById(
                    new TypeofFolderDeleteByIdRequest {Id = int.Parse(id)});
                if (responseTypeofFolder.IsSuccess)
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
