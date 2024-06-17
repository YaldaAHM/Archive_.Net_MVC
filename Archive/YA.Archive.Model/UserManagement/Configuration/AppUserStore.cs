using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public class AppUserStore : UserStore<UserModel, IdentityRole, string,
        IdentityUserLogin, IdentityUserRole, ApplicationUserClaim>
    {
        public AppUserStore(ArchiveDataContext context)
            : base(context)
        {
        }
    }
}
