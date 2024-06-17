using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace YA.Archive.Model.UserManagement.Configuration
{
    public interface IClaimsIdentityFactory<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Create a ClaimsIdentity from an user using a UserManager
        /// </summary>
        Task<ClaimsIdentity> CreateAsync(UserManager<TUser, TKey> manager, TUser user, string authenticationType);
    }
}
