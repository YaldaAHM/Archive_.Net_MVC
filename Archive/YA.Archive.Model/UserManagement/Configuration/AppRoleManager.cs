using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public class AppRoleManager : RoleManager<RoleModel>, IDisposable
    {
        public AppRoleManager(RoleStore<RoleModel> store)
            : base(store)
        {
        }

        public static AppRoleManager Create(
            IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context)
        {
            return new AppRoleManager(new
                RoleStore<RoleModel>(context.Get<ArchiveDataContext>()));
        }
    }
}