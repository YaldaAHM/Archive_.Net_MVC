using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.GroupMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging
{
    public class PermissionClaimsGroupUpdateRequest : BaseListRequest<PermissionClaimsVM>
    {
        //public List<UserVM> Users { get; set; }
        public GroupVM Group { get; set; }
    }
}
