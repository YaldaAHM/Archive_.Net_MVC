using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.MVC2.Models.Folder;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RCommentMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        // GET: Comment
        private FolderService _folderService;

        public CommentController()
        {
            _folderService = new FolderService();
        }

       

        public ActionResult Index(int? projectId)
        {
            if(projectId==null)
                return RedirectToAction("Index", "CommentProject");
            var response = _folderService.FindById(
                new FolderFindByIdRequest() { Id = projectId.Value });
            var n = response.entity.Name;
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
                                   Url="/CommentProject/Index?isBack=1"
                                  
                               },
                                new SiteRouting()

                               {
                                     Subject = " ( Document "+n+" ) Comment",
                                     Url="/Comment/Index"
                                 
                               },
                           };
            ViewData["CommentSiteMap"] = mapSites;
            ViewBag.projectId = projectId;
            return View();
        }
        [ClaimsAccess(ClaimType = "Comment", Value = "CanViewComment")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int projectId
            , string sCode, string sDescription
            , string sStartDate, string sEndDate)
        {
            
            if (sCode == "*")
            {
                TempData["reportCommentInitialize"] = new ReportCFCommentRRequest()
                {
                    RequsetCurrentUserId =
                  System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new CommentExpVM()
                    {
                        FolderId = projectId,
                        Code = "",
                        Description = sDescription,
                        Folder = new FolderExpVM()
                        {

                        }
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate

                };
                var responseGroup1 = _folderService.FindByExpression(
                new FolderFindByExpressionExpRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x => //x.Id == sFolderId &&
                                      x.Comments.Any()

                                      && x.Comments.Count() != 0 &&
                                     (projectId == 0 || x.Comments.Select(m => m.FolderId).Contains(projectId)) 
                                       

                });
                
                if (responseGroup1.IsSuccess)
                {
                    
                    var cList = new List<CommentViewModel>();
                    foreach (var r in responseGroup1.Entities)
                    {
                        foreach (var c in r.Comments)
                        {
                            
                                cList.Add(new CommentViewModel()
                                {
                                    Code = c.Code,
                                    CreateUserName = c.CreateUser.UserName,
                                    Description = c.Description,
                                    Id = c.Id,
                                    LastUpdateUserName = c.LastUpdateUser.UserName,
                                    CreateDate = c.CreateDate,
                                    LastUpdateDate = c.LastUpdateDate,
                                    FolderId = c.FolderId,
                                    //Folder=c.Folder,
                                    HasClaimEdit = c.Folder.HasClaimEdit,
                                    HasClaimDelete = c.Folder.HasClaimDelete,
                                    HasClaimInsert = c.Folder.HasClaimInsert,
                                    HasClaimView = c.Folder.HasClaimView,
                                    HasClaimUpdateLock = c.Folder.HasClaimUpdateLock,
                                    HasClaimDeleteLock = c.Folder.HasClaimDeleteLock,
                                    HasClaimUpdateFileLock = c.Folder.HasClaimUpdateFileLock,
                                    HasClaimDeleteFileLock = c.Folder.HasClaimDeleteFileLock,
                                    HasClaimUpdateCommentLock = c.Folder.HasClaimUpdateCommentLock,
                                    HasClaimDeleteCommentLock = c.Folder.HasClaimDeleteCommentLock,
                                    IsUpdateLock = c.Folder.IsUpdateLock,
                                    IsDeleteLock = c.Folder.IsDeleteLock,
                                    IsUpdateFileLock = c.Folder.IsUpdateFileLock,
                                    IsDeleteFileLock = c.Folder.IsDeleteFileLock,
                                    ClientCenterId = c.Folder.ClientCenterId,

                                    IsDeleteCommentLockCreateUser = r.IsDeleteCommentLockCreateUser,
                                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                                    IsUpdateCommentLockCreateUser = r.IsUpdateCommentLockCreateUser,
                                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
                                }
                                    );
                        }
                    }
                    return Json(cList.OrderByDescending(P => P.LastUpdateDate)
                        .ThenByDescending(p => p.Id).ToDataSourceResult(request));
                }
            }
           
            var startDate = fpub.convert2miladi(sStartDate);
            var endDate = fpub.convert2miladi(sEndDate);
            TempData["reportCommentInitialize"] = new ReportCFCommentRRequest()
            {
                RequsetCurrentUserId =
                System.Web.HttpContext.Current.User.Identity.GetUserId(),
                entity = new CommentExpVM()
                {
                    FolderId = projectId,
                    Code = sCode,
                    Description = sDescription,
                    Folder = new FolderExpVM()
                    {
                      
                    }
                },
                FromDate = sStartDate,
                ToDate = sEndDate

            };
            var responseGroup = _folderService.FindByExpression(
                new FolderFindByExpressionExpRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x => //x.Id == sFolderId &&
                                      x.Comments.Any()

                                      && x.Comments.Count() != 0 &&
                                     (projectId == 0 || x.Comments.Select(m => m.FolderId).Contains(projectId)) &&
                                       (sDescription == "" ||
                                        x.Comments.Any(m => m.Description.Contains(sDescription))) &&
                                      (sCode == "" || x.Comments.Any(m => m.Code.Contains(sCode))) &&
                                      (sStartDate == "" || x.Comments.Any(m => m.CreateDate >= startDate))
                                      &&
                                      (sEndDate == "" || x.Comments.Any(m => m.CreateDate <= endDate))

                });
            if (responseGroup.IsSuccess)
            {
               
                var cList = new List<CommentViewModel>();
                foreach (var r in responseGroup.Entities)
                {
                    foreach (var c in r.Comments)
                    {
                        if (//(sFolderId == null || c.FolderId == sFolderId) &&
                                       (sDescription == "" ||
                                         c.Description.Contains(sDescription)) &&
                                      (sCode == "" || c.Code.Contains(sCode)) &&
                                      (sStartDate == "" ||
                                      fpub.convert2miladi(c.CreateDate) >= startDate)
                                      &&
                                      (sEndDate == "" ||
                                      fpub.convert2miladi(c.CreateDate) <= endDate))

                            cList.Add(new CommentViewModel()
                            {
                                Code = c.Code,
                                CreateUserName = c.CreateUser.UserName,
                                Description = c.Description,
                                Id = c.Id,
                                LastUpdateUserName = c.LastUpdateUser.UserName,
                                CreateDate = c.CreateDate,
                                LastUpdateDate = c.LastUpdateDate,
                                FolderId = c.FolderId,
                                //Folder=c.Folder,
                                HasClaimEdit = c.Folder.HasClaimEdit,
                                HasClaimDelete = c.Folder.HasClaimDelete,
                                HasClaimInsert = c.Folder.HasClaimInsert,
                                HasClaimView = c.Folder.HasClaimView,
                                HasClaimUpdateLock = c.Folder.HasClaimUpdateLock,
                                HasClaimDeleteLock = c.Folder.HasClaimDeleteLock,
                                HasClaimUpdateFileLock = c.Folder.HasClaimUpdateFileLock,
                                HasClaimDeleteFileLock = c.Folder.HasClaimDeleteFileLock,
                                HasClaimUpdateCommentLock = c.Folder.HasClaimUpdateCommentLock,
                                HasClaimDeleteCommentLock = c.Folder.HasClaimDeleteCommentLock,
                                IsUpdateLock = c.Folder.IsUpdateLock,
                                IsDeleteLock = c.Folder.IsDeleteLock,
                                IsUpdateFileLock = c.Folder.IsUpdateFileLock,
                                IsDeleteFileLock = c.Folder.IsDeleteFileLock,
                                ClientCenterId = c.Folder.ClientCenterId,
                                IsDeleteCommentLockCreateUser = r.IsDeleteCommentLockCreateUser,
                                IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                                IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                                IsUpdateCommentLockCreateUser = r.IsUpdateCommentLockCreateUser,
                                IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                                IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
                            }
                                );
                    }
                }
                return Json(cList.OrderByDescending(P => P.Id)
                    .ThenByDescending(p => p.Id).ToDataSourceResult(request));
            }
            return View("Index", responseGroup.Entities);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "Comment", Value = "CanCreateComment")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, CommentViewModel comment, int projectId)
        {
         
            var folder = new FolderVM();
            folder.Id = projectId;
            folder.Name = "bgvfd";
            folder.Comments = new List<CommentVM>();
            folder.Comments.Add(new CommentVM()
            {
                CreateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                Description = comment.Description,
                FolderId = projectId,
                CreateDate = fpub.convert2shamsiS(DateTime.Now),
                LastUpdateDate = fpub.convert2shamsiS(DateTime.Now)
            });
            var responseFolder = _folderService.InsertComment(
                new FolderInsertRequest
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = folder
                });
            if (responseFolder.IsSuccess)
            {
                var c = responseFolder.entity.Comments.FirstOrDefault(m => m.Description == comment.Description);
                var rfc=new CommentViewModel()
                        {
                    Code = c.Code,
                    CreateUserName = c.CreateUser.UserName,
                    Description = c.Description,
                    Id = c.Id,
                    LastUpdateUserName = c.LastUpdateUser.UserName,
                    CreateDate = c.CreateDate,
                    LastUpdateDate = c.LastUpdateDate,
                    FolderId = c.FolderId,
                    //Folder=c.Folder,
                    HasClaimEdit = c.Folder.HasClaimEdit,
                    HasClaimDelete = c.Folder.HasClaimDelete,
                    HasClaimInsert = c.Folder.HasClaimInsert,
                    HasClaimView = c.Folder.HasClaimView,
                    HasClaimUpdateLock = c.Folder.HasClaimUpdateLock,
                    HasClaimDeleteLock = c.Folder.HasClaimDeleteLock,
                    HasClaimUpdateFileLock = c.Folder.HasClaimUpdateFileLock,
                    HasClaimDeleteFileLock = c.Folder.HasClaimDeleteFileLock,
                    HasClaimUpdateCommentLock = c.Folder.HasClaimUpdateCommentLock,
                    HasClaimDeleteCommentLock = c.Folder.HasClaimDeleteCommentLock,
                    IsUpdateLock = c.Folder.IsUpdateLock,
                    IsDeleteLock = c.Folder.IsDeleteLock,
                    IsUpdateFileLock = c.Folder.IsUpdateFileLock,
                    IsDeleteFileLock = c.Folder.IsDeleteFileLock,
                    IsDeleteCommentLockCreateUser = c.IsDeleteCommentLockCreateUser,
                    IsDeleteFileLockCreateUser = c.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = c.IsDeleteLockCreateUser,
                    IsUpdateCommentLockCreateUser = c.IsUpdateCommentLockCreateUser,
                    IsUpdateFileLockCreateUser = c.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = c.IsUpdateLockCreateUser,
                };
                return Json(new[] { rfc }.ToDataSourceResult(request));
            }
            else
            {
                ModelState.AddModelError("Error", responseFolder.Message);
                return Json(ModelState.ToDataSourceResult());
            }
          
        }




        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "Comment", Value = "CanEditComment")]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CommentViewModel comment, int projectId)
        {
            //if (comment != null && ModelState.IsValid)
            //{
            var folder = new FolderVM();
            folder.Id = projectId;
            folder.Comments = new List<CommentVM>();
            folder.Comments.Add(new CommentVM()
            {
                LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                Description = comment.Description,
                FolderId = projectId,
                LastUpdateDate = fpub.convert2shamsiS(DateTime.Now),
                Id = comment.Id
            });

            var responseFolder = _folderService.UpdateComment(
                new FolderUpdateRequest
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = folder
                });
            if (responseFolder.IsSuccess)
            {
                var c = responseFolder.entity.Comments.FirstOrDefault(m => m.Description == comment.Description);
                var rfc = new CommentViewModel()
                {
                    Code = c.Code,
                    CreateUserName = c.CreateUser.UserName,
                    Description = c.Description,
                    Id = c.Id,
                    LastUpdateUserName = c.LastUpdateUser.UserName,
                    CreateDate = c.CreateDate,
                    LastUpdateDate = c.LastUpdateDate,
                    FolderId = c.FolderId,
                    //Folder=c.Folder,
                    HasClaimEdit = c.Folder.HasClaimEdit,
                    HasClaimDelete = c.Folder.HasClaimDelete,
                    HasClaimInsert = c.Folder.HasClaimInsert,
                    HasClaimView = c.Folder.HasClaimView,
                    HasClaimUpdateLock = c.Folder.HasClaimUpdateLock,
                    HasClaimDeleteLock = c.Folder.HasClaimDeleteLock,
                    HasClaimUpdateFileLock = c.Folder.HasClaimUpdateFileLock,
                    HasClaimDeleteFileLock = c.Folder.HasClaimDeleteFileLock,
                    HasClaimUpdateCommentLock = c.Folder.HasClaimUpdateCommentLock,
                    HasClaimDeleteCommentLock = c.Folder.HasClaimDeleteCommentLock,
                    IsUpdateLock = c.Folder.IsUpdateLock,
                    IsDeleteLock = c.Folder.IsDeleteLock,
                    IsUpdateFileLock = c.Folder.IsUpdateFileLock,
                    IsDeleteFileLock = c.Folder.IsDeleteFileLock,
                    IsDeleteCommentLockCreateUser = c.IsDeleteCommentLockCreateUser,
                    IsDeleteFileLockCreateUser = c.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = c.IsDeleteLockCreateUser,
                    IsUpdateCommentLockCreateUser = c.IsUpdateCommentLockCreateUser,
                    IsUpdateFileLockCreateUser = c.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = c.IsUpdateLockCreateUser,
                };
                return Json(new[] { rfc }.ToDataSourceResult(request));
            }
            
            return Json(new[] { responseFolder.entity }.ToDataSourceResult(request));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "Comment", Value = "CanDeleteComment")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, CommentViewModel comment, int projectId)
        {
            if (comment != null)
            {
                var folder = new FolderVM();
                folder.Id = projectId;
                folder.Comments = new List<CommentVM>();
                folder.Comments.Add(new CommentVM()
                {
                    FolderId = projectId,
                    Id = comment.Id,
                });
                var responseGroup = _folderService.DeleteComment(
                                            new FolderDeleteRequest { entity = folder });
            }

            return Json(new[] { comment }.ToDataSourceResult(request));
        }
        public JsonResult MultiDelete(string Ids, int FolderId)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');
           
            foreach (var id in ids)
            {
                var folder = new FolderVM();
                folder.Id = FolderId;
                folder.Comments = new List<CommentVM>();
                folder.Comments.Add(new CommentVM()
                {
                    FolderId = FolderId,
                    Id =int.Parse(id),
                });
                var responseTypeofFile = _folderService.DeleteComment(
                    new FolderDeleteRequest() { entity=folder});
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