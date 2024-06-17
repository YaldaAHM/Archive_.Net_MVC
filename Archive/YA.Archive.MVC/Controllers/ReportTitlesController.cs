using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.ReportTitleMdl.Imps;
using YA.Archive.Service.ReportTitleMdl.Messaging;
using YA.Archive.Service.ReportTitleMdl.VMs;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class ReportTitlesController : Controller
    {

        // GET: ReportTitles
        private ReportTitleService _reportTitleService;

        public ReportTitlesController()
        {
                _reportTitleService=new ReportTitleService();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase UploadImage, ReportTitleVM reportTitle)
        {
            string imagePath;
            if (UploadImage.ContentLength > 0)
            {
                string path = Server.MapPath("~") + "Files\\ReportImage\\" + UploadImage.FileName;
                UploadImage.InputStream.ResizeImageByWidth(500, path, Utilty.ImageComperssion.Normal);
                var response=  _reportTitleService.Insert(
                    new ReportTitleInsertRequest() {entity=reportTitle});
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Ok. done successfully.";
                }
                else
                {
                    ViewBag.Message = " not done.";
                }
            }

            return View();
        }


        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(HttpPostedFileBase UploadImage, ReportTitleVM reportTitle, FormCollection form)
        {
            for (int i = 0; i < Request.Files.Count; i++)

            {
                var file = Request.Files[i];

                var statuses = new List<ViewDataUploadFilesResult10>();
                var headers = Request.Headers;

            }

         


                string imagePath;
            if (UploadImage.ContentLength > 0)
            {
                var fileName = UploadImage.FileName;
                string path = Server.MapPath("~") + "Files\\ReportImage\\" + "ReportTitle";
                reportTitle.ImageUrl = path;
                System.IO.File.Delete(path);
                UploadImage.InputStream.ResizeImage(100,70, path, Utilty.ImageComperssion.Normal);
                var response = _reportTitleService.Insert(
                   new ReportTitleInsertRequest() { entity = reportTitle });
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Ok. done successfully.";
                }
                else
                {
                    ViewBag.Message = " not done.";
                }
            }
            return View();
        }

        public ActionResult Update()
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
                                   Subject = "Title Report ",
                                    Url="/ReportTitle/Update"
                               },
                               

                           };
            ViewData["ReportTitleSiteMap"] = mapSites;



            var response = _reportTitleService.FindAll(
                  new ReportTitleFindAllRequest() { });
            var R=new ReportTitleVM();
            R = response.Entities.FirstOrDefault();
            if (response.IsSuccess)
            {
                
            }
            else
            {
               
            }
            return View(R);
        }
        [HttpPost]
        public ActionResult Update(HttpPostedFileBase file_upload, ReportTitleVM reportTitle)
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
                                   Subject = "Title Report ",
                                    Url="/ReportTitle/Update"
                               },


                           };
            ViewData["ReportTitleSiteMap"] = mapSites;
            string imagePath;
            string path = Server.MapPath("~") + "Files\\ReportImage\\" + "ReportTitle";
            reportTitle.ImageUrl = path;
            if (file_upload != null && file_upload.ContentLength > 0)
            {
                var fileName = file_upload.FileName;
                try
                {
                    System.IO.File.Delete(path);
                }
                catch 
                {
                    ViewBag.Message = "not done. The reporting process is in progress";
                    return View();
                }
                file_upload.InputStream.ResizeImage(100, 70, path, Utilty.ImageComperssion.Normal);
            }
            var response = _reportTitleService.Update(
                   new ReportTitleUpdateRequest() { entity = reportTitle });
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Ok. done successfully.";
                }
                else
                {
                    ViewBag.Message = " not done.";
                }
            
            return View();
        }
    }
}