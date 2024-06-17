using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public class MyClaimsIdentityFactory : ClaimsIdentityFactory<UserModel, string>
    {
        public override async Task<ClaimsIdentity> CreateAsync(UserManager<UserModel, string> userManager, UserModel user, string authenticationType)
        {
            var claimsIdentity = await base.CreateAsync(userManager, user, authenticationType);

            claimsIdentity.AddClaim(new Claim("MyApplication:GroupId", "42"));

            return claimsIdentity;
        }
    }
}
