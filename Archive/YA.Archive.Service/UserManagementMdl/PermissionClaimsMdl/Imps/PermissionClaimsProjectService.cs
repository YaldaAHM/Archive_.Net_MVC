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
    public class PermissionClaimsProjectService
    {
        private ArchiveDataContext _db;
        private PermissionClaimsService _permissionClaimsService;
        //private GroupService _groupService;
        private FolderService _folderService;
        public PermissionClaimsProjectService()
        {
            _db = new ArchiveDataContext();
            _permissionClaimsService = new PermissionClaimsService();
          //  _groupService=new GroupService();
            _folderService = new FolderService();
        }
        public PermissionClaimsProjectService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
            _permissionClaimsService = new PermissionClaimsService();
        }

        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore (_db));
            }
        }
        public bool Insert(int folderId,string userId,int? groupId)
        {
            _permissionClaimsService.Insert(
                         new PermissionClaimsGroupInsertRequest()
                         {
                             entity = new GroupClaimVM()
                             {
                                 ClaimType = folderId.ToString(),
                                 ClaimValue = "CanCreate",
                                 GroupId = groupId
                             },
                             User = new UserVM() { Id=userId}
                         });
            _permissionClaimsService.Insert(
                         new PermissionClaimsGroupInsertRequest()
                         {
                             entity = new GroupClaimVM()
                             {
                                 ClaimType = folderId.ToString(),
                                 ClaimValue = "CanEdit",
                                 GroupId = groupId
                             },
                             User = new UserVM() { Id = userId }
                         }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanDelete",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanView",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanUpdateLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanDeleteLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanUpdateFileLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanDeleteFileLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanUpdateCommentLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          }); _permissionClaimsService.Insert(
                          new PermissionClaimsGroupInsertRequest()
                          {
                              entity = new GroupClaimVM()
                              {
                                  ClaimType = folderId.ToString(),
                                  ClaimValue = "CanDeleteCommentLock",
                                  GroupId = groupId
                              },
                              User = new UserVM() { Id = userId }
                          });
            return true;

        }
        public List<PermissionClaimsModel> PermissionClaimsProjectInitialize(int ClientCenterId)
        {
            var responseFolder = _folderService
               .FindByExpression(new FolderFindByExpressionExpRequest()
               {
                  RequsetCurrentUserId= System.Web.HttpContext.Current.User.Identity.GetUserId(),
                   expression =x=>x.ClientCenterId==ClientCenterId
               });

            List<PermissionClaimsModel> inlineDefault = new List<PermissionClaimsModel>();
               
                foreach(var f in responseFolder.Entities) { 
                  inlineDefault.Add(new PermissionClaimsModel
                    {
                        ClaimId=f.Id.ToString(),
                        CreateClaimId="CanCreate",EditClaimId="CanEdit",
                        DeleteClaimId="CanDelete",ViewClaimId="CanView",
                      UpdateLockClaimId = "CanUpdateLock",
                      DeleteLockClaimId = "CanDeleteLock",
                      UpdateFileLockClaimId = "CanUpdateFileLock",
                      DeleteFileLockClaimId = "CanDeleteFileLock",
                      UpdateCommentLockClaimId = "CanUpdateCommentLock",
                      DeleteCommentLockClaimId = "CanDeleteCommentLock",
                        UpdateLock = false,DeleteLock=false,
                        UpdateFileLock = false,DeleteFileLock = false,
                        UpdateCommentLock = false,DeleteCommentLock=false,
                        Create =false,Edit=false,Delete=false,View=false,Description=f.Name,
                


                    });
        }
            return inlineDefault;
        }

        public PermissionClaimsUpdateResponse Update(PermissionClaimsUpdateRequest request)
        {
            try
            {
                _permissionClaimsService.Update(new PermissionClaimsUpdateRequest()
                                                {
                                                    Entities = request.Entities,
                                                    User = request.User
                                                });

                
                return new PermissionClaimsUpdateResponse
                {
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
            catch (Exception ex)
            {

                return new PermissionClaimsUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
        }

        public PermissionClaimsFindAllResponse FindAll(PermissionClaimsFindAllRequest request)
        {

            var cc = UserManager.FindById(request.User.Id).Claims.Where(c => c.GroupId == null);
            var permissionsModel = PermissionClaimsProjectInitialize(request.entity.User.ClientCenterId);// new List<PermissionClaimsModel>();
            foreach (var item in permissionsModel)
            {
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                        && x.ClaimValue == "CanCreate") != null)
                {
                    item.Create = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanEdit") != null)
                {
                    item.Edit = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                          && x.ClaimValue == "CanDelete") != null)
                {
                    item.Delete = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                          && x.ClaimValue == "CanView") != null)
                {
                    item.View = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanUpdateLock") != null)
                {
                    item.UpdateLock = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanDeleteLock") != null)
                {
                    item.DeleteLock = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanUpdateCommentLock") != null)
                {
                    item.UpdateCommentLock = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanDeleteCommentLock") != null)
                {
                    item.DeleteCommentLock = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanUpdateFileLock") != null)
                {
                    item.UpdateFileLock = true;
                }
                if (cc.FirstOrDefault(x => x.GroupId == null && x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == "CanDeleteFileLock") != null)
                {
                    item.DeleteFileLock = true;
                }

            }
            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = permissionsModel.ToViewModelList()
            };
        }
    }
}
