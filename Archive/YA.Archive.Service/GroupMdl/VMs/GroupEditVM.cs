using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.GroupMdl.VMs
{
    public class GroupEditVM
    {
        public int GroupId { get; set; }
        public GroupVM Group { get; set; }
        public IEnumerable<UserVM> Members { get; set; }
        public IEnumerable<UserVM> NonMembers { get; set; }
    }
}
