using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.Reporting.RFileMdl.Imps;
using YA.Archive.Service.Reporting.RFileMdl.Messaging;
using YA.Archive.Service.Reporting.RFileMdl.VMs;
using Microsoft.AspNet.Identity;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ReportFileController : Controller
    {
        // GET: ReportFile
        private ReportingFileService _reportFileService;
        private FolderService _folderService;
        private ReportTitleService _reportTitleService;

        public ReportFileController()
        {
            _folderService = new FolderService();
            _reportFileService =new ReportingFileService();
            _reportTitleService = new ReportTitleService();
        }
        public ActionResult Index(int? projectId)
        {
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
                                   Subject = "Attachment Management ",
                                    Url="/FileProject/Index?isBack=1"
                                   
                               },
                                new SiteRouting()

                               {
                                   Subject = "Attachment",
                                   Url="/File/Index?projectId="+projectId,
                              
                               },
                               new SiteRouting()

                               {
                                   Subject = "( Document"+n+")Report Attachment ",
                                   Url="/ReportFile/Index?projectId="+projectId,
                                
                               },

                           };
            ViewData["ReportFileSiteMap"] = mapSites;
            var rep = (ReportCFFileRRequest)TempData["reportFileInitialize"];
            TempData["reportFileInitialize"] = rep;
            var responseFolder = _reportFileService.ReportFile(rep);

            if (!responseFolder.IsSuccess)
            {
            }

            if (responseFolder.Entities == null)
                TempData["reportFi"] = new List<ReportClientFolderFileRVM>();
            else
                TempData["reportFi"] = responseFolder.Entities.ToList();
            return View("ReportFile");

            //TempData["report"] = "";

            //return View("ReportMealReport");

        }

        [HttpPost]
        public ActionResult Index(int? sClientCenterId, int? sFolderId, int? sOFolderId,
            List<TypeofFileVM> sTypeofFiles,
            string sCode, string sFileName, string sKeyWords
            , string sStartDate, string sEndDate, FormCollection form)
        {
            if (sCode == "" && sClientCenterId == null && sFolderId == null && sOFolderId == null
                && form["STypeofFiles[0]"] == null
                && sFileName == "" && sKeyWords == "" && sStartDate == "" && sEndDate == "")
            {
                return RedirectToAction("Index", "File");
            }
            if (sOFolderId != null)
                sFolderId = sOFolderId;
            int ii = 0;
            var gu = new List<TypesOfFileVM>();
            // var ttttt = GetProjects();
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

            var responseFile = _reportFileService.ReportFile(
                new ReportCFFileRRequest()
                {
                    entity = new FileExpVM()
                             {
                                 Code = sCode,
                                 FolderId = sFolderId ?? 0,
                                 FileName = sFileName,
                                 KeyWord = sKeyWords,
                                 TypesOfFiles = gu
                             },
                    FromDate = sStartDate,
                    ToDate = sEndDate,
                });
            if (!responseFile.IsSuccess)
            {

            }
            else
            {
                if (responseFile.Entities == null)
                    TempData["reportFi"] = "";
                else
                    TempData["reportFi"] = responseFile.Entities.ToList();

            }
            return View("ReportFile");
        }


        public ActionResult ReportFile()
        {
            string title1 = "";
            string title2 = "";
            string title3 = "";
            string imageUrl = "";
            var responseTitle = _reportTitleService.FindAll(new ReportTitleFindAllRequest());
            if (responseTitle.IsSuccess && responseTitle.Entities != null && responseTitle.Entities.Count() != 0)
            {
                var firstOrDefault = responseTitle.Entities.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    title1 = firstOrDefault.Title1 ?? "";
                    title2 = firstOrDefault.Title2 ?? "";
                    title3 = firstOrDefault.Title3 ?? "";
                    imageUrl = firstOrDefault.ImageUrl ?? Server.MapPath("~") + "Files\\ReportImage\\" + "ReportTitle";
                }
            }
            var img = new System.Drawing.Bitmap(@imageUrl);

            byte[] array1 = ImageHelper.imageToByteArray(img);

            MemoryStream ms = new MemoryStream(array1);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

            try
            {
                List<ReportClientFolderFileRVM> reportModelTemp = (List<ReportClientFolderFileRVM>)TempData["reportFi"];
                StiReport report = new StiReport();
                string Path = Server.MapPath("~/Reports/ReportFile.mrt");
                report.Load(Path);
                report.RegBusinessObject("ReportClientFolderFileRVM", reportModelTemp);
                report.Dictionary.Variables.Add("Image1", image);
                report.Dictionary.Variables.Add("Title1", title1);
                report.Dictionary.Variables.Add("Title2", title2);
                report.Dictionary.Variables.Add("Title3", title3);
                report.Dictionary.SynchronizeBusinessObjects();
                return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Custom Error Message: " + e.Message);
            }
        }

       
        public ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult(this.HttpContext);
        }
        //Create Log out
        public ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult(this.HttpContext);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult(this.HttpContext);
        }
    }
}