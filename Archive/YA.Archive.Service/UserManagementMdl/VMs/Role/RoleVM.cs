using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Service.UserManagementMdl.VMs.Role
{
    public class RoleVM
    {
       
        ////
        //// Summary:
        ////     Role id
        public string Id { get; set; }
        //
        // Summary:
        //     Role name
        public string Name { get; set; }
        //
        // Summary:
        //     Navigation property for users in the role
        public virtual ICollection<IdentityUserRole> Users { get; }
       // public IdentityUserRole();

        //
        // Summary:
        //     RoleId for the role
        public virtual string RoleId { get; set; }
        //
        // Summary:
        //     UserId for the user that is in the role
        public virtual string UserId { get; set; }
    }
}
