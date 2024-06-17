using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Service.UserManagementMdl.VMs
{
    public class ClaimVM: IdentityUserClaim
    {
        public int? GroupId { get; set; }

        //
        // Summary:
        //     Claim type
        public virtual string ClaimType { get; set; }
        //
        // Summary:
        //     Claim value
        public virtual string ClaimValue { get; set; }
        //
        // Summary:
        //     Primary key
        public virtual int Id { get; set; }
        //
        // Summary:
        //     User Id for the user who owns this login
        public virtual string UserId { get; set; }
    }
}
