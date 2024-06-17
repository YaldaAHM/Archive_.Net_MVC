using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public class CustomUserValidator : UserValidator<UserModel>
    {
        public CustomUserValidator(AppUserManager mgr)
            : base(mgr)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(UserModel user)
        {
           IdentityResult result = await base.ValidateAsync(user);
            AllowOnlyAlphanumericUserNames = false;
            RequireUniqueEmail = false;

            return result;
        }
    }
}
