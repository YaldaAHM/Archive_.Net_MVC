using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;

namespace YA.Archive.MVC2.Controllers
{
    public class TestReport1Controller : Controller
    {
        // GET: TestReport1
        public ActionResult Index()
        {
            return View();
        }
        public virtual ActionResult StiReport()
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fa-IR");

            // List of persons: 
            // you can change the following list to list of persons that get from database.
            var persons = new List<Models.Person>()
            {
                new Person() {Id = 0, FirstName = "sadar", LastName = "marvati", Gender = true}
                , new Person() {Id = 1, FirstName = "omid", LastName = "nasri", Gender = true}
                 , new Person() {Id = 2, FirstName = "hana", LastName = "akbari", Gender = false}
                  , new Person() {Id = 3, FirstName = "ali", LastName = "jahani", Gender = true }
                   , new Person() {Id = 4, FirstName = "sara", LastName = "sabori", Gender = false }
            };
            var mainReport = new Stimulsoft.Report.StiReport();
            mainReport.Load(Server.MapPath("~/Reports/Report.mrt"));
            mainReport.Compile();
            mainReport["DateTimeNow"] = DateTime.UtcNow;
            mainReport.RegBusinessObject("persons_business", persons);
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
            return View();
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