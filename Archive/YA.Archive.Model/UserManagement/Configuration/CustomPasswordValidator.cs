using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public class CustomPasswordValidator : PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);
          
            RequiredLength = 6;
            RequireNonLetterOrDigit = false;
            RequireDigit = false;
            RequireLowercase = false;
            RequireUppercase = false;
            return result;
        }
    }
}
