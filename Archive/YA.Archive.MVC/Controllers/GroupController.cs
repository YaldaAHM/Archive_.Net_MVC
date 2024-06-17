using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.Group;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        // GET: Group
        private GroupService _groupService;
        private ClientCenterService _clientCenterService;
        public GroupController()
        {
            _groupService = new GroupService();
            _clientCenterService = new ClientCenterService();
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
                                    Url="/Home/Index"
                                  
                               },
                               new SiteRouting()

                               {
                                   Subject = "Group Management",
                                   Url="/Group/Index"
                                
                               },

                           };
            ViewData["GroupSiteMap"] = mapSites;
            TempData["isbackG"] = isBack;
           
            ViewBag.TypeOfFilelist = GetTypeOfFile(null);
            Session["ValidityDate"] = "";
            IList<ClientCenterVM> ClientCenter = new List<ClientCenterVM>();

            var responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest { RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(), });
            if (responseClientCenter.IsSuccess)
            {
                  foreach (var e in responseClientCenter.Entities.ToList())
                {
                    
                    ClientCenter.Add(new ClientCenterVM() { Id = e.Id, Name = e.Name });

                }
                TempData["ClientCenterOne"] = ClientCenter.First().Id;
                ViewData["ClientCenter"] = ClientCenter;
            }
            return View();
        }
        
        [ClaimsAccess(ClaimType = "Group", Value = "CanViewGroup")]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request,
             int? sClientCenterId,  string sName, bool sIsClickSearch)
        {
            Session["ValidityDate"] = "";

    

            if (sName=="" && sClientCenterId == null && sIsClickSearch == false)
            {
               
                    var oldS = (GroupVM)TempData["oldSearchBackG"];

                    if (oldS != null)
                    {
                        TempData["oldSearchBackG"] = oldS;

                        if (oldS.Name == "" && oldS.ClientCenterId == 0)
                        {
                            return Json(ModelState.ToDataSourceResult());
                        }
                        if (oldS.Name == "*")
                        {
                            var responseGroup1 = _groupService.FindAll(
                                new GroupFindAllRequest()
                                {
                                //ClientCenterType = ClientCenterType.Main,
                                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),

                                });
                            if (responseGroup1.IsSuccess)
                            {
                                var gg = new List<GroupUs>();
                                if (responseGroup1.Entities.Count() != 0)
                                    foreach (var r in responseGroup1.Entities.OrderByDescending(c => c.Id))
                                    {


                                        r.GoupClaims = null;
                                        r.GroupUsers = null;
                                        gg.Add(new GroupUs()
                                        {
                                            Id = r.Id,
                                            ClientCenterId = r.ClientCenterId,
                                            Name = r.Name,
                                            ValidityDate = r.ValidityDate
                                        });
                                    }


                            }
                            return
                                Json(responseGroup1.Entities.OrderByDescending(c => c.Id)
                                    .ToDataSourceResult(request, ModelState));

                        }
                        var responseGroup2 = _groupService.FindByExpression(
        new GroupFindByExpressionExpRequest()
        {
       
        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
            expression = x =>
            (oldS.Name == "" || x.Name.Contains(oldS.Name)) &&
            (oldS.ClientCenterId == 0 || x.ClientCenterId == oldS.ClientCenterId)

        });
                        if (responseGroup2.IsSuccess)
                        {
                            var g = new List<GroupUs>();
                            if (responseGroup2.Entities.Count() != 0)
                                foreach (var r in responseGroup2.Entities.OrderByDescending(c => c.Id))
                                {
                                    r.GoupClaims = null;
                                    r.GroupUsers = null;
                                    g.Add(new GroupUs()
                                    {
                                        Id = r.Id,
                                        ClientCenterId = r.ClientCenterId,
                                        Name = r.Name,
                                        ValidityDate = r.ValidityDate
                                    });
                                    
                                }
                        }
                           return Json(responseGroup2.Entities.OrderByDescending(c => c.Id).ToDataSourceResult(request, ModelState));
                    }
              

                return Json(ModelState.ToDataSourceResult());
            }
            if (sName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackG"] = new GroupVM()
                {
                      Name = sName,
                        ClientCenterId = sClientCenterId ?? 0,
                        GroupUsers = null,
                };
                var responseGroup1 = _groupService.FindAll(
                    new GroupFindAllRequest()
                    {
                         RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                 
                    });
                if (responseGroup1.IsSuccess)
                {
                    var gg = new List<GroupUs>();
                    if (responseGroup1.Entities.Count() != 0)
                        foreach (var r in responseGroup1.Entities.OrderByDescending(c => c.Id))
                        {


                            r.GoupClaims = null;
                            r.GroupUsers = null;
                            gg.Add(new GroupUs()
                                   {
                                       Id = r.Id,
                                       ClientCenterId = r.ClientCenterId,
                                       Name = r.Name,
                                       ValidityDate = r.ValidityDate
                                   });
                        }
                   

                } return
                        Json(responseGroup1.Entities.OrderByDescending(c => c.Id)
                            .ToDataSourceResult(request, ModelState));
            }
           
            TempData["oldSearchBackG"] = new GroupVM()
            {
                Name = sName,
                    ClientCenterId = sClientCenterId ?? 0,
                    GroupUsers = null,

            };
            var responseGroup = _groupService.FindByExpression(
       new GroupFindByExpressionExpRequest()
       {
               RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
           expression = x => 
           (sName == "" || x.Name.Contains(sName)) &&
           (sClientCenterId == null || x.ClientCenterId == sClientCenterId) 
           
       });
            if (responseGroup.IsSuccess)
            {
                var g = new List<GroupUs>();
                if (responseGroup.Entities.Count() != 0)
                    foreach (var r in responseGroup.Entities.OrderByDescending(c=>c.Id))
                    {


                        r.GoupClaims = null;
                        r.GroupUsers = null;
                        g.Add(new GroupUs()
                              {
                                  Id = r.Id,
                                  ClientCenterId = r.ClientCenterId,
                                  Name = r.Name,
                                  ValidityDate = r.ValidityDate
                              });
                       
                    }
            }
        
                        return Json(responseGroup.Entities.OrderByDescending(c=>c.Id).ToDataSourceResult(request, ModelState));
           
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "Group", Value = "CanCreateGroup")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, GroupUs group1)
        {
            if (group1 != null && ModelState.IsValid)
            {
                var idcc =TempData["ClientCenterOne"];
                int idc = 0;
                if (idcc != null)
                {
                    idc = (int) idcc;
                    TempData["ClientCenterOne"] = idc;
                }
                if (group1.ClientCenterId == 0)
                {
                    group1.ClientCenterId = idc;
                }
                string ValidityDateT = "";
                if (Session["ValidityDate"] == null)
                {
                    ValidityDateT = "5555/10/10";
                }
               else if (Session["ValidityDate"].ToString() == "1300/10/10")
                {
                    ValidityDateT = "5555/10/10";
                }
                else if (Session["ValidityDate"].ToString() == "1100/11/22" ||
                         Session["ValidityDate"].ToString() == ""
                         || Session["ValidityDate"] == null)
                {
                    ValidityDateT = group1.ValidityDate;
                    if (group1.ValidityDate == null)
                    {
                        ValidityDateT = "5555/10/10";
                    }
                }

                group1.ValidityDate = ValidityDateT;
                var responseTypeofFolder = _groupService.Insert(
                    new GroupInsertRequest {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = new GroupVM()
                                                      {
                        Id = group1.Id,
                        ClientCenterId = group1.ClientCenterId,
                        Name = group1.Name,
                        ValidityDate = group1.ValidityDate,
                        GroupUsers=group1.GroupUsers
                    }  });
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] { responseTypeofFolder.entity }.ToDataSourceResult(request, ModelState));
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
       [ClaimsAccess(ClaimType = "Group", Value = "CanEditGroup")]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, GroupUs group1)
        {
            if (group1 != null && ModelState.IsValid)
            {
                string ValidityDateT = "";
                if (Session["ValidityDate"].ToString() == "1300/10/10")
                {
                    ValidityDateT = "5555/10/10";
                }
                else if (Session["ValidityDate"].ToString() == "1100/11/22" ||
                         Session["ValidityDate"].ToString() == ""
                         || Session["ValidityDate"] == null)
                {
                    ValidityDateT = group1.ValidityDate;
                    if (group1.ValidityDate == null)
                    {
                        ValidityDateT = "5555/10/10";
                    }
                }

                group1.ValidityDate = ValidityDateT;
                var responseGroup = _groupService.Update(
                             new GroupUpdateRequest {
                                 RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                 entity = new GroupVM()
                                                      {
                        Id = group1.Id,
                        ClientCenterId = group1.ClientCenterId,
                        Name = group1.Name,
                        ValidityDate = group1.ValidityDate,
                        GroupUsers=group1.GroupUsers
                    }  });
                if (responseGroup.IsSuccess)
                    return Json(new[] { group1 }.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseGroup.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }
            else
            {
                return Json(ModelState.ToDataSourceResult());
            }
          
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "Group", Value = "CanDeleteGroup")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, GroupUs group1)
        {
            if (group1 != null)
            {

                var responseGroup = _groupService.Delete(
                                            new GroupDeleteRequest { entity = new GroupVM()
                                            {
                                                Id = group1.Id,
                                                ClientCenterId = group1.ClientCenterId,
                                                Name = group1.Name,
                                                ValidityDate = group1.ValidityDate,
                                                GroupUsers = group1.GroupUsers
                                            }
                                            });

                if (responseGroup.IsSuccess)
                    return Json(new[] { group1 }.ToDataSourceResult(request, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseGroup.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }

            return Json(new[] { group1 }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');

            foreach (var id in ids)
            {
                var responseTypeofFile = _groupService.DeleteById(
                    new GroupDeleteByIdRequest() { Id = int.Parse(id) });
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
        public ActionResult EditUserInGroup(int id)
        {

            var result = _groupService.FindGroupsUser(
                new GroupUserFindRequest() { Id = id });
            if (result.IsSuccess)
            {
                return View(result.entity);
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult EditUserInGroup(GroupModificationVM model)
        {
            //if (ModelState.IsValid)
            //{
            var result = _groupService.UpdateUsersInGroup(
                new GroupUserUpdateRequest() { entity = model });
            var result1 = _groupService.FindGroupsUser(
                new GroupUserFindRequest() { Id = model.GroupId });
            if (result1.IsSuccess)
            {
                return View(result1.entity);
            }
            
            return View("Error", new string[] { "Group Not Found" });
        }
        public JsonResult GetGroupsd(int? Id)
        {

            if (Id != null)
            {
                var responseC = _groupService.FindByExpression(
               new GroupFindByExpressionRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                   expression = x => x.ClientCenterId == Id
               });
               
                return Json(responseC.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
            }

            

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Action2(List<string> idList)
        {
            return Json("");
        }
        public JsonResult GetGroups(int? value,int? supplierId,int? field,
            
            string CompanyName,int? SupplierID)
        {

          
            if (field == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            if (field == 0 && value == 0 && CompanyName == "true")
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            if (field == 0 && value != 0 && value != null)
            {
                field = value;
            }
                var responseC = _groupService.FindByExpression(
               new GroupFindByExpressionExpRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                   expression = x =>(field==0|| x.ClientCenterId == field)//&& x.FolderId == null 
               });
                 return Json(responseC.Entities.Select(p => new { Id = p.Id, Name = p.Name
    }), JsonRequestBehavior.AllowGet);
           
        }
        
        public JsonResult GetGroupsddd(string Id)
        {

            if (Id != null)
            {
                var responseC = _groupService.FindByExpression(
               new GroupFindByExpressionRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                   expression = x => x.ClientCenterId ==int.Parse(Id) //&& x.FolderId == null 
               });
                
                return Json(responseC.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
            }

           

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGroupsdd(int Id)
        {

            if (Id != null)
            {
                var responseC = _groupService.FindByExpression(
               new GroupFindByExpressionRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                   expression = x => x.ClientCenterId == Id//&& x.FolderId == null 
               });
                  return Json(responseC.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
            }

          

            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGroupss()
        {

            
                var responseC = _groupService.FindAll(
               new GroupFindAllRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
               
               });
               
                return Json(responseC.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
           

            
           
        }
        public ActionResult CommentsValidityDate(string ValidityDate)
        {
            Session["ValidityDate"] = ValidityDate;
            return Json("1390");

        }
        

    }
}