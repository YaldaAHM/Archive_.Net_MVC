using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Infrastructure.Infrastructure.Folder;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.Imps;
using YA.Archive.Service.TypeofFileMdl.Imps;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.FolderMdl.Imps;
using Kendo.Mvc.UI;
using YA.Archive.Service.TypeofFileMdl.Messaging;
using Microsoft.AspNet.Identity;
using Kendo.Mvc.Extensions;
using YA.Archive.Service.Reporting.RFileMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.FileMdl.FileStorage;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Localization.MessageLocalize;
using File = YA.Archive.MVC2.Models.File.File;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        // GET: File
        private FolderService _folderService;
        private FileService _fileService;
        private ClientCenterService _clientCenterService;
        private TypeofFileService _typeOfFileService;
        public FileController()
        {
            _folderService = new FolderService();
            _fileService = new FileService();
            _clientCenterService = new ClientCenterService();
            _typeOfFileService = new TypeofFileService();
        }
        
        public ActionResult Index(int? projectId)
        {
            if (projectId == null)
                return RedirectToAction("Index", "FileProject");
            var response = _folderService.FindById(
                new FolderFindByIdRequest() {Id = projectId.Value});
           var n= response.entity.Name;
            var mapSites = new List<SiteRouting>
                           {
                               new SiteRouting()

                               {
                                   Subject = "Home",
                                   Url="/Home/Index"
                               },
                               new SiteRouting()

                               {
                                   Subject ="Attachment Management",
                                   Url="/FileProject/Index?isback=1"
                                  
                               },
                               new SiteRouting()

                               {
                                   Subject = " ( Document "+n+" ) Attachment",
                                   Url="/File/Index"
                                  
                               },
                           };
            ViewData["FileSiteMap"] = mapSites;
            ViewBag.projectId = projectId;

            List<TypeofFileVM> typeList = new List<TypeofFileVM>();
            var responseType = _typeOfFileService.FindAll(
               new TypeofFileFindAllRequest
               {
               });
            if (responseType.IsSuccess)
            {
                foreach (var e in responseType.Entities.ToList())
                {
                    typeList.Add(new TypeofFileVM() { Id = e.Id, Name = e.Name });

                }
                ViewData["TypeofFile"] = typeList;
            }
            return View();
        }
        [ClaimsAccess(ClaimType = "File", Value = "CanViewFile")]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int projectId,
           List<TypeofFileVM> sTypeofFiles,
            string sCode, string sFileName, string sKeyWords
            , string sStartDate, string sEndDate, FormCollection form)
        {
            int currentPage = request.Page;
            var files = new List<File>();
            if (sCode == "*" || sFileName == "*")
            {
               
                TempData["reportFileInitialize"] = new ReportCFFileRRequest()
                {
                    entity = new FileExpVM()
                    {
                        FileName = "",
                        FolderId = projectId,
                        Code = "",
                        KeyWord = sKeyWords,

                        TypesOfFiles = new List<TypesOfFileVM>()
                       
                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate

                };
                var responseFile1 = _fileService.FindByExpressionPaging(
                     new FileFindByExpressionExpRequest()
                     {
                         RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                         expression = x =>
                                           (projectId == 0 || x.FolderId == projectId)
                                           ,
                         Index = currentPage,
                         Count = 10,
                     });
                if (!responseFile1.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseFile1.Message);
                    return Json(ModelState.ToDataSourceResult());
                }
                files = ToFiles(responseFile1.Entities.ToList());

                var resultPaging1 = new DataSourceResult()
                {
                    Data = files,
                    Total = responseFile1.TotalRecords
                };

                return Json(resultPaging1);
            }
        
            int ii = 0;
            var startDate = fpub.convert2miladi(sStartDate);
            var endDate = fpub.convert2miladi(sEndDate);

            var gu = new List<TypesOfFileVM>();
            

            if (sTypeofFiles != null && form["STypeofFiles[0]"] != null)
            {
                foreach (var i in sTypeofFiles)
                {
                    gu.Add(new TypesOfFileVM()
                           {
                               TypeofFileId = int.Parse(form["STypeofFiles[" + ii + "]"]),
                               FileId = 1
                           });
                    ii++;
                }
            }
            TempData["reportFileInitialize"] = new ReportCFFileRRequest()
                {
                    entity = new FileExpVM()
                    {
                        FileName = sFileName,
                        FolderId = projectId,
                        Code = sCode,
                        KeyWord = sKeyWords,
                        TypesOfFiles = gu,
                       

                    },
                    FromDate = sStartDate,
                    ToDate = sEndDate

                };
                var typeofFileId = gu.Select(c => c.TypeofFileId).ToArray();
            var tp = (typeofFileId.Any()) ? typeofFileId.ToList().Count() : 0;
          
                    var responseFile = _fileService.FindByExpressionPaging(
                        new FileFindByExpressionExpRequest()
                        {
                            RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            expression = x => 
                                              (sCode == "" || x.Code.Contains(sCode)) &&
                                              (sFileName == "" || x.FileName.Contains(sFileName)) &&
                                               (projectId == 0 || x.FolderId == projectId)&&
                                              
                                              (sKeyWords == "" || x.KeyWord.Contains(sKeyWords)) &&
                                              (sStartDate == "" || x.CreateDate >= startDate) &&
                                              (sEndDate == "" || x.CreateDate <= endDate) &&
                                            
                                               (tp == 0 ||
                                              (x.TypesOfFiles.Any()

                                              && x.TypesOfFiles.Count() != 0 &&
                                              x.TypesOfFiles.Any(s => typeofFileId.Contains(s.TypeofFileId))))
,
                            Index = currentPage,
                            Count = 10,
                  
                        });
                    if (!responseFile.IsSuccess)
                    {
                        ModelState.AddModelError("Error", responseFile.Message);
                        return Json(ModelState.ToDataSourceResult());
                    }
                  
                    files.AddRange(ToFiles(responseFile.Entities.ToList()));
   
            var resultPaging = new DataSourceResult()
            {
                Data = files,
                Total = responseFile.TotalRecords
            };

            return Json(resultPaging);

        }
      

        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "File", Value = "CanCreateFile")]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, File file, int projectId)
        {
            
                return Json(ModelState.ToDataSourceResult());
           

        }




        [AcceptVerbs(HttpVerbs.Post)]
         [ClaimsAccess(ClaimType = "File", Value = "CanEditFile")]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, File file, int projectId)
        {
            if (file != null && ModelState.IsValid)
            {
                var gu = new List<TypesOfFileVM>();
                if (file.TypeofFiles != null)
                    foreach (var g in file.TypeofFiles)
                    {
                        gu.Add(new TypesOfFileVM()
                        {
                            TypeofFileId = g.Id,
                            FileId = file.Id
                        });
                    }
                var responseFile = new FileUpdateResponse();

                responseFile = _fileService.Update(
                    new FileUpdateRequest
                    {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = new FileVM()
                        {
                            Id = file.Id,
                            Description = file.Description,
                            FolderId = projectId,
                            //KeyWord = file.Path,
                            KeyWord=file.KeyWorddd,
                            LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                            TypesOfFiles = gu
                        }
                    });
                if (!responseFile.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseFile.Message);

                }
                return Json(ToFiles( responseFile.entity));
            }

            return Json(ModelState.ToDataSourceResult());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ClaimsAccess(ClaimType = "File", Value = "CanDeleteFile")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, File file)
        {
                
                var responseFile = new FileDeleteResponse();

                var response =
                    _fileService.DeleteById(new FileDeleteByIdRequest()
                    {
                        Id = file.Id,
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    }

                        , new HttpFileCollectionBaseAdapter());
                if (response.IsSuccess)
                {
                }
                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("Error", responseFile.Message);

                }
                //}

                return Json(ModelState.ToDataSourceResult());

            }

        public ActionResult MultiTypeOfFile([DataSourceRequest] DataSourceRequest request)
        {
            var responseTypeOfFile = new TypeofFileFindAllResponse();
            responseTypeOfFile = _typeOfFileService.FindAll(
                new TypeofFileFindAllRequest { });
            if (responseTypeOfFile.IsSuccess)
                return Json(responseTypeOfFile.Entities, JsonRequestBehavior.AllowGet);
            return View("List", responseTypeOfFile.Entities);

        }

        [HttpGet]
        public void Delete(int fileId)
        {
           
            var response =
                _fileService.DeleteById(new FileDeleteByIdRequest()
                {
                    Id = fileId,
                    RequsetCurrentUserId =
                                                System.Web.HttpContext.Current.User.Identity.GetUserId(),
                }

                    , new HttpFileCollectionBaseAdapter());
            if (response.IsSuccess)
            {
                ModelState.AddModelError("Error", response.Message);
            }
        }

        [HttpPost]
        [ClaimsAccess(ClaimType = "File", Value = "CanCreateFile")]
        public ActionResult UploadFiles(File filee, FormCollection form, int projectId)
        {
            if (Session["PId"] != null && Session["PId"].ToString() != "")
            {
                filee.FolderId = int.Parse(Session["PId"].ToString());
            }

            var gu = new List<TypesOfFileVM>();
            if (filee.TypeofFiles != null)
            {
                var ppp = form.GetValues("TypeofFiles");
                foreach (var g in ppp) // filee.TypeofFiles)
                {
                    gu.Add(new TypesOfFileVM()
                           {
                               TypeofFileId = int.Parse(g), // g.Id,
                               FileId = filee.Id
                           });
                }
            }

           
            var r = new List<ViewDataUploadFilesResult10>();
   
            
            filee.CreateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            filee.LastUpdateUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            
            for (int i = 0; i < Request.Files.Count; i++)

            {
                var file = Request.Files[i];

                var statuses = new List<ViewDataUploadFilesResult10>();
                var headers = Request.Headers;

                if (file.ContentLength > 52425500)
                {
                    statuses.Add(new ViewDataUploadFilesResult10()
                    {
                        name = file.FileName,
                        size = file.ContentLength,
                        type = file.ContentType,
                        url = "/File/Download/" + file.FileName,
                        delete_url = "/File/Delete/" + file.FileName,
                        thumbnail_url = "",
                        delete_type = "GET",
                        origname = file.FileName,
                        filename = file.FileName,
                        message = "The file size should not exceed 50MB."
                    });
                    JsonResult result = Json(statuses);

                    return result;

                }
                else {
                var response = _fileService.Insert<HttpRequestBase, string, HttpPostedFileBase>(
                    new FileInsertRequest<HttpRequestBase, string>
                    // new FileInsertRequest<string, string>
                    {
                        Request = Request,
                        IsPartialFile = !string.IsNullOrEmpty(headers["X-File-Name"]),
                        RootPath = "",
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        entity = new FileVM()
                        {
                            Id = filee.Id,
                            Description = filee.Description,
                            FolderId = projectId,
                            KeyWord = filee.KeyWorddd,
                            Code = filee.Code,
                            CreateUserId = filee.CreateUserId,
                            LastUpdateUserId = filee.LastUpdateUserId,
                            TypesOfFiles = gu,
                            FileName = file.FileName,
                        }
                    }, new HttpFileCollectionBaseAdapter());
                if (response.IsSuccess)
                {
                   
                    var message = "Confirmed";
                    statuses.Add(new ViewDataUploadFilesResult10()
                    {
                        name = response.FileInfo.OrigName, //file.FileName,
                        size = response.FileInfo.FileSize, // file.ContentLength,
                        type = response.FileInfo.FileType, //file.ContentType,
                        url = "/File/Download/" + response.FileInfo.FileName, //file.FileName,
                        delete_url = "/File/Delete/" + response.FileInfo.FileName, //file.FileName,
                        thumbnail_url = response.FileInfo.EncodeFile, // EncodeFile(fullPath),
                        delete_type = "GET",
                        origname = response.FileInfo.OrigName,
                        filename = response.FileInfo.FileName,
                        fileid = response.entity.Id,
                        message=message
                    });
                    JsonResult result = Json(statuses);
                    //result.ContentType = "text/plain";

                    return result;
                }
                else
                {
                    var message = "Not Confirmed";
                    
                    statuses.Add(new ViewDataUploadFilesResult10()
                    {
                        name = file.FileName,
                        size =  file.ContentLength,
                        type = file.ContentType,
                        url = "/File/Download/" + file.FileName,
                        delete_url = "/File/Delete/" + file.FileName,
                        thumbnail_url = "",//response.FileInfo.EncodeFile, // EncodeFile(fullPath),
                        delete_type = "GET",
                        origname = file.FileName,
                        filename = file.FileName,
                       
                        message = response.Message
                    });
                    JsonResult result = Json(statuses);
            

                    return result;
                }
            } }
            return Json(r);
        }
        public ActionResult Download(int? fileId)
        {
            var responseFile = new FileFindByIdResponse();

            responseFile = _fileService.FindById(
                new FileFindByIdRequest() { Id = fileId.Value });
             string rootPath = responseFile.entity.Folder.Path + FolderName.MainFolder +
                              responseFile.entity.Folder.Code.Substring(0, 2) + @"\" + responseFile.entity.Folder.Code;
            string pp = Path.Combine(rootPath, responseFile.entity.Code.Substring(2) +"-"+ responseFile.entity.OrginalName);
            if (System.IO.File.Exists(Path.Combine(pp)))
            {
                return File(pp, "application/force-download", responseFile.entity.OrginalName);
            }
            ModelState.AddModelError("Error", "file does not exist");
            return Json(ModelState.ToDataSourceResult(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult MultiDelete(string Ids)
        {
            var m = new ValidationMessage();
            m.IsSuccess = true;
            m.Message = MessageResource.DeleteSuccess;
            var ids = Ids.Split(',');

            foreach (var id in ids)
            {
                var responseTypeofFile = _fileService.DeleteById(
                    new FileDeleteByIdRequest()
                    {
                        Id = int.Parse(id) ,
                        RequsetCurrentUserId= System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    }, new HttpFileCollectionBaseAdapter());
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

        public List<File> files = new List<File>();

        public List<File> GetFiles()
        {
            var responseFile = _fileService.FindAll(
                new FileFindAllRequest()
                {
                    //  ClientCenterType = ClientCenterType.Main,
                    // expression = x => x.FolderId == projectId
                });
            foreach (var r in responseFile.Entities)
            {
                files.Add(new File
                {
                    Id = r.Id,
                    
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    KeyWordd = r.KeyWord,
                    KeyWorddd = r.KeyWord,
                    Code = r.Code,
               
                    TypeofFiles = TypeofFileList(r.TypesOfFiles),
                    OrginalName = r.OrginalName,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,
                    CreateUser = r.CreateUser,
                    LastUpdateUser = r.LastUpdateUser,
                   FileSize = ConvertToMB(r.FileSize),
                
                    FolderId = r.FolderId,
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



                
                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
                });
            }
            return files;
        }

        public List<File> ToFiles(List<FileVM> file)
        {
            files = new List<File>();
            foreach (var r in file.OrderByDescending(n => n.LastUpdateDate))
            {
                files.Add(new File
                {
                    Id = r.Id,
                   
                    Description = r.Description,
                    KeyWord = r.KeyWord,
                    KeyWordd = r.KeyWord,
                    KeyWorddd = r.KeyWord,
                    Code = r.Code,
              
                    TypeofFiles = TypeofFileList(r.TypesOfFiles),
                    OrginalName = r.OrginalName,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,
                 
                 
                    FolderId = r.FolderId,
                    CreateUserName = r.CreateUser.UserName,
                    LastUpdateUserName = r.LastUpdateUser.UserName,
                    ClientCenterId = r.Folder.ClientCenterId,
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


                   
                    IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                    IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                    IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                    IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,

                });

            }
            return files;
        }

        public File ToFiles(FileVM r)
        {
            return new File
            {
                Id = r.Id,
                
                Description = r.Description,
                KeyWord = r.KeyWord,
                KeyWordd = r.KeyWord,
                KeyWorddd = r.KeyWord,
                Code = r.Code,
                TypeofFiles = TypeofFileList(r.TypesOfFiles),
                OrginalName = r.OrginalName,
                CreateDate = r.CreateDate,
                LastUpdateDate = r.LastUpdateDate,
                FileSize = ConvertToMB(r.FileSize),
         
                FolderId = r.FolderId,
                CreateUserName = r.CreateUser.UserName,
                LastUpdateUserName = r.LastUpdateUser.UserName,
                ClientCenterId = r.Folder.ClientCenterId,
                IsUpdateLock = r.IsUpdateLock,
                IsDeleteLock = r.IsDeleteLock,
                IsUpdateFileLock = r.IsUpdateFileLock,
                IsDeleteFileLock = r.IsDeleteFileLock,

                IsDeleteFileLockCreateUser = r.IsDeleteFileLockCreateUser,
                IsDeleteLockCreateUser = r.IsDeleteLockCreateUser,
                IsUpdateFileLockCreateUser = r.IsUpdateFileLockCreateUser,
                IsUpdateLockCreateUser = r.IsUpdateLockCreateUser,
            };
        }

        private List<TypeofFileVM> TypeofFileList(List<TypesOfFileVM> gu)
        {
            List<TypeofFileVM> typesOfFolders = new List<TypeofFileVM>();
            foreach (var g in gu)
            {
                typesOfFolders.Add(new TypeofFileVM() { Id = g.TypeofFileId, Name = g.TypeofFile.Name });
            }
            return typesOfFolders;
        }

        public string ConvertToMB(int f)
        {
           
              long OneKb = 1024;
         long OneMb = OneKb * 1024;
         long OneGb = OneMb * 1024;
         long OneTb = OneGb * 1024;
        var asTb = Math.Round((double)f / OneTb, 0);
            var asGb = Math.Round((double)f / OneGb, 0);
            var asMb = Math.Round((double)f / OneMb, 0);
            var asKb = Math.Round((double)f / OneKb, 0);
            string chosenValue = asTb > 1 ? string.Format("{0}Tb", asTb)
                : asGb > 1 ? string.Format("{0}Gb", asGb)
                : asMb > 1 ? string.Format("{0}Mb", asMb)
                : asKb > 1 ? string.Format("{0}Kb", asKb)
                : string.Format("{0}B", Math.Round((double)f, 0));
            return chosenValue;
        }

    }

    public class ViewDataUploadFilesResult10
    {
        public int fileid { get; set; }
        public string name { get; set; }
        public string origname { get; set; }
        public string filename { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
        public string message { get; set; }
    }
}