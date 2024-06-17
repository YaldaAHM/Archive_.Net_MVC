using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging
{
    public class PermissionClaimsFindAllRequest : BaseEntityResponse<PermissionClaimsVM>
    {
        public int CurrentUserClientCenterId { get; set; }
        public ClientCenterType ClientCenterType { get; set; }
        public UserVM User;
    }
}
