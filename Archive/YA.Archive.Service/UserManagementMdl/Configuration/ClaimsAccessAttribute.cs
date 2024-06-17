using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Model.UserManagement;

namespace YA.Archive.Service.UserManagementMdl.Configuration
{
    public class ClaimsAccessAttribute : AuthorizeAttribute
    {
        public string Issuer { get; set; }
        public string ClaimType { get; set; }
        public string Value { get; set; }
        protected override bool AuthorizeCore(HttpContextBase context)
        {
            return context.User.Identity.IsAuthenticated
            && context.User.Identity is ClaimsIdentity
            && ((ClaimsIdentity)context.User.Identity).HasClaim(x =>
            x.Type == ClaimType && x.Value == Value
            );
        }
    }
}
