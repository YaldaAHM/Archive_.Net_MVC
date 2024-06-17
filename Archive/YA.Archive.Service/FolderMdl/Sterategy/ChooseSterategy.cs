using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Infrastructure.Service;
using YA.Archive.Service.FolderMdl.Messaging;

namespace YA.Archive.Service.FolderMdl.Sterategy
{
    public class ChooseSterategy
    {
        private static RoleManagementService _roleService;
        public static BaseListResponse<FolderVM> FolderFindByExpression(FolderFindByExpressionExpRequest request)
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
        public static BaseListResponse<FolderVM> FolderFindAll(FolderFindAllRequest request)
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
        public static int ClientCenterId(string userId)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = userId,
                  RoleName = RolesT.MainCenter,
              });
            return responseUserIsInRole.IsInRole? 0 : responseUserIsInRole.entity.ClientCenterId;
        }

        public static BaseListResponse<FolderVM> FolderFindByExpressionPaging(FolderFindByExpressionExpRequest request)
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
        public static BaseListResponse<FolderVM> FolderFindAllPaging(FolderFindAllRequest request)
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
