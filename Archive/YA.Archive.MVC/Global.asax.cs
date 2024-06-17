using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
//using YA.Archive.MVC2.App_Start;
using YA.Archive.MVC2.Filters;
using YA.Archive.Service;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Security;
using static Stimulsoft.Report.Func;
using static Stimulsoft.Report.StiOptions;

namespace YA.Archive.MVC2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private UserLogTimeService _userLogTimeService;
        protected void Application_Start()
        {
             AutoMapperConfiguration.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
          

            // Localization setup
            var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            //new CultureInfo("fr")
        };

            var defaultCulture = new CultureInfo("en");
          

        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            _userLogTimeService=new UserLogTimeService();

        }
        protected void Session_End(Object sender, EventArgs e)
        {
          
        }
    }
}
