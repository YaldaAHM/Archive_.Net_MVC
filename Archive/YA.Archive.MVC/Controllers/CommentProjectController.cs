using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.Folder;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.Imps;
using YA.Archive.Service.TypeofFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class CommentProjectController : Controller
    {
        // GET: CommentProject
        private FolderService _folderService;
        private ClientCenterService _clientCenterService;
        private TypeofFolderService _typeOfFolderService;

        public CommentProjectController()
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
                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
            });
            foreach (var r in responseFolder.Entities.OrderByDescending(n => n.LastUpdateDate))
            {
                projects.Add(new Project
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    Code = r.Code,
                    ClientCenterId = r.ClientCenterId,
                    EditLockDate = r.IsEditLockDate == true ? r.EditLockDate : null,
                    RemoveLockDate = r.IsRemoveLockDate == true ? r.RemoveLockDate : null,
                    TypeofFolders = TypeofFolderList(r.TypesOfFolders),
                    EditCommentLockDate = r.IsEditCommentLockDate == true ? r.EditCommentLockDate : null,
                    RemoveCommentLockDate = r.IsRemoveCommentLockDate == true ? r.RemoveCommentLockDate : null,
                    EditFileLockDate = r.IsEditFileLockDate == true ? r.EditFileLockDate : null,
                    RemoveFileLockDate = r.IsRemoveFileLockDate == true ? r.RemoveFileLockDate : null,
                    IsRemoveCommentLockDate = r.IsRemoveCommentLockDate,
                    IsRemoveFileLockDate = r.IsRemoveFileLockDate,
                    IsRemoveLockDate = r.IsRemoveLockDate,
                    IsEditCommentLockDate = r.IsEditCommentLockDate,
                    IsEditFileLockDate = r.IsEditFileLockDate,
                    IsEditLockDate = r.IsEditLockDate,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,

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
            projects = new List<Project>();
            foreach (var r in folders.OrderByDescending(n => n.LastUpdateDate))
            {
                projects.Add(new Project
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    Code = r.Code,
                    ClientCenterId = r.ClientCenterId,
                    EditLockDate = r.EditLockDate,
                    RemoveLockDate = r.RemoveLockDate,
                    TypeofFolders = TypeofFolderList(r.TypesOfFolders),
                    EditCommentLockDate = r.EditCommentLockDate,
                    RemoveCommentLockDate = r.RemoveCommentLockDate,
                    EditFileLockDate = r.EditFileLockDate,
                    RemoveFileLockDate = r.RemoveFileLockDate,
                    IsRemoveCommentLockDate = r.IsRemoveCommentLockDate,
                    IsRemoveFileLockDate = r.IsRemoveFileLockDate,
                    IsRemoveLockDate = r.IsRemoveLockDate,
                    IsEditCommentLockDate = r.IsEditCommentLockDate,
                    IsEditFileLockDate = r.IsEditFileLockDate,
                    IsEditLockDate = r.IsEditLockDate,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,

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
                                   Subject = "Comment Management",
                                   Url="/CommentProject/Index"
                                   
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
            ViewData["ClientCenter"] = responseClientCenter.Entities.ToList();// studentList;
            TempData["isbackC"] = isBack;
            return View();
        }
        [ClaimsAccess(ClaimType = "Project", Value = "CanViewProject")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest dsRequest,
            int? sClientCenterId, List<TypeofFolderVM> sTypeofFolders,
           string sCode, string sName, string sKeyWords
            , string sStartDate, string sEndDate, FormCollection form, bool? isBack, bool sIsClickSearch)
        {
            int currentPage = dsRequest.Page;
            var projects = new List<Project>();
           
            if (sCode == "" && sClientCenterId == null
               && form["STypeofFolders[0]"] == null
             && sName == "" && sKeyWords == "" && sStartDate == "" && sEndDate == "" && sIsClickSearch == false)
            {
                
                    var oldS = (ReportClientFolderRRequest)TempData["oldSearchBackPC"];

                    if (oldS != null)
                    {
                        TempData["oldSearchBackPC"] = oldS;
                        if (oldS.entity.Code == "*" || oldS.entity.Name == "*")
                        {

                            var responseFolder1 = _folderService.FindAllPaging(
                                new FolderFindAllRequest()
                                {
                                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                                    Index = currentPage,
                                    Count = 10,
                                });
                            if (!responseFolder1.IsSuccess)
                            {
                                ModelState.AddModelError("Error", responseFolder1.Message);
                                return Json(ModelState.ToDataSourceResult());
                            }
                            projects = ToProject(responseFolder1.Entities.ToList());
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
                                        expression = x => //x.TypesOfFolders.Any()

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
                    if (total1 < (currentPage * 10))
                    {
                        dsRequest.Page = 1;
                    }
                    return Json(resultPaging);
                }
            
            return Json(ModelState.ToDataSourceResult());
        }
    
                       
            if (sCode == "*" || sName == "*" || sIsClickSearch)
            {
                TempData["oldSearchBackPC"] = new ReportClientFolderRRequest()
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
            // var ttttt = GetProjects();
            if (sTypeofFolders != null && form["STypeofFolders[0]"] != null)
            {
                foreach (var i in sTypeofFolders)
                {
                    gu.Add(new TypesOfFolderVM()
                           {
                               TypeofFolderId = int.Parse(form["STypeofFolders[" + ii + "]"]),
                               FolderId = 1
                           });
                    ii++;
                }
            }
            TempData["oldSearchBackPC"] = new ReportClientFolderRRequest()
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
              
                var typeofFolderId = gu.Select(c => c.TypeofFolderId).ToArray();
            var tp = (typeofFolderId.Any()) ? typeofFolderId.ToList().Count() : 0;
            
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
               //!idF.Contains(x.Id)
                (tp == 0 ||
                                              (x.TypesOfFolders.Any()

                                              && x.TypesOfFolders.Count() != 0 &&
                                              x.TypesOfFolders.Any(s => typeofFolderId.Contains(s.TypeofFolderId))))
                           ,                   //!idF.Contains(x.Id),
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
        public ActionResult DropProject([DataSourceRequest]DataSourceRequest request)
        {
            var responseFolder = new FolderFindAllResponse();
            responseFolder = _folderService.FindAll(
               new FolderFindAllRequest()
               {
                   RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
               });

            return Json(responseFolder.Entities, JsonRequestBehavior.AllowGet);
            return View("List", responseFolder.Entities);

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
       
    }
}