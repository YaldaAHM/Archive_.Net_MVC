using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;

namespace YA.Archive.Service.ClientCenterStrategy
{
    public class ChooseClientCenter
    {
        public static ClientCenterInfo ClientCenterId(string userId)
        {
           var _roleService = new RoleManagementService();
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = userId,
                  RoleName = RolesT.MainCenter,
              });
            return responseUserIsInRole.IsInRole ?
                new ClientCenterInfo() {ClientCenterId=0,ClientCenterType = ClientCenterType.Main }
                : new ClientCenterInfo() { ClientCenterId = responseUserIsInRole.entity.ClientCenterId,
                    ClientCenterType = ClientCenterType.Other }
                ;
        }
    }
}
