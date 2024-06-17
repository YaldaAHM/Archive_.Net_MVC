using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Infrastructure.Infrastructure.Permission;

namespace YA.Archive.Service.FolderMdl.Access
{
    public class AllowAccess
    {
        private static PermissionClaimsService _permissionClaimsService;
        private static RoleManagementService _roleService;

        public AllowAccess()
        {
            _permissionClaimsService = new PermissionClaimsService();
        }

        public static bool AllowFolderAccess(string claimType,
            string claimValue, string requsetCurrentUserId)
        {
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = requsetCurrentUserId,
                  RoleName = RolesT.AdminClient,
              });

            if (responseUserIsInRole.IsInRole) return true;
            var responseUserIsInRole1 = _roleService.FindUserIsInRole(
             new UserInRoleFindRequest()
             {
                 Id = requsetCurrentUserId,
                 RoleName = RolesT.Administrators,
             });

            if (responseUserIsInRole1.IsInRole) return true;
            if (_permissionClaimsService.FindByExpression(
                new PermissionClaimsFindByExpressionRequest()
                {
                    expression =
                        x =>
                        x.ClaimType == claimType && x.ClaimValue == claimValue
                        && x.UserId == requsetCurrentUserId
                }).Entities != null)
            {
                return true;
            }
            return false;
        }
    }
}
