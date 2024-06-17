using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.GroupUserMdl.VMs
{
    public class GroupUserVM : ViewModelBase<int>
    {
        public virtual GroupVM Group { get; set; }
        public int GroupId { get; set; }
        public virtual UserVM User { get; set; }
        public string UserId { get; set; }
    }
}
