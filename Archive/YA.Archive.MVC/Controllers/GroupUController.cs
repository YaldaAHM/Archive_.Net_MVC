using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models.Group;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace YA.Archive.MVC2.Controllers
{
    public class GroupUController : Controller
    {
        // GET: GroupU
        private GroupService _groupService;
        private ClientCenterService _clientCenterService;

        public GroupUController()
        {
            _groupService = new GroupService();
            _clientCenterService = new ClientCenterService();
        }

        public ActionResult Index()
        {
            IList<ClientCenterVM> ClientCenter = new List<ClientCenterVM>();

            var responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest { });
            if (responseClientCenter.IsSuccess)
            {
                 foreach (var e in responseClientCenter.Entities.ToList())
                {
                    ClientCenter.Add(new ClientCenterVM() { Id = e.Id, Name = e.Name });

                }
                ViewData["ClientCenter"] = ClientCenter;
            }
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest dsRequest)
        {
            var responseGroup = _groupService.FindAll(
               new GroupFindAllRequest { });
            if (responseGroup.IsSuccess)
            {
                if (responseGroup.Entities.Count() != 0)
                {
                    responseGroup.Entities.FirstOrDefault().GoupClaims = null;
                    responseGroup.Entities.FirstOrDefault().GroupUsers = null;
                }
                return Json(responseGroup.Entities.ToDataSourceResult(dsRequest));
            }
            var result = responseGroup.Entities.ToDataSourceResult(dsRequest);
            return Json(result);
        }
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, GroupUs group)
        {
            if (group != null && ModelState.IsValid)
            {
                var responseTypeofFolder = _groupService.Insert(
                    new GroupInsertRequest
                    {
                        entity = new GroupVM()
                        {
                            Id = group.Id,
                            Name = group.Name,
                            GroupUsers = group.GroupUsers,
                            ClientCenterId = group.ClientCenterId,
                            ValidityDate = group.ValidityDate,


                        }
                    });
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] { group }.ToDataSourceResult(request, ModelState));
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

        public ActionResult Update([DataSourceRequest] DataSourceRequest dsRequest, GroupUs group)
        {
            if (group != null && ModelState.IsValid)
            {
                var responseGroup = _groupService.Update(
                             new GroupUpdateRequest
                             {
                                 entity = new GroupVM()
                                 {
                                     Id = group.Id,
                                     Name = group.Name,
                                     GroupUsers = group.GroupUsers,
                                     ClientCenterId = group.ClientCenterId,
                                     ValidityDate = group.ValidityDate,

                                 }
                             });
            }

            return Json(new[] { group }.ToDataSourceResult(dsRequest, ModelState));


            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, GroupVM group)
        {
            if (group != null)
            {

                var responseGroup = _groupService.Delete(
                                            new GroupDeleteRequest { entity = group });
            }

            return Json(new[] { group }.ToDataSourceResult(request, ModelState));
        }

    }
}