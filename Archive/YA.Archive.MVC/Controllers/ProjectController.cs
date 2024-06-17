﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Localization.TypeOfProjectLocalization;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.Folder;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.Imps;
using YA.Archive.Service.TypeofFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        // GET: Project
        private FolderService _folderService;
        private ClientCenterService _clientCenterService;
        private TypeofFolderService _typeOfFolderService;

        public ProjectController()
        {
            _folderService = new FolderService();
            _clientCenterService = new ClientCenterService();
            _typeOfFolderService = new TypeofFolderService();
        }
        public List<Project> projects = new List<Project>();

        public List<Project> GetProjects()
        {
            var responseFolder = _folderService.FindAll(
            new FolderFindAllRequest()
            {
                //ClientCenterType = ClientCenterType.Main,
                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
        });
            foreach (var r in responseFolder.Entities.OrderByDescending(n=>n.LastUpdateDate))
            {
                projects.Add(new Project
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    KeyWorddd = r.KeyWord,
                    Code = r.Code,
                    ClientCenterId = r.ClientCenterId,
                    EditLockDate = r.IsEditLockDate == true? r.EditLockDate:null,
                    RemoveLockDate = r.IsRemoveLockDate == true ? r.RemoveLockDate : null,
                    TypeofFolders = TypeofFolderList(r.TypesOfFolders),
                    EditCommentLockDate= r.IsEditCommentLockDate == true ? r.EditCommentLockDate : null,
                    RemoveCommentLockDate= r.IsRemoveCommentLockDate == true ? r.RemoveCommentLockDate : null,
                    EditFileLockDate= r.IsEditFileLockDate == true ? r.EditFileLockDate : null,
                    RemoveFileLockDate= r.IsRemoveFileLockDate == true ? r.RemoveFileLockDate : null,
                    IsRemoveCommentLockDate = r.IsRemoveCommentLockDate,
                    IsRemoveFileLockDate = r.IsRemoveFileLockDate,
                    IsRemoveLockDate = r.IsRemoveLockDate,
                    IsEditCommentLockDate = r.IsEditCommentLockDate,
                    IsEditFileLockDate = r.IsEditFileLockDate,
                    IsEditLockDate = r.IsEditLockDate,
                    IsDeleteCommentLockCreateUser = r.IsDeleteCommentLockCreateUser,
                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                    IsUpdateCommentLockCreateUser = r.IsUpdateCommentLockCreateUser,
                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
                    CreateDate = r.CreateDate,
                    LastUpdateDate=r.LastUpdateDate,
                    CreateUserName = r.CreateUser.UserName,
                    LastUpdateUserName = r.LastUpdateUser.UserName,

                    EditLockDateH = r.IsEditLockDate == true ? r.EditLockDate : null,
                    RemoveLockDateH = r.IsRemoveLockDate == true ? r.RemoveLockDate : null,
                    EditCommentLockDateH = r.IsEditCommentLockDate == true ? r.EditCommentLockDate : null,
                    RemoveCommentLockDateH = r.IsRemoveCommentLockDate == true ? r.RemoveCommentLockDate : null,
                    EditFileLockDateH = r.IsEditFileLockDate == true ? r.EditFileLockDate : null,

                });

            }

            return projects;
        }

        public List<Project> ToProject(List<FolderVM> folders)
        {
            projects=new List<Project>();
            foreach (var r in folders.OrderByDescending(n => n.LastUpdateDate))
            {
                projects.Add(new Project
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    KeyWorddd = r.KeyWord,
                    Code = r.Code,
                    ClientCenterId = r.ClientCenterId,
                    EditLockDate =  r.EditLockDate ,
                    RemoveLockDate =  r.RemoveLockDate,
                    TypeofFolders = TypeofFolderList(r.TypesOfFolders),
                    EditCommentLockDate =  r.EditCommentLockDate,
                    RemoveCommentLockDate =  r.RemoveCommentLockDate,
                    EditFileLockDate =  r.EditFileLockDate,
                    RemoveFileLockDate = r.RemoveFileLockDate,
                    IsRemoveCommentLockDate = r.IsRemoveCommentLockDate,
                    IsRemoveFileLockDate = r.IsRemoveFileLockDate,
                    IsRemoveLockDate = r.IsRemoveLockDate,
                    IsEditCommentLockDate = r.IsEditCommentLockDate,
                    IsEditFileLockDate = r.IsEditFileLockDate,
                    IsEditLockDate = r.IsEditLockDate,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,
                    CreateUserName = r.CreateUser.UserName,
                    LastUpdateUserName = r.LastUpdateUser.UserName,

                    EditLockDateH = r.IsEditLockDate == true ? r.EditLockDate : null,
                    RemoveLockDateH = r.IsRemoveLockDate == true ? r.RemoveLockDate : null,
                    EditCommentLockDateH = r.IsEditCommentLockDate == true ? r.EditCommentLockDate : null,
                    RemoveCommentLockDateH = r.IsRemoveCommentLockDate == true ? r.RemoveCommentLockDate : null,
                    EditFileLockDateH = r.IsEditFileLockDate == true ? r.EditFileLockDate : null,

                    HasClaimEdit = r.HasClaimEdit,
                    HasClaimDelete = r.HasClaimDelete,
                    HasClaimInsert = r.HasClaimInsert,
                    HasClaimView = r.HasClaimView,
                    HasClaimUpdateLock = r.HasClaimUpdateLock,
                    HasClaimDeleteLock = r.HasClaimDeleteLock,
                    HasClaimUpdateFileLock = r.HasClaimUpdateFileLock,
                    HasClaimDeleteFileLock = r.HasClaimDeleteFileLock,
                    HasClaimUpdateCommentLock = r.HasClaimUpdateCommentLock,
                    HasClaimDeleteCommentLock = r.HasClaimDeleteCommentLock,
                    IsUpdateLock = r.IsUpdateLock,
                    IsDeleteLock = r.IsDeleteLock,
                    IsUpdateFileLock = r.IsUpdateFileLock,
                    IsDeleteFileLock = r.IsDeleteFileLock,

                    IsDeleteCommentLockCreateUser = r.IsDeleteCommentLockCreateUser,
                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                    IsUpdateCommentLockCreateUser = r.IsUpdateCommentLockCreateUser,
                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
                });

            }
            return projects;
        }
        public Project ToProject(FolderVM r)
        {
                return new Project()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    KeyWorddd = r.KeyWord,
                    Code = r.Code,
                    ClientCenterId = r.ClientCenterId,
                     TypeofFolders = TypeofFolderList(r.TypesOfFolders),
                     IsRemoveCommentLockDate = r.IsRemoveCommentLockDate,
                    IsRemoveFileLockDate = r.IsRemoveFileLockDate,
                    IsRemoveLockDate = r.IsRemoveLockDate,
                    IsEditCommentLockDate = r.IsEditCommentLockDate,
                    IsEditFileLockDate = r.IsEditFileLockDate,
                    IsEditLockDate = r.IsEditLockDate,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,
                    CreateUserName = r.CreateUser.UserName,
                    LastUpdateUserName=r.LastUpdateUser.UserName,

                    EditLockDateH = r.IsEditLockDate == true ? r.EditLockDate : null,
                    RemoveLockDateH = r.IsRemoveLockDate == true ? r.RemoveLockDate : null,
                    EditCommentLockDateH = r.IsEditCommentLockDate == true ? r.EditCommentLockDate : null,
                    RemoveCommentLockDateH = r.IsRemoveCommentLockDate == true ? r.RemoveCommentLockDate : null,
                    EditFileLockDateH = r.IsEditFileLockDate == true ? r.EditFileLockDate : null,
                    HasClaimEdit = r.HasClaimEdit,
                    HasClaimDelete = r.HasClaimDelete,
                    HasClaimInsert = r.HasClaimInsert,
                    HasClaimView = r.HasClaimView,
                    HasClaimUpdateLock = r.HasClaimUpdateLock,
                    HasClaimDeleteLock = r.HasClaimDeleteLock,
                    HasClaimUpdateFileLock = r.HasClaimUpdateFileLock,
                    HasClaimDeleteFileLock = r.HasClaimDeleteFileLock,
                    HasClaimUpdateCommentLock = r.HasClaimUpdateCommentLock,
                    HasClaimDeleteCommentLock = r.HasClaimDeleteCommentLock,
                    IsUpdateLock = r.IsUpdateLock,
                    IsDeleteLock = r.IsDeleteLock,
                    IsUpdateFileLock = r.IsUpdateFileLock,
                    IsDeleteFileLock = r.IsDeleteFileLock,

                    IsDeleteCommentLockCreateUser = r.IsDeleteCommentLockCreateUser,
                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                    IsUpdateCommentLockCreateUser = r.IsUpdateCommentLockCreateUser,
                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,

                };
                
        }
        private List<TypeofFolderVM> TypeofFolderList(List<TypesOfFolderVM> gu)
        {
            List<TypeofFolderVM> typesOfFolders = new List<TypeofFolderVM>();
            foreach (var g in gu)
            {
                typesOfFolders.Add(new TypeofFolderVM() { Id = g.TypeofFolderId, Name = g.TypeofFolder.Name });
            }
            return typesOfFolders;
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
                                   Subject = "Documents",
                                   Url="/Project/Index"
                               },
                              
                           };
            ViewData["ProjectSiteMap"] = mapSites;
            Session["EditLockDate"] = "";
            Session["RemoveLockDate"] = "";
            Session["EditFileLockDate"] = "";
            Session["RemoveFileLockDate"] = "";
            Session["EditCommentLockDate"] = "";
            Session["RemoveCommentLockDate"] = "";
            List<TypeofFolderVM> typeList = new List<TypeofFolderVM>();

            var responseType = _typeOfFolderService.FindAll(
               new TypeofFolderFindAllRequest { });
            if (responseType.IsSuccess)
            {
                foreach (var e in responseType.Entities.ToList())
                {
                    typeList.Add(new TypeofFolderVM() { Id = e.Id, Name = e.Name });

                }
                ViewData["TypeofFolder"] = typeList;
            }
            var responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest { RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(), });
            if (responseClientCenter.IsSuccess)
            {
            

               
            }
            TempData["isbackP"] = isBack;
            ViewData["ClientCenter"] = responseClientCenter.Entities.ToList();

           
            return View();
        }

        [ClaimsAccess(ClaimType = "Project", Value = "CanViewProject")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest dsRequest,
            int? sClientCenterId, List<TypeofFolderVM> sTypeofFolders,
            string sCode, string sName, string sKeyWords
            , string sStartDate, string sEndDate, FormCollection form,bool sIsClickSearch)
        {

            var projects = new List<Project>();
           
            Session["EditLockDate"] = "";
            Session["RemoveLockDate"] = "";
            Session["EditFileLockDate"] = "";
            Session["RemoveFileLockDate"] = "";
            Session["EditCommentLockDate"] = "";
            Session["RemoveCommentLockDate"] = "";
        
            int currentPage = dsRequest.Page;
      





            if (sCode == "" && sClientCenterId == null
                && form["STypeofFolders[0]"] == null
                && sName == "" && sKeyWords == "" && sStartDate == "" && sEndDate == "" && sIsClickSearch==false)
            {
                //return Json(ModelState.ToDataSourceResult());
                var oldS = (ReportClientFolderRRequest)TempData["oldSearchBackP"];

                if (oldS != null)
                {
                    
                        TempData["oldSearchBackP"] = oldS;
                        if (oldS.entity.Code == "*" || oldS.entity.Name == "*")
                        {

                            var responseFolder1 = _folderService.FindAllPaging(
                                new FolderFindAllRequest()
                                {
                                    //ClientCenterType = ClientCenterType.Main,
                                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                    Index = currentPage, Count = 10,
                                });
                            if (!responseFolder1.IsSuccess)
                            {
                                ModelState.AddModelError("Error", responseFolder1.Message);
                                return Json(ModelState.ToDataSourceResult());
                            }
                            projects = ToProject(responseFolder1.Entities.ToList());

                            //var result2 = projects.OrderByDescending(p => p.Id).ToDataSourceResult(dsRequest);
                            
                        var resultPaging2 = new DataSourceResult()
                        {
                            Data = projects,
                            Total = responseFolder1.TotalRecords
                        };
                        if (responseFolder1.TotalRecords < (currentPage * 10))
                        {
                            dsRequest.Page = 1;
                        }
                        return Json(resultPaging2);
                    }
                        var startDate1 = fpub.convert2miladi(oldS.FromDate);
                        var endDate1 = fpub.convert2miladi(oldS.ToDate);
 int total1 = 0;
                       
                            var typeofFolderId1 = oldS.entity.TypesOfFolders.Select(c => c.TypeofFolderId).ToArray();
                    var tp1 = (typeofFolderId1.Any()) ? typeofFolderId1.ToList().Count() : 0;
                   
                    var responseFolder11 = _folderService.FindByExpressionPaging(
                                    new FolderFindByExpressionExpRequest()
                                    {

                                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                        expression = x =>
                                              (oldS.entity.Code == "" || x.Code.Contains(oldS.entity.Code)) &&
                                                          (oldS.entity.Name == "" || x.Name.Contains(oldS.entity.Name)) &&
                                                          (oldS.entity.ClientCenterId == 0 ||
                                                           x.ClientCenterId == oldS.entity.ClientCenterId) &&
                                                          (oldS.entity.KeyWord == "" ||
                                                           x.KeyWord.Contains(oldS.entity.KeyWord)) &&
                                                          (oldS.FromDate == "" || x.CreateDate >= startDate1) &&
                                                          (oldS.FromDate == "" || x.CreateDate <= endDate1) &&
                                                      (tp1 == 0 ||
                                              (x.TypesOfFolders.Any()

                                              && x.TypesOfFolders.Count() != 0 &&
                                              x.TypesOfFolders.Any(s => typeofFolderId1.Contains(s.TypeofFolderId))))

                                 
                                 ,
                                        Index = currentPage,
                                        Count = 10,

                                    });
                                if (!responseFolder11.IsSuccess)
                                {
                                    ModelState.AddModelError("Error", responseFolder11.Message);
                                    return Json(ModelState.ToDataSourceResult());
                                }
                               
                                projects.AddRange(ToProject(responseFolder11.Entities.ToList()));
                                total1 = responseFolder11.TotalRecords;
                           
                    var resultPaging = new DataSourceResult()
                    {
                        Data = projects,
                        Total = total1,
                    };
                    if (total1 < (currentPage*10))
                    {
                        dsRequest.Page = 1;
                    }
                    return Json(resultPaging);

                   
                }
                return Json(ModelState.ToDataSourceResult());
            }




            if (sCode == "*" || sName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackP"] = new ReportClientFolderRRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new FolderExpVM()
                    {
                        Name = sName,
                        ClientCenterId = sClientCenterId ?? 0,
                        Code = sCode,
                        KeyWord = sKeyWords,
                        TypesOfFolders = new List<TypesOfFolderVM>(),
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                };
                TempData["reportProjectInitialize"] = new ReportClientFolderRRequest()
                                                      {
                                                          RequsetCurrentUserId =
                                                              System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                                          entity = new FolderExpVM()
                                                                   {
                                                                       Name = "",
                                                                       ClientCenterId = sClientCenterId ?? 0,
                                                                       Code = "",
                                                                       KeyWord = sKeyWords,
                                                                       TypesOfFolders = new List<TypesOfFolderVM>(),
                                                                   },
                                                          FromDate = sStartDate,
                                                          ToDate = sEndDate
                                                      };
                var responseFolder = _folderService.FindAllPaging(
                    new FolderFindAllRequest()
                    {
                        //ClientCenterType = ClientCenterType.Main,
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        Index = currentPage,
                        Count = 10,
                    });
                if (!responseFolder.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseFolder.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
                projects = ToProject(responseFolder.Entities.ToList());

                var resultPaging = new DataSourceResult()
                {
                    Data = projects,
                    Total = responseFolder.TotalRecords,
                };
                if (responseFolder.TotalRecords < (currentPage * 10))
                {
                    dsRequest.Page = 1;
                }
                return Json(resultPaging);
            }
            int ii = 0;
            var startDate = fpub.convert2miladi(sStartDate);
            var endDate = fpub.convert2miladi(sEndDate);
             var gu = new List<TypesOfFolderVM>();
            if (sTypeofFolders != null && form["STypeofFolders[0]"] != null)
            {
                foreach (var i in sTypeofFolders)
                {
                    gu.Add(new TypesOfFolderVM()
                           {
                               TypeofFolderId = int.Parse(form["STypeofFolders[" + ii + "]"]),
                               FolderId = 1
                           });
                    ii ++;
                }
            }
            TempData["oldSearchBackP"] = new ReportClientFolderRRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new FolderExpVM()
                    {
                        Name = sName,
                        ClientCenterId = sClientCenterId ?? 0,
                        Code = sCode,
                        KeyWord = sKeyWords,
                        TypesOfFolders = gu,

                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate

                };
                TempData["reportProjectInitialize"] = new ReportClientFolderRRequest()
                                                      {
                                                          RequsetCurrentUserId =
                                                              System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                                          entity = new FolderExpVM()
                                                                   {
                                                                       Name = sName,
                                                                       ClientCenterId = sClientCenterId ?? 0,
                                                                       Code = sCode,
                                                                       KeyWord = sKeyWords,
                                                                       TypesOfFolders = gu,

                                                                   },
                                                          FromDate = sStartDate,
                                                          ToDate = sEndDate

                                                      };
                var typeofFolderId = gu.Select(c => c.TypeofFolderId);
            var tp = (typeofFolderId.Any())?typeofFolderId.ToList().Count(): 0;
              
                    var responseFolder2 = _folderService.FindByExpressionPaging(
                        new FolderFindByExpressionExpRequest()
                        {

                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x =>
                                              (sCode == "" || x.Code.Contains(sCode)) &&
                                              (sName == "" || x.Name.Contains(sName)) &&
                                              (sClientCenterId == null || x.ClientCenterId == sClientCenterId) &&
                                              (sKeyWords == "" || x.KeyWord.Contains(sKeyWords)) &&
                                              (sStartDate == "" || x.CreateDate >= startDate) &&
                                              (sEndDate == "" || x.CreateDate <= endDate) &&
                                              (tp==0||
                                              (x.TypesOfFolders.Any() 

                                              && x.TypesOfFolders.Count() != 0 &&
                                              x.TypesOfFolders.Any(s => typeofFolderId.Contains(s.TypeofFolderId))))
                           ,                  
                            Index = currentPage,
                            Count = 10,

                        });
                    if (!responseFolder2.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseFolder2.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                    
                   
                    projects.AddRange(ToProject(responseFolder2.Entities.ToList()));
                    
            var resultPaging1 = new DataSourceResult()
            {
                Data = projects,
                Total = responseFolder2.TotalRecords
        };
            
            return Json(resultPaging1);


        }
        [ClaimsAccess(ClaimType = "Project", Value = "CanCreateProject")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request,
            Project project)
        {
            if (project != null && ModelState.IsValid)
            {
             var folder = new FolderVM();
                string EditLockDateT = "";
                string RemoveLockDateT = "";
                string EditFileLockDateT = "";
                string RemoveFileLockDateT = "";
                string EditCommentLockDateT = "";
                string RemoveCommentLockDateT = "";
                if (Session["EditLockDate"].ToString() == "1300/10/10")
                {
                    EditLockDateT = "5555/10/10";
                }
                else if (Session["EditLockDate"].ToString() == "1100/11/22" ||
                         Session["EditLockDate"].ToString() == ""
                         || Session["EditLockDate"] == null)
                {
                    EditLockDateT = project.EditLockDate;
                    if (project.EditLockDate == null)
                    {
                        EditLockDateT= "5555/10/10";
                    }
                }
                ///////////////////
                if (Session["RemoveLockDate"].ToString() == "1300/10/10")
                {
                    RemoveLockDateT = "5555/10/10";
                }
                else if (Session["RemoveLockDate"].ToString() == "1100/11/22" ||
                         Session["RemoveLockDate"].ToString() == ""
                         || Session["RemoveLockDate"] == null)
                {
                    RemoveLockDateT = project.RemoveLockDate;
                    if (project.RemoveLockDate == null)
                    {
                        RemoveLockDateT = "5555/10/10";
                    }
                }
                //////////////////////
                if (Session["EditFileLockDate"].ToString() == "1300/10/10")
                {
                    EditFileLockDateT = "5555/10/10";
                }
                else if (Session["EditFileLockDate"].ToString() == "1100/11/22" ||
                         Session["EditFileLockDate"].ToString() == ""
                         || Session["EditFileLockDate"] == null)
                {
                    EditFileLockDateT = project.EditFileLockDate;
                    if (project.EditFileLockDate == null)
                    {
                        EditFileLockDateT = "5555/10/10";
                    }
                }
                ////////////////////
                if (Session["RemoveFileLockDate"].ToString() == "1300/10/10")
                {
                    RemoveFileLockDateT = "5555/10/10";
                }
                else if (Session["RemoveFileLockDate"].ToString() == "1100/11/22" ||
                         Session["RemoveFileLockDate"].ToString() == ""
                         || Session["RemoveFileLockDate"] == null)
                {
                    RemoveFileLockDateT = project.RemoveFileLockDate;
                    if (project.RemoveFileLockDate == null)
                    {
                        RemoveFileLockDateT = "5555/10/10";
                    }
                }
                ///////////////////
                if (Session["EditCommentLockDate"].ToString() == "1300/10/10")
                {
                    EditCommentLockDateT = "5555/10/10";
                }
                else if (Session["EditCommentLockDate"].ToString() == "1100/11/22" ||
                         Session["EditCommentLockDate"].ToString() == ""
                         || Session["EditCommentLockDate"] == null)
                {
                    EditCommentLockDateT = project.EditCommentLockDate;
                    if (project.EditCommentLockDate == null)
                    {
                        EditCommentLockDateT = "5555/10/10";
                    }
                }
                /////////////////
                if (Session["RemoveCommentLockDate"].ToString() == "1300/10/10")
                {
                    RemoveCommentLockDateT = "5555/10/10";
                }
                else if (Session["RemoveCommentLockDate"].ToString() == "1100/11/22" ||
                         Session["RemoveCommentLockDate"].ToString() == ""
                         || Session["RemoveCommentLockDate"] == null)
                {
                    RemoveCommentLockDateT = project.RemoveCommentLockDate;
                    if (project.RemoveCommentLockDate == null)
                    {
                        RemoveCommentLockDateT = "5555/10/10";
                    }
                }
                
                folder.EditLockDate = EditLockDateT;
                folder.RemoveLockDate = RemoveLockDateT;
                folder.EditFileLockDate = EditFileLockDateT;
                folder.RemoveFileLockDate = RemoveFileLockDateT;
                folder.EditCommentLockDate = EditCommentLockDateT;
                folder.RemoveCommentLockDate = RemoveCommentLockDateT;



                var gu = new List<TypesOfFolderVM>();
                if(project.TypeofFolders!=null)
                foreach (var g in project.TypeofFolders)
                {
                    gu.Add(new TypesOfFolderVM()
                    {
                        TypeofFolderId = g.Id,
                        FolderId = project.Id,
                        
                    });
                }
                folder.CreateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                folder.LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                folder.Description = project.Description;
                folder.KeyWord = project.KeyWorddd;
                folder.Name = project.Name;
                folder.TypesOfFolders = gu;
                folder.ClientCenterId = project.ClientCenterId;
                var responseTypeofFolder = _folderService.Insert(
                    new FolderInsertRequest { RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = folder });
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] { ToProject( responseTypeofFolder.entity) }.ToDataSourceResult(request, ModelState));
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
        [ClaimsAccess(ClaimType = "Project", Value = "CanEditProject")]
        public ActionResult Update([DataSourceRequest] DataSourceRequest dsRequest, Project project)
        {
            try
            {
                if (project != null )
                {
                    var folder = new FolderVM();
                    string EditLockDateT = "";
                    string RemoveLockDateT = "";
                    string EditFileLockDateT = "";
                    string RemoveFileLockDateT = "";
                    string EditCommentLockDateT = "";
                    string RemoveCommentLockDateT = "";
                    if (Session["EditLockDate"].ToString() == "1300/10/10")
                    {
                        EditLockDateT = "5555/10/10";
                    }
                    else if (Session["EditLockDate"].ToString() == "1100/11/22" ||
                             Session["EditLockDate"].ToString() == ""
                             || Session["EditLockDate"] == null)
                    {
                        EditLockDateT = project.EditLockDate;
                        if (project.EditLockDate == null)
                        {
                            EditLockDateT = "5555/10/10";
                        }
                    }
                    ///////////////////
                    if (Session["RemoveLockDate"].ToString() == "1300/10/10")
                    {
                        RemoveLockDateT = "5555/10/10";
                    }
                    else if (Session["RemoveLockDate"].ToString() == "1100/11/22" ||
                             Session["RemoveLockDate"].ToString() == ""
                             || Session["RemoveLockDate"] == null)
                    {
                        RemoveLockDateT = project.RemoveLockDate;
                        if (project.RemoveLockDate == null)
                        {
                            RemoveLockDateT = "5555/10/10";
                        }
                    }
                    //////////////////////
                    if (Session["EditFileLockDate"].ToString() == "1300/10/10")
                    {
                        EditFileLockDateT = "5555/10/10";
                    }
                    else if (Session["EditFileLockDate"].ToString() == "1100/11/22" ||
                             Session["EditFileLockDate"].ToString() == ""
                             || Session["EditFileLockDate"] == null)
                    {
                        EditFileLockDateT = project.EditFileLockDate;
                        if (project.EditFileLockDate == null)
                        {
                            EditFileLockDateT = "5555/10/10";
                        }
                    }
                    ////////////////////
                    if (Session["RemoveFileLockDate"].ToString() == "1300/10/10")
                    {
                        RemoveFileLockDateT = "5555/10/10";
                    }
                    else if (Session["RemoveFileLockDate"].ToString() == "1100/11/22" ||
                             Session["RemoveFileLockDate"].ToString() == ""
                             || Session["RemoveFileLockDate"] == null)
                    {
                        RemoveFileLockDateT = project.RemoveFileLockDate;
                        if (project.RemoveFileLockDate == null)
                        {
                            RemoveFileLockDateT = "5555/10/10";
                        }
                    }
                    ///////////////////
                    if (Session["EditCommentLockDate"].ToString() == "1300/10/10")
                    {
                        EditCommentLockDateT = "5555/10/10";
                    }
                    else if (Session["EditCommentLockDate"].ToString() == "1100/11/22" ||
                             Session["EditCommentLockDate"].ToString() == ""
                             || Session["EditCommentLockDate"] == null)
                    {
                        EditCommentLockDateT = project.EditCommentLockDate;
                        if (project.EditCommentLockDate == null)
                        {
                            EditCommentLockDateT = "5555/10/10";
                        }
                    }
                    /////////////////
                    if (Session["RemoveCommentLockDate"].ToString() == "1300/10/10")
                    {
                        RemoveCommentLockDateT = "5555/10/10";
                    }
                    else if (Session["RemoveCommentLockDate"].ToString() == "1100/11/22" ||
                             Session["RemoveCommentLockDate"].ToString() == ""
                             || Session["RemoveCommentLockDate"] == null)
                    {
                        RemoveCommentLockDateT = project.RemoveCommentLockDate;
                        if (project.RemoveCommentLockDate == null)
                        {
                            RemoveCommentLockDateT = "5555/10/10";
                        }
                    }
                   
                    folder.EditLockDate = EditLockDateT ;
                    folder.RemoveLockDate = RemoveLockDateT ;
                    folder.EditFileLockDate = EditFileLockDateT ;
                    folder.RemoveFileLockDate = RemoveFileLockDateT ;
                    folder.EditCommentLockDate = EditCommentLockDateT ;
                    folder.RemoveCommentLockDate = RemoveCommentLockDateT ;
                    var gu = new List<TypesOfFolderVM>();
                    if (project.TypeofFolders != null)
                        foreach (var g in project.TypeofFolders)
                        {
                            gu.Add(new TypesOfFolderVM()
                                   {
                                       TypeofFolderId = g.Id,
                                       FolderId = project.Id
                                   });
                        }
                    folder.LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    folder.Description = project.Description;
                    folder.KeyWord = project.KeyWorddd;
                    folder.Name = project.Name;
                    folder.TypesOfFolders = gu;
                    folder.Id = project.Id;
                     folder.Comments = null;
                    folder.ClientCenterId = project.ClientCenterId;
                    var responseTypeofFolder = _folderService.Update(
                        new FolderUpdateRequest
                        {
                            entity = folder,
                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
                });
                    if (responseTypeofFolder.IsSuccess)
                        return Json(new[] { ToProject( responseTypeofFolder.entity) }.ToDataSourceResult(dsRequest, ModelState));
                    else
                    {
                        ModelState.AddModelError("Error", responseTypeofFolder.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                }
                return Json(ModelState.ToDataSourceResult());
            }
            catch
            {// return Json(ModelState.ToDataSourceResult());
                return RedirectToAction("Index");
            }

           
        }
        [ClaimsAccess(ClaimType = "Project", Value = "CanDeleteProject")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest dsRequest, Project project)
        {
            if (project != null && ModelState.IsValid)
            {
               
                
                var responseTypeofFolder = _folderService.DeleteById(
                    new FolderDeleteByIdRequest
                    {
                        Id = project.Id,
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
                    });
                if (responseTypeofFolder.IsSuccess)
                    return Json(new[] { project }.ToDataSourceResult(dsRequest, ModelState));
                else
                {
                    ModelState.AddModelError("Error", responseTypeofFolder.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
            }


            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult DropClientCenter([DataSourceRequest]DataSourceRequest request)
        {
            var responseClientCenter = new ClientCenterFindAllResponse();
            responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
               });
            return Json(responseClientCenter.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
            return Json(responseClientCenter.Entities.ToList(), JsonRequestBehavior.AllowGet);
            return View("List", responseClientCenter.Entities);

        }
        public ActionResult DropClientCenter1([DataSourceRequest]DataSourceRequest request)
        {
            var responseClientCenter = new ClientCenterFindAllResponse();
            responseClientCenter = _clientCenterService.FindAll(
               new ClientCenterFindAllRequest
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
               });
             return Json(responseClientCenter.Entities.Select(p => new { Id = p.Id, Name = p.Name }), JsonRequestBehavior.AllowGet);
            

        }
        public ActionResult DropProject([DataSourceRequest]DataSourceRequest request)
        {
            var responseFolder = new FolderFindAllResponse();
            responseFolder = _folderService.FindAll(
               new FolderFindAllRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
        });

            return Json(responseFolder.Entities, JsonRequestBehavior.AllowGet);
           

        }
        public ActionResult MultiTypeOfFolder([DataSourceRequest]DataSourceRequest request)
        {
            var responseTypeOfFolder = new TypeofFolderFindAllResponse();
            responseTypeOfFolder = _typeOfFolderService.FindAll(
               new TypeofFolderFindAllRequest { });
            if (responseTypeOfFolder.IsSuccess)
                return Json(responseTypeOfFolder.Entities, JsonRequestBehavior.AllowGet);
            return View("List", responseTypeOfFolder.Entities);

        }
        public ActionResult CommentsEditLockDate(string EditLockDate)
        {
            Session["EditLockDate"] = EditLockDate;
            return Json("1390");

        }
        public ActionResult CommentsRemoveLockDate(string RemoveLockDate)
        {
            Session["RemoveLockDate"] = RemoveLockDate;
            return Json("1390");

        }
        public ActionResult CommentsEditFileLockDate( string EditFileLockDate)
        {
            Session["EditFileLockDate"] = EditFileLockDate;
            return Json("1390");

        }
        public ActionResult CommentsRemoveFileLockDate( string RemoveFileLockDate)
        {
            Session["RemoveFileLockDate"] = RemoveFileLockDate;
            return Json("1390");

        }
        public ActionResult CommentsEditCommentLockDate(string EditCommentLockDate)
        {
            Session["EditCommentLockDate"] = EditCommentLockDate;
            return Json("1390");

        }
        public ActionResult CommentsRemoveCommentLockDate( string RemoveCommentLockDate)
        {
            Session["RemoveCommentLockDate"] = RemoveCommentLockDate;
            return Json("1390");

        }

        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');
            foreach (var id in ids)
            {
                var responseTypeofFolder = _folderService.DeleteById(
                    new FolderDeleteByIdRequest
                    {
                        Id =int.Parse( id),
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
                    });
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
        public JsonResult NewGridView(int Id, string Name, string Description)
        {
            var tmpList = (List<int>) TempData["isDeleteP"];
                  tmpList.Add(Id);
           
            TempData["isDeleteP"] = tmpList;
           
            var result = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}