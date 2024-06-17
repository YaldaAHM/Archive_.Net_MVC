using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Model.UserManagement.PermissionClaims;
using YA.Infrastructure.Service;
using YA.Archive.Infrastructure.Infrastructure.Permission;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<List<PermissionClaimsModel>,String,String>
    {
           List<PermissionClaimsModel> ApplyFindAllTo();

       
    }
}
