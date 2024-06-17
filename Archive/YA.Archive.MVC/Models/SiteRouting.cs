using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YA.Archive.MVC2.Models
{
    public class SiteRouting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string RouteValue { get; set; }
        public string Url { get; set; }
    }
}