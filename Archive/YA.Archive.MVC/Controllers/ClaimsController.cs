using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        // GET: Claims
        [Authorize]
        public ActionResult Index()
        {
            //ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            //if (ident == null)
            //{
            //    return View("Error", new string[] { "No claims available" });
            //}
            //else
            //{
            //    return View(ident.Claims);
            //}
            return View();
        }
    }
}