using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.UserManagement;

namespace YA.Archive.Service.UserManagementMdl.VMs.Role
{
    public class RoleEditVM
    {
        public RoleModel Role { get; set; }
        public IEnumerable<UserVM> Members { get; set; }
        public IEnumerable<UserVM> NonMembers { get; set; }
    }
}
