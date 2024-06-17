using YA.Archive.MVC2.Models;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.Imps;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.Messaging;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.VMs;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ReportUserLogTimeController : Controller
    {
        // GET: ReportUserLogTime
        private ReportingUserLogTimeService _reportingUserLogTimeService;
        private ReportTitleService _reportTitleService;

        public ReportUserLogTimeController()
        {
            _reportingUserLogTimeService = new ReportingUserLogTimeService();
            _reportTitleService = new ReportTitleService();
        }

        // GET: ReportMeal



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
                                   Subject = "Users Log",
                                   Url = "/UserLogTime/Index"
                               },

                               new SiteRouting()

                               {
                                   Subject = "Report Users ",
                                   Url="/ReportUserLogTime/Index",
                               },

                           };
            ViewData["ReportUserLogTimeSiteMap"] = mapSites;
            var rep = (ReportClientUserLogTimeRRequest)TempData["reportUserLogTimeInitialize"];
            TempData["reportUserLogTimeInitialize"] = rep;
            var responseUserLogTime = _reportingUserLogTimeService.ReportUserLogTime(rep);

            if (!responseUserLogTime.IsSuccess)
            {
            }

            if (responseUserLogTime.Entities == null)
                TempData["reportUserLogTime"] = new List<ReportClientUserLogTimeRVM>();
            else
                TempData["reportUserLogTime"] = responseUserLogTime.Entities.ToList();
            return View("ReportUserLogTime");


        }

     

        public ActionResult ReportUserLogTime()
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


            var reportModelTemp = TempData["reportUserLogTime"];
            StiReport report = new StiReport();
            string Path = Server.MapPath("~/Reports/ReportUserLogTime.mrt"); 
            report.Load(Path);
            report.RegBusinessObject("ReportClientUserLogTimeRVM", reportModelTemp); 
                                                                           
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