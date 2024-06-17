using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;

namespace YA.Archive.Service.FolderMdl.Access
{
    public class ApplyAccess
    {
        private static RoleManagementService _roleService;
        private static PermissionClaimsService _permissionClaimsService;
        public ApplyAccess()
        {
            _permissionClaimsService = new PermissionClaimsService();
        }
        public static IEnumerable<FolderVM> ApplyFolderAccessList(
            IEnumerable<FolderVM> enti, string requsetCurrentUserId)
        {
            _permissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = requsetCurrentUserId,
                  RoleName = RolesT.AdminClient,
              });

            if (responseUserIsInRole.IsInRole)
            {
                foreach (var e in enti)
                {
                    e.IsUpdateLock = false;
                    e.IsDeleteLock = false;
                    e.IsUpdateFileLock = false;
                    e.IsDeleteFileLock = false;
                    e.IsUpdateCommentLock = false;
                    e.IsDeleteCommentLock = false;
                    e.HasClaimUpdateLock = true;
                    e.HasClaimDeleteLock = true;
                    e.HasClaimUpdateFileLock = true;
                    e.HasClaimDeleteFileLock = true;
                    e.HasClaimEdit = true;
                    e.HasClaimDelete = true;
                    e.HasClaimView = true;
                    e.HasClaimInsert = true;


                }
                return enti;
            }
            foreach (var e in enti)
            {
                e.IsDeleteCommentLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsDeleteFileLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsDeleteLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsUpdateLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsUpdateCommentLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsUpdateFileLockCreateUser = (requsetCurrentUserId != e.CreateUserId);
                e.IsUpdateLock = (fpub.convert2miladi(e.EditLockDate) < DateTime.Now);
                e.IsDeleteLock = (fpub.convert2miladi(e.RemoveLockDate) < DateTime.Now);
                e.IsUpdateFileLock = (fpub.convert2miladi(e.EditFileLockDate) < DateTime.Now);
                e.IsDeleteFileLock = (fpub.convert2miladi(e.RemoveFileLockDate) < DateTime.Now);
                e.IsUpdateCommentLock = (fpub.convert2miladi(e.EditCommentLockDate) < DateTime.Now);
                e.IsDeleteCommentLock = (fpub.convert2miladi(e.RemoveCommentLockDate) < DateTime.Now);
                e.HasClaimUpdateLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateLock
                            && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDeleteLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteLock
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimUpdateFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDeleteFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimEdit = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanEdit
                && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDelete = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDelete
              && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimView = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanView
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimInsert = _permissionClaimsService.FindByExpression(
                   new PermissionClaimsFindByExpressionRequest()
                   {
                       expression =
                           x =>
                           x.ClaimType == e.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanCreate
                 && x.UserId == requsetCurrentUserId
                   }).Entities != null;

            }
            return enti;
        }
        public static FolderVM ApplyFolderAccess(
           FolderVM enti, string requsetCurrentUserId)
        {
            _permissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = requsetCurrentUserId,
                  RoleName = RolesT.AdminClient,
              });

            if (responseUserIsInRole.IsInRole)
            {
                    enti.IsUpdateLock = false;
                    enti.IsDeleteLock = false;
                    enti.IsUpdateFileLock = false;
                    enti.IsDeleteFileLock = false;
                    enti.IsUpdateCommentLock = false;
                    enti.IsDeleteCommentLock = false;
                    enti.HasClaimUpdateLock = true;
                    enti.HasClaimDeleteLock = true;
                    enti.HasClaimUpdateFileLock = true;
                    enti.HasClaimDeleteFileLock = true;
                    enti.HasClaimEdit = true;
                    enti.HasClaimDelete = true;
                    enti.HasClaimView = true;
                    enti.HasClaimInsert = true;
 return enti;
                }

            enti.IsDeleteCommentLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsDeleteFileLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsDeleteLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsUpdateLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsUpdateCommentLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsUpdateFileLockCreateUser = (requsetCurrentUserId != enti.CreateUserId);
            enti.IsUpdateLock = (fpub.convert2miladi(enti.EditLockDate) < DateTime.Now);
                enti.IsDeleteLock = (fpub.convert2miladi(enti.RemoveLockDate) < DateTime.Now);
                enti.IsUpdateFileLock = (fpub.convert2miladi(enti.EditFileLockDate) < DateTime.Now);
                enti.IsDeleteFileLock = (fpub.convert2miladi(enti.RemoveFileLockDate) < DateTime.Now);
                enti.IsUpdateCommentLock = (fpub.convert2miladi(enti.EditCommentLockDate) < DateTime.Now);
                enti.IsDeleteCommentLock = (fpub.convert2miladi(enti.RemoveCommentLockDate) < DateTime.Now);
                enti.HasClaimUpdateLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateLock
                            && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDeleteLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteLock
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimUpdateFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDeleteFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimEdit = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanEdit
                && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDelete = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDelete
              && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimView = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanView
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimInsert = _permissionClaimsService.FindByExpression(
                   new PermissionClaimsFindByExpressionRequest()
                   {
                       expression =
                           x =>
                           x.ClaimType == enti.Id.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanCreate
                 && x.UserId == requsetCurrentUserId
                   }).Entities != null;
            return enti;
        }

        public static IEnumerable<FileVM> ApplyFileAccessList(IEnumerable<FileVM> enti, string requsetCurrentUserId)
        {
            _permissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = requsetCurrentUserId,
                  RoleName = RolesT.AdminClient,
              });
            var responseUserIsInRole1 = _roleService.FindUserIsInRole(
             new UserInRoleFindRequest()
             {
                 Id = requsetCurrentUserId,
                 RoleName = RolesT.Administrators,
             });

            if (responseUserIsInRole.IsInRole || responseUserIsInRole1.IsInRole)
            {
                foreach (var e in enti)
                {
                    e.IsUpdateLock = false;
                    e.IsDeleteLock = false;
                    e.IsUpdateFileLock = false;
                    e.IsDeleteFileLock = false;
                    e.IsUpdateCommentLock = false;
                    e.IsDeleteCommentLock = false;
                    e.HasClaimUpdateLock = true;
                    e.HasClaimDeleteLock = true;
                    e.HasClaimUpdateFileLock = true;
                    e.HasClaimDeleteFileLock = true;
                    e.HasClaimEdit = true;
                    e.HasClaimDelete = true;
                    e.HasClaimView = true;
                    e.HasClaimInsert = true;

                }
                return enti;
            }
            foreach (var e in enti)
            {
                e.IsDeleteFileLockCreateUser = (requsetCurrentUserId != e.Folder.CreateUserId);
                e.IsDeleteLockCreateUser = (requsetCurrentUserId != e.Folder.CreateUserId);
                e.IsUpdateLockCreateUser = (requsetCurrentUserId != e.Folder.CreateUserId);
                e.IsUpdateFileLockCreateUser = (requsetCurrentUserId != e.Folder.CreateUserId);
                e.IsUpdateLock = (fpub.convert2miladi(e.Folder.EditLockDate) < DateTime.Now);
                e.IsDeleteLock = (fpub.convert2miladi(e.Folder.RemoveLockDate) < DateTime.Now);
                e.IsUpdateFileLock = (fpub.convert2miladi(e.Folder.EditFileLockDate) < DateTime.Now);
                e.IsDeleteFileLock = (fpub.convert2miladi(e.Folder.RemoveFileLockDate) < DateTime.Now);
                e.HasClaimUpdateLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {expression = 
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateLock
                            && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDeleteLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteLock
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimUpdateFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDeleteFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimEdit = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanEdit
                && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimDelete = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDelete
              && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimView = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanView
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                e.HasClaimInsert = _permissionClaimsService.FindByExpression(
                   new PermissionClaimsFindByExpressionRequest()
                   {
                       expression =
                           x =>
                           x.ClaimType == e.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanCreate
                 && x.UserId == requsetCurrentUserId
                   }).Entities != null;

            }
            return enti;
        }

        public static FileVM ApplyFileAccess(FileVM enti, string requsetCurrentUserId)
        {
            _permissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = requsetCurrentUserId,
                  RoleName = RolesT.AdminClient,
              });
            var responseUserIsInRole1 = _roleService.FindUserIsInRole(
             new UserInRoleFindRequest()
             {
                 Id = requsetCurrentUserId,
                 RoleName = RolesT.Administrators,
             });

            if (responseUserIsInRole.IsInRole || responseUserIsInRole1.IsInRole)
            {
                enti.IsUpdateLock = false;
                enti.IsDeleteLock = false;
                enti.IsUpdateFileLock = false;
                enti.IsDeleteFileLock = false;
                enti.IsUpdateCommentLock = false;
                enti.IsDeleteCommentLock = false;
                enti.HasClaimUpdateLock = true;
                enti.HasClaimDeleteLock = true;
                enti.HasClaimUpdateFileLock = true;
                enti.HasClaimDeleteFileLock = true;
                enti.HasClaimEdit = true;
                enti.HasClaimDelete = true;
                enti.HasClaimView = true;
                enti.HasClaimInsert = true;
                return enti;
            }
            enti.IsDeleteFileLockCreateUser = (requsetCurrentUserId != enti.Folder.CreateUserId);
            enti.IsDeleteLockCreateUser = (requsetCurrentUserId != enti.Folder.CreateUserId);
            enti.IsUpdateLockCreateUser = (requsetCurrentUserId != enti.Folder.CreateUserId);
            enti.IsUpdateFileLockCreateUser = (requsetCurrentUserId != enti.Folder.CreateUserId);
            enti.IsUpdateLock = (fpub.convert2miladi(enti.Folder.EditLockDate) < DateTime.Now);
                enti.IsDeleteLock = (fpub.convert2miladi(enti.Folder.RemoveLockDate) < DateTime.Now);
                enti.IsUpdateFileLock = (fpub.convert2miladi(enti.Folder.EditFileLockDate) < DateTime.Now);
                enti.IsDeleteFileLock = (fpub.convert2miladi(enti.Folder.RemoveFileLockDate) < DateTime.Now);


                enti.HasClaimUpdateLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateLock
                            && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDeleteLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteLock
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimUpdateFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanUpdateFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDeleteFileLock = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDeleteFileLock
                  && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimEdit = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanEdit
                && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimDelete = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanDelete
              && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimView = _permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanView
                 && x.UserId == requsetCurrentUserId
                    }).Entities != null;
                enti.HasClaimInsert = _permissionClaimsService.FindByExpression(
                   new PermissionClaimsFindByExpressionRequest()
                   {
                       expression =
                           x =>
                           x.ClaimType == enti.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanCreate
                 && x.UserId == requsetCurrentUserId
                   }).Entities != null;

            return enti;
        }
    }
}
