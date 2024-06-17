using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RCommentMdl.Imps;
using YA.Archive.Service.Reporting.RCommentMdl.Messaging;
using YA.Archive.Service.Reporting.RCommentMdl.VMs;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;
using Microsoft.AspNet.Identity;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ReportCommentController : Controller
    {
        // GET: ReportComment
        private ReportingCommentService _reportCommentService;
        private FolderService _folderService;
        private ReportTitleService _reportTitleService;
        public ReportCommentController()
        {
            _folderService = new FolderService();
            _reportCommentService =new ReportingCommentService();
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
                                   Subject = "Comment Management ",
                                    Url="/CommentProject/Index?isBack=1"
                               },
                               new SiteRouting()

                               {
                                   Subject = "Comment",
                                   Url="/Comment/Index?projectId="+projectId,
                                 
                               },
                               new SiteRouting()

                               {
                                   Subject = "( Document"+n+")Report Comment ",
                                   Url="/ReportComment/Index?projectId="+projectId,
                                 
                               },

                           };
            ViewData["ReportCommentSiteMap"] = mapSites;
            var rep = (ReportCFCommentRRequest)TempData["reportCommentInitialize"];
            TempData["reportCommentInitialize"] = rep;
            var responseFolder = _reportCommentService.ReportComment(rep);

            if (!responseFolder.IsSuccess)
            {
            }

            if (responseFolder.Entities == null)
                TempData["reportComm"] = new List<ReportClientFolderCommentRVM>();
            else
                TempData["reportComm"] = responseFolder.Entities.ToList();
            return View("ReportComment");
        }

        [HttpPost]
        public ActionResult Index(int projectId, string sDescription
            , string sStartDate, string sEndDate)
        {
               var response = _reportCommentService.ReportComment(
                new ReportCFCommentRRequest()
                {
                    entity = new CommentExpVM()
                             {
                                 Id = projectId,

                             },
                    FromDate = sStartDate,
                    ToDate = sEndDate
                });
            if (response.IsSuccess)
            {
                if (response.Entities == null)
                    TempData["reportComm"] = new List<ReportClientFolderCommentRVM>();
                else
                    TempData["reportComm"] = response.Entities.ToList();

            }
            return View("ReportComment");
        }


        public ActionResult ReportComment()
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
            System.Drawing.Image image = null;
            if (imageUrl != "")
            {
                var img = new System.Drawing.Bitmap(@imageUrl);
                byte[] array1 = ImageHelper.imageToByteArray(img);

                MemoryStream ms = new MemoryStream(array1);
                image = System.Drawing.Image.FromStream(ms);
            }
            



            List<ReportClientFolderCommentRVM> reportModelTemp = (List<ReportClientFolderCommentRVM>)TempData["reportComm"];
            StiReport report = new StiReport();
            string Path = Server.MapPath("~/Reports/ReportComment.mrt");  
            report.Load(Path);
            // report.RegBusinessObject("ReportClientCumulative", reportModel); 
            report.RegBusinessObject("ReportClientFolderCommentRVM", reportModelTemp); 
                                                                                       // report.RegBusinessObject("Students", lst.Select(m=>m.Students).ToList());
                                                                                       //  report.Dictionary.DataSources["DataSourceName"].Parameters["ParameterName"].ParameterValue = yourValue;
            report.Dictionary.Variables.Add("Image1", image);
            report.Dictionary.Variables.Add("Title1", title1);
            report.Dictionary.Variables.Add("Title2", title2);
            report.Dictionary.Variables.Add("Title3", title3);
            report.Dictionary.SynchronizeBusinessObjects();
            return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);

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