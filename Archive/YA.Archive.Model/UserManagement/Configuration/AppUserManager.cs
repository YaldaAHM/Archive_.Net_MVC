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
    public class AppUserManager : UserManager<UserModel, string>
    {

        public AppUserManager(AppUserStore store) : base(store)
        {
            UserValidator = new UserValidator<UserModel>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
        }

        public AppUserManager(IUserStore<UserModel> store) : base(store)
        {
            UserValidator = new UserValidator<UserModel>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
        }

        public static AppUserManager Create(
        IdentityFactoryOptions<AppUserManager> options,
        IOwinContext context)
        {

            ArchiveDataContext db = context.Get<ArchiveDataContext>();
            AppUserManager manager = new AppUserManager(new AppUserStore(db));
            manager.SupportsUserClaim.Equals(true);



            manager.PasswordValidator = new CustomPasswordValidator
            {


            };


            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            return manager;

        }
    }
}

