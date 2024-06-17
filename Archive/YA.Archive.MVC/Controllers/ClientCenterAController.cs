using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.ClientCenter;
using YA.Archive.Service.ClientCenterAMdl.Imps;
using YA.Archive.Service.ClientCenterAMdl.Messaging;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ClientCenterAController : Controller
    {
        // GET: ClientCenterA
        private ClientCenterAService _clientCenterAService;

        public ClientCenterAController()
        {
            _clientCenterAService = new ClientCenterAService();
        }

        // GET: ClientCenter
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
                                   Subject = "Centers",
                                   Url = "/ClientCenter/Index"
                                 
                               },

                           };
            ViewData["ClientCenterSiteMap"] = mapSites;
            return View();
        }

        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanViewTypeOfFolder")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ClientCenterA> clList=new  List<ClientCenterA>();
            var responseClientCenter = _clientCenterAService.FindAll(
                new ClientCenterAFindAllRequest
                {
                    RequsetCurrentUserId =
                        System.Web.HttpContext.Current.User.Identity.GetUserId(),
                });
            if (responseClientCenter.IsSuccess)
            {

                var dd= responseClientCenter.Entities.ToList()
                    .OrderByDescending(x => x.Id);
                foreach (var d in dd)
                {
                    clList.Add(new ClientCenterA()
                               {
                                   Id=d.Id,
                                   Name=d.Name,
                                   UserName=d.UserName,
                                   UserId=d.UserId
                               });
                }
                return Json(clList.ToDataSourceResult(request));
            }
            return View("Index", clList);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanCreateTypeOfFolder")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, ClientCenterA clientCenter)// ClientCenterAVM clientCenter)
        {
            if (clientCenter != null && ModelState.IsValid)
            {
                if (clientCenter.Password.Length < 6)
                {
                    ModelState.AddModelError("Error", "Password must be more than 6 characters");
                    return Json(ModelState.ToDataSourceResult());
                }
                var responseClientCenter = _clientCenterAService.Insert(
                    new ClientCenterAInsertRequest {entity = new ClientCenterAVM()
                                                             {
                                                                 Name=clientCenter.Name,
                                                                 UserName=clientCenter.UserName,
                                                                 Password=clientCenter.Password,
                                                                 UserId=clientCenter.UserId,
                                                             },
                                                             RequsetCurrentUserId =
                                                              System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    });
                if (responseClientCenter.IsSuccess)
                    return Json(new[] {responseClientCenter.entity}.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseClientCenter.Message);
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
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, ClientCenterA clientCenter)// ClientCenterAVM clientCenter)
        {
            if (clientCenter != null && ModelState.IsValid)
            {
                if (clientCenter.Password != null && clientCenter.Password.Length < 6)
                {
                    ModelState.AddModelError("Error", "Password must be more than 6 characters");
                    return Json(ModelState.ToDataSourceResult());
                }
                var responseClientCenter = _clientCenterAService.Update(
                    new ClientCenterAUpdateRequest
                    {
                    entity = new ClientCenterAVM()
                    {
                        Name = clientCenter.Name,
                        UserName = clientCenter.UserName,
                        Password = clientCenter.Password,
                        ClientCenterId=clientCenter.Id,
                        Id=clientCenter.Id,
                        UserId=clientCenter.UserId
                    },
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    });
                if (responseClientCenter.IsSuccess)
                {
                    return Json(new[] { responseClientCenter.entity }.ToDataSourceResult(request));

                }
                else
                {
                    ModelState.AddModelError("Error", responseClientCenter.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }

            return Json(new[] {clientCenter}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "TypeOfFolder", Value = "CanDeleteTypeOfFolder")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, ClientCenterA clientCenter)// ClientCenterAVM clientCenter)
        {
            if (clientCenter != null)
            {

                var responseClientCenter = _clientCenterAService.Delete(
                    new ClientCenterADeleteRequest
                    {RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = new ClientCenterAVM()
                                 {
                            Name = clientCenter.Name,
                            UserName = clientCenter.UserName,
                            Password = clientCenter.Password,
                            ClientCenterId = clientCenter.Id,
                            Id = clientCenter.Id,
                            UserId = clientCenter.UserId
                        }
                    
                    });
                if (responseClientCenter.IsSuccess)
                {
                    return Json(new[] {clientCenter}.ToDataSourceResult(request));

                }
                else
                {
                    ModelState.AddModelError("Error", responseClientCenter.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }

            return Json(new[] {clientCenter}.ToDataSourceResult(request, ModelState));
        }


        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');

            foreach (var id in ids)
            {
                var responseTypeofFile = _clientCenterAService.DeleteById(
                    new ClientCenterADeleteByIdRequest() {Id = int.Parse(id)});
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