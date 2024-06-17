using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging
{
    public class PermissionClaimsGroupDeleteRequest : BaseEntityRequest<GroupClaimVM>
    {
        //public GroupVM Group { get; set; }
        public UserVM User { get; set; }
    }
}
