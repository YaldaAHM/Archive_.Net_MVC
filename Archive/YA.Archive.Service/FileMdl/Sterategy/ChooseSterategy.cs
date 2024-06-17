using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FileMdl.Sterategy
{
    public class ChooseSterategy
    {
        private static RoleManagementService _roleService;
        public static BaseListResponse<FileVM> FileFindByExpression(FileFindByExpressionExpRequest request)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = request.RequsetCurrentUserId,
                  RoleName = RolesT.MainCenter,
              });
            IClientCenterStrategy clientCenterStrategy =
                ClientCenterFactory.GetClientCenterStrategyFor(
                    responseUserIsInRole.IsInRole ? ClientCenterType.Main : ClientCenterType.Other);
            request.RequsetCurrentClientCenterId = responseUserIsInRole.entity.ClientCenterId;
            var c = clientCenterStrategy.ApplyFindByExpressionTo(request);
            return c;
        }
        public static BaseListResponse<FileVM> FileFindAll(FileFindAllRequest request)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = request.RequsetCurrentUserId,
                  RoleName = RolesT.MainCenter,
              });
            IClientCenterStrategy clientCenterStrategy =
                ClientCenterFactory.GetClientCenterStrategyFor(
                    responseUserIsInRole.IsInRole ? ClientCenterType.Main : ClientCenterType.Other);
            request.RequsetCurrentClientCenterId = responseUserIsInRole.entity.ClientCenterId;
            var c = clientCenterStrategy.ApplyFindAllTo(request);
            return c;
        }


        public static BaseListResponse<FileVM> FileFindByExpressionPaging(FileFindByExpressionExpRequest request)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = request.RequsetCurrentUserId,
                  RoleName = RolesT.MainCenter,
              });
            IClientCenterStrategy clientCenterStrategy =
                ClientCenterFactory.GetClientCenterStrategyFor(
                    responseUserIsInRole.IsInRole ? ClientCenterType.Main : ClientCenterType.Other);
            request.RequsetCurrentClientCenterId = responseUserIsInRole.entity.ClientCenterId;
            var c = clientCenterStrategy.ApplyFindByExpressionPagingTo(request);
            return c;
        }
        public static BaseListResponse<FileVM> FileFindAllPaging(FileFindAllRequest request)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = request.RequsetCurrentUserId,
                  RoleName = RolesT.MainCenter,
              });
            IClientCenterStrategy clientCenterStrategy =
                ClientCenterFactory.GetClientCenterStrategyFor(
                    responseUserIsInRole.IsInRole ? ClientCenterType.Main : ClientCenterType.Other);
            request.RequsetCurrentClientCenterId = responseUserIsInRole.entity.ClientCenterId;
            var c = clientCenterStrategy.ApplyFindAllPagingTo(request);
            return c;
        }
    }
}
