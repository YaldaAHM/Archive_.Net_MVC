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
using YA.Archive.Infrastructure.ClientCenterStrategy;
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
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Sterategy;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps
{
    public class PermissionClaimsGroupService
    {
        private ArchiveDataContext _db;
        private PermissionClaimsService _permissionClaimsService;
        private GroupService _groupService;
        public PermissionClaimsGroupService()
        {
            _db = new ArchiveDataContext();
            _permissionClaimsService = new PermissionClaimsService();
            _groupService=new GroupService();
        }
        public PermissionClaimsGroupService(ArchiveDataContext db)
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
                if (request.Group != null)
                {
                    var group = _groupService.FindById(new GroupFindByIdRequest() { Id = request.Group.Id });
                    if (group.IsSuccess && group.entity == null)
                    {
                        return new PermissionClaimsGroupUpdateResponse
                        {
                            ResponseType = ResponseType.Error,
                            Entities = request.Entities

                        };
                    }
                    foreach (var r in request.Entities)
                    {
                        var gcCreate = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                                                                        g.ClaimType == r.ClaimId &&
                                                                        g.ClaimValue == r.CreateClaimId);
                        if (r.Create)
                        {

                            if (gcCreate == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.CreateClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.CreateClaimId,
                                            }
                                        });
                                }

                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcCreate != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.CreateClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });
                                }


                                _db.GroupClaim.Remove(gcCreate);

                                _db.SaveChanges();

                            }
                        }
                        var gcEdit = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                                                                       g.ClaimType == r.ClaimId &&
                                                                       g.ClaimValue == r.EditClaimId);
                        if (r.Edit)
                        {

                            if (gcEdit == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.EditClaimId,
                                });

                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.EditClaimId,
                                            }
                                        });
                                }

                                _db.SaveChanges();
                            }

                        }
                        else
                        {
                            if (gcEdit != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.EditClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });
                                }
                                _db.GroupClaim.Remove(gcEdit);
                                _db.SaveChanges();
                            }
                        }
                        var gcDelete = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                                                                                              g.ClaimType == r.ClaimId &&
                                                                                              g.ClaimValue == r.DeleteClaimId);
                        if (r.Delete)
                        {

                            if (gcDelete == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.DeleteClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteClaimId,
                                            }
                                        });
                                }
                            }
                            _db.SaveChanges();
                        }
                        else
                        {
                            if (gcDelete != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });
                                }
                                _db.GroupClaim.Remove(gcDelete);
                                _db.SaveChanges();
                            }
                        }
                        var gcView = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                                                                       g.ClaimType == r.ClaimId &&
                                                                       g.ClaimValue == r.ViewClaimId);
                        if (r.View)
                        {

                            if (gcView == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.ViewClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.ViewClaimId,
                                            }
                                        });

                                }
                            }
                            _db.SaveChanges();
                        }
                        else
                        {
                            if (gcView != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.ViewClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });
                                }
                                _db.GroupClaim.Remove(gcView);
                                _db.SaveChanges();
                            }
                        }

                        var gcUpdateLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                                                                        g.ClaimType == r.ClaimId &&
                                                                        g.ClaimValue == r.UpdateLockClaimId);
                        if (r.UpdateLock)
                        {
                            if (gcUpdateLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.UpdateLockClaimId,
                                });

                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcUpdateLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });


                                }
                                _db.GroupClaim.Remove(gcUpdateLock);
                                _db.SaveChanges();
                            }
                        }
                        //////////////////////////////////
                        var gcDeleteLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                        g.ClaimType == r.ClaimId &&
                        g.ClaimValue == r.DeleteLockClaimId);
                        if (r.DeleteLock)
                        {
                            if (gcDeleteLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.DeleteLockClaimId,
                                });

                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcDeleteLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });

                                }
                                _db.GroupClaim.Remove(gcDeleteLock);
                                _db.SaveChanges();
                            }
                        }
                        ///////////////////////////////////
                        var gcUpdateFileLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                       g.ClaimType == r.ClaimId &&
                       g.ClaimValue == r.UpdateFileLockClaimId);
                        if (r.UpdateFileLock)
                        {
                            if (gcUpdateFileLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.UpdateFileLockClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateFileLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcUpdateFileLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateFileLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });

                                }
                                _db.GroupClaim.Remove(gcUpdateFileLock);
                                _db.SaveChanges();
                            }
                        }
                        /////////////////////////////////////
                        var gcDeleteFileLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                        g.ClaimType == r.ClaimId &&
                        g.ClaimValue == r.DeleteFileLockClaimId);
                        if (r.DeleteFileLock)
                        {
                            if (gcDeleteFileLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.DeleteFileLockClaimId,
                                });

                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteFileLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcDeleteFileLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteFileLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });

                                }
                                _db.GroupClaim.Remove(gcDeleteFileLock);
                                _db.SaveChanges();
                            }
                        }
                        /////////////////////////////////////////////////
                        var gcUpdateCommentLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                       g.ClaimType == r.ClaimId &&
                       g.ClaimValue == r.UpdateCommentLockClaimId);
                        if (r.UpdateCommentLock)
                        {
                            if (gcUpdateCommentLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.UpdateCommentLockClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateCommentLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcUpdateCommentLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.UpdateCommentLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });

                                }

                                _db.GroupClaim.Remove(gcUpdateCommentLock);
                                _db.SaveChanges();
                            }
                        }
                        ///////////////////////////////////////////////////////
                        var gcDeleteCommentLock = _db.GroupClaim.FirstOrDefault(g => g.GroupId == request.Group.Id &&
                       g.ClaimType == r.ClaimId &&
                       g.ClaimValue == r.DeleteCommentLockClaimId);
                        if (r.DeleteCommentLock)
                        {
                            if (gcDeleteCommentLock == null)
                            {
                                _db.GroupClaim.Add(new GroupClaimModel()
                                {
                                    GroupId = request.Group.Id,
                                    ClaimType = r.ClaimId,
                                    ClaimValue = r.DeleteCommentLockClaimId,
                                });
                                foreach (var u in group.entity.GroupUsers)
                                {
                                    _permissionClaimsService.Insert(
                                        new PermissionClaimsGroupInsertRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                GroupId = request.Group.Id,
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteCommentLockClaimId,
                                            }
                                        });
                                }
                                _db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (gcDeleteCommentLock != null)
                            {
                                foreach (var u in group.entity.GroupUsers)
                                {

                                    _permissionClaimsService.Delete(
                                        new PermissionClaimsGroupDeleteRequest()
                                        {
                                            User = u.User,
                                            entity = new GroupClaimVM()
                                            {
                                                ClaimType = r.ClaimId,
                                                ClaimValue = r.DeleteCommentLockClaimId,
                                                GroupId = request.Group.Id
                                            }
                                        });

                                }
                                _db.GroupClaim.Remove(gcDeleteCommentLock);
                                _db.SaveChanges();
                            }
                        }

                    }

                }
                return new PermissionClaimsGroupUpdateResponse
                        {
                            
                            IsSuccess=true,
                            ResponseType = ResponseType.Success,
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


            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            IClientCenterStrategy clientCenterStrategy =
              ClientCenterFactory.GetClientCenterStrategyFor(request.ClientCenterType);
            var lpc = clientCenterStrategy.ApplyFindAllTo();

            foreach (var item in lpc)
            {
                item.GroupId = request.Group.Id;
                if (groupClaims.FirstOrDefault(x => x.ClaimType == item.ClaimId
                && x.ClaimValue == item.CreateClaimId) != null)
                {
                    item.Create = true;
                }
                if (groupClaims.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.EditClaimId) != null)
                {
                    item.Edit = true;
                }
                if (groupClaims.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.DeleteClaimId) != null)
                {
                    item.Delete = true;
                }
                if (groupClaims.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.ViewClaimId) != null)
                {
                    item.View = true;
                }

            }
            return new PermissionClaimsGroupFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
            };

        }
    }
}
