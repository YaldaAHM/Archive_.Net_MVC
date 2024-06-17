using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging
{
   public class PermissionClaimsFindByExpressionRequest:
        BaseFunkRequest<Func<ApplicationUserClaim, bool>>
      
    {
       
       // public GroupVM Group { get; set; }
    }
}

