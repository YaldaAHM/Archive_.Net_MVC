using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.Owin.Security.Cookies;

namespace YA.Archive.MVC2
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            IdentityConfigure identity1 = new IdentityConfigure();
            identity1.Configuration(app);

        }
    }
}
