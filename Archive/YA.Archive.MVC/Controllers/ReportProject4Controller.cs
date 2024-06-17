using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.Reporting.RFolderMdl.Imps;
using YA.Archive.Service.Reporting.RFolderMdl.Messaging;
using YA.Archive.Service.Reporting.RFolderMdl.VMs;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;

namespace YA.Archive.MVC2.Controllers
{
    public class ReportProject4Controller : Controller
    {
        // GET: ReportProject4
        private ReportingFolderService _reportingFolderService;
        private ReportTitleService _reportTitleService;

        public ReportProject4Controller()
        {
            _reportingFolderService = new ReportingFolderService();
            _reportTitleService = new ReportTitleService();
        }
        public ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult StiReport()
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


            var reportModelTemp = TempData["reportProj"];

            var mainReport = new Stimulsoft.Report.StiReport();
            mainReport.Load(Server.MapPath("~/Reports/ReportProject11.mrt"));
            mainReport.Compile();
            mainReport["Image1"] = image;
            mainReport["Title1"] = title1;
            mainReport["Title2"] = title2;
            mainReport["Title3"] = title3;
            mainReport.RegBusinessObject("ReportClientFolderRVM", reportModelTemp);
            return Stimulsoft.Report.Mvc.StiMvcViewer.GetReportSnapshotResult(mainReport);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ViewerEvent()
        {
            return Stimulsoft.Report.Mvc.StiMvcViewer.ViewerEventResult(HttpContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Print()
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
                                   Subject = " Document Management  ",
                                    Url="/Project/Index?isBack=1"
                                   
                               },

                               new SiteRouting()

                               {
                                   Subject = "Report Document ",
                                   Url="/ReportProject/Index",
                                   
                               },

                           };
            ViewData["ReportProjectSiteMap"] = mapSites;
            var rep = (ReportClientFolderRRequest)TempData["reportProjectInitialize"];
            TempData["reportProjectInitialize"] = rep;
            var responseFolder = _reportingFolderService.ReportFolder(rep);

            if (!responseFolder.IsSuccess)
            {
            }

            if (responseFolder.Entities == null)
                TempData["reportProj"] = new List<ReportClientFolderRVM>();
            else
                TempData["reportProj"] = responseFolder.Entities.ToList();
            return View("ReportProject");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult PrintReport()
        {
            return Stimulsoft.Report.Mvc.StiMvcViewer.PrintReportResult(HttpContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual FileResult ExportReport()
        {
            return Stimulsoft.Report.Mvc.StiMvcViewer.ExportReportResult(HttpContext);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Interaction()
        {
            return Stimulsoft.Report.Mvc.StiMvcViewer.InteractionResult(HttpContext);
        }
    }
}