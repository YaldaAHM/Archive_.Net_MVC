using AutoMapper;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web.UI.WebControls;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Mapping;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.GroupMdl.Imps;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Infrastructure.ClientCenterStrategy;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps
{
    public class PermissionClaimsGroupProjectService
    {
        private ArchiveDataContext _db;
        private PermissionClaimsService _permissionClaimsService;
        private PermissionClaimsGroupService _permissionClaimsGroupService;
        private PermissionClaimsProjectService _permissionClaimsProjectService;
        private GroupService _groupService;
        private FolderService _folderService;
        public PermissionClaimsGroupProjectService()
        {
            _db = new ArchiveDataContext();
            _permissionClaimsService = new PermissionClaimsService();
            _permissionClaimsGroupService = new PermissionClaimsGroupService();
            _permissionClaimsProjectService = new PermissionClaimsProjectService();
            _groupService =new GroupService();
            _folderService = new FolderService();
        }
        public PermissionClaimsGroupProjectService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore (_db));
            }
        }
      


        public PermissionClaimsGroupUpdateResponse Update(PermissionClaimsGroupUpdateRequest request)
        {
            try
            {

                _permissionClaimsGroupService.Update(new PermissionClaimsGroupUpdateRequest()
                {
                    Entities = request.Entities,
                    Group=request.Group
                    
                });
                
                return new PermissionClaimsGroupUpdateResponse
                {
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
            catch (Exception ex)
            {

                return new PermissionClaimsGroupUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
        }

        public PermissionClaimsGroupFindAllResponse FindAll(PermissionClaimsGroupFindAllRequest request)
        {

            var groupClaims = _db.GroupClaim.Where(g => g.GroupId == request.Group.Id);
            var permissionsModel =
                _permissionClaimsProjectService.PermissionClaimsProjectInitialize(request.Group.ClientCenterId);
             foreach (var item in permissionsModel)
            {
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                                    && x.ClaimValue == "CanCreate") != null)
                {
                    item.Create = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                                    && x.ClaimValue == "CanEdit") != null)
                {
                    item.Edit = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                                    && x.ClaimValue == "CanDelete") != null)
                {
                    item.Delete = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                                    && x.ClaimValue == "CanView") != null)
                {
                    item.View = true;
                }
               
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                           && x.ClaimValue == "CanUpdateLock") != null)
                {
                    item.UpdateLock = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                           && x.ClaimValue == "CanDeleteLock") != null)
                {
                    item.DeleteLock = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                          && x.ClaimValue == "CanUpdateFileLock") != null)
                {
                    item.UpdateFileLock = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                           && x.ClaimValue == "CanDeleteFileLock") != null)
                {
                    item.DeleteFileLock = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                           && x.ClaimValue == "CanUpdateCommentLock") != null)
                {
                    item.UpdateCommentLock = true;
                }
                if (groupClaims.FirstOrDefault(x => x.GroupId == request.Group.Id && x.ClaimType == item.ClaimId.ToString()
                                           && x.ClaimValue == "CanDeleteCommentLock") != null)
                {
                    item.DeleteCommentLock = true;
                }
            }


            return new PermissionClaimsGroupFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = permissionsModel.ToViewModelList()
                   };
        }
    }
}
