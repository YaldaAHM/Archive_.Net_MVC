using YA.Archive.Service.TypeofFileMdl.Imps;
using YA.Archive.Service.TypeofFileMdl.Messaging;
using YA.Archive.Service.TypeofFileMdl.VMs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class TypeOfFileController : Controller
    {
        // GET: TypeOfFile
        private TypeofFileService _typeOfFileService;

        public TypeOfFileController()
        {
            _typeOfFileService = new TypeofFileService();
        }
        public ActionResult Index()
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
                                   Subject = "Attachment Type",
                                   Url="/TypeOfFile/Index",
                                   
                               },

                           };
            ViewData["TypeOfFileSiteMap"] = mapSites;
            return View();
        }
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanViewTypeOfFolder")]
        public ActionResult Read2([DataSourceRequest]DataSourceRequest request)
        {
            var responseTypeofFile  = _typeOfFileService.FindAll(
               new TypeofFileFindAllRequest { });
            if (responseTypeofFile.IsSuccess)
                return Json(responseTypeofFile.Entities.ToDataSourceResult(request));
            return View("Index", responseTypeofFile.Entities);
        }
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanViewTypeOfFolder")]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request,string Name)
        {
                var responseTypeofFile = _typeOfFileService.FindAll(
               new TypeofFileFindAllRequest { });
            if (responseTypeofFile.IsSuccess)
            {
               var rr=new List<TypeofFileVM>();
                var  r = responseTypeofFile.Entities.OrderByDescending(x => x.Id);
                return Json(r.ToDataSourceResult(request));

            } return View("Index", responseTypeofFile.Entities.
                    OrderByDescending(x=>x.Id));
         

        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanCreateTypeOfFolder")]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, TypeofFileVM typeofFile)
        {
            if (typeofFile != null && ModelState.IsValid)
            {
                var responseTypeofFile = _typeOfFileService.Insert(
                              new TypeofFileInsertRequest { entity = typeofFile });
                if (responseTypeofFile.IsSuccess)
                    return Json(new[] { typeofFile }.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFile.Message);
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
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, TypeofFileVM typeofFile)
        {
            if (typeofFile != null && ModelState.IsValid)
            {
                var responseTypeofFile = _typeOfFileService.Update(
                             new TypeofFileUpdateRequest { entity = typeofFile });
                if (responseTypeofFile.IsSuccess)
                {
                    return Json(new[] { typeofFile }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFile.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }

            return Json(new[] { typeofFile }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanDeleteTypeOfFolder")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, TypeofFileVM typeofFile)
        {
            if (typeofFile != null)
            {

                var responseTypeofFile = _typeOfFileService.Delete(
                    new TypeofFileDeleteRequest {entity = typeofFile});

                if (responseTypeofFile.IsSuccess)
                {
                    return Json(new[] {typeofFile}.ToDataSourceResult(request));
                }
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFile.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }
            return Json(new[] {typeofFile}.ToDataSourceResult(request, ModelState));
        }

        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');
           
            foreach (var id in ids)
            {
                var responseTypeofFile = _typeOfFileService.DeleteById(
                    new TypeofFileDeleteByIdRequest { Id = int.Parse(id) });
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