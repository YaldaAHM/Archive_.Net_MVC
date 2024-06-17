using YA.Archive.Service.DirectoryMdl.Imps;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Service.DirectoryMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Archive.MVC2.Models;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class DirectoryController : Controller
    {
        // GET: Directory

        private DirectoryService _DirectoryService;

        public DirectoryController()
        {
            _DirectoryService = new DirectoryService();
        }

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
                                   Subject = "Define the folder path",
                                   Url="/Directory/Index"
                               },

                           };
            ViewData["DirectorySiteMap"] = mapSites;
            return View();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var responseDirectory = _DirectoryService.FindAll(new DirectoryVM()
                );

            return Json(responseDirectory.ToDataSourceResult(request));
            return View("Index", responseDirectory);
        }
    

    [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest]DataSourceRequest request, DirectoryVM directory)
        {
            if (directory != null && ModelState.IsValid)
            {
                var responseDirectory = _DirectoryService.Insert( directory);
                
                    return Json(ModelState.ToDataSourceResult());
              
               
            }
            else
            {
              
                return Json(ModelState.ToDataSourceResult());
            }

        }


        [AcceptVerbs(HttpVerbs.Post)]
        //[ClaimsAccess(ClaimType = "Directory", Value = "CanEditDirectory")]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, DirectoryVM Directory)
        {
            if (Directory != null && ModelState.IsValid)
            {
                var responseDirectory = _DirectoryService.Update(
                            Directory );
            }
            return Json(ModelState.ToDataSourceResult());

          
        }
        [AcceptVerbs(HttpVerbs.Post)]
        //[ClaimsAccess(ClaimType = "Directory", Value = "CanDeleteDirectory")]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, DirectoryVM Directory)
        {
            if (Directory != null)
            {

                var responseDirectory = _DirectoryService.Delete(Directory );
            }

            return Json(new[] { Directory }.ToDataSourceResult(request, ModelState));
        }
    
}
}