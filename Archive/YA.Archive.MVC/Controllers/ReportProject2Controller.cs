
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RFolderMdl.Imps;
using YA.Archive.Service.Reporting.RFolderMdl.Messaging;
using YA.Archive.Service.Reporting.RFolderMdl.VMs;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using System.IO;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;
using YA.Archive.Service.ReportTitleMdl.VMs;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ReportProject2Controller : Controller
    {
        // GET: ReportMeal
        private ReportingFolderService _reportingFolderService;
        private ReportTitleService _reportTitleService;

        public ReportProject2Controller()
        {
            _reportingFolderService = new ReportingFolderService();
            _reportTitleService=new ReportTitleService();
        }

        // GET: ReportMeal



        public ActionResult Index()
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
            var  rep = (ReportClientFolderRRequest)TempData["reportProjectInitialize"];
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

        [HttpPost]
        public ActionResult Index(int? sClientCenterId, List<TypeofFolderVM> sTypeofFolders,
            string sCode, string sName, string sKeyWords
            , string sStartDate, string sEndDate, FormCollection form)
        {
            if (sCode == "" && sClientCenterId == null
                && form["STypeofFolders[0]"] == null
                && sName == "" && sKeyWords == "" && sStartDate == "" && sEndDate == "")
            {
                return RedirectToAction("Index", "Project");
            }
            int ii = 0;
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
                    ii++;
                }
            }
            var responseFolder = _reportingFolderService.ReportFolder(
                new ReportClientFolderRRequest()
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
                });

            if (!responseFolder.IsSuccess)
            {
            }

            if (responseFolder.Entities == null)
                TempData["reportProj"] = new List<ReportClientFolderRVM>();
            else
                TempData["reportProj"] = responseFolder.Entities.ToList();
            return View("ReportProject");
        }

        public virtual ActionResult ReportProjectt()
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
            byte[] array1 =ImageHelper.imageToByteArray(img);

            MemoryStream ms = new MemoryStream(array1);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            var reportModelTemp = TempData["reportProj"];
            StiReport report = new StiReport();
            string Path = @"D:\\SVN\\Archive1\\YA.Archive.MVC2\\Reports\\ReportProject11.mrt"; 
            report.Load(Path);
            report.RegBusinessObject("ReportClientFolderRVM", reportModelTemp); 
            
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