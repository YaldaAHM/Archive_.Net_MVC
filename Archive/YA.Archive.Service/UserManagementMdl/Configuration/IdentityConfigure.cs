using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Model.UserManagement.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace YA.Archive.Service.UserManagementMdl.Configuration
{
    public class IdentityConfigure
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<ArchiveDataContext>(ArchiveDataContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);
           
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LogoutPath = new PathString("/Account/Logout"),
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}
