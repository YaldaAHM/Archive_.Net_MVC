using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace YA.Archive.MVC2.Filters
{
    public class LocalizationFilter : ActionFilterAttribute
    {
        private readonly CultureInfo _defaultCulture;
        private readonly List<CultureInfo> _supportedCultures;

        public LocalizationFilter(CultureInfo defaultCulture, List<CultureInfo> supportedCultures)
        {
            _defaultCulture = defaultCulture;
            _supportedCultures = supportedCultures;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cultureName = (string)filterContext.RouteData.Values["culture"] ?? _defaultCulture.Name;

            if (!_supportedCultures.Exists(c => c.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase)))
            {
                cultureName = _defaultCulture.Name;
            }

            var culture = new CultureInfo(cultureName);

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            base.OnActionExecuting(filterContext);
        }
    }
}