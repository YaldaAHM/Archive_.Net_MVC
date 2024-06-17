using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.Group;

namespace YA.Archive.Model.UserManagement
{
    public class ApplicationUserClaim : IdentityUserClaim
    {
        public int? GroupId { get; set; }
    }
}
