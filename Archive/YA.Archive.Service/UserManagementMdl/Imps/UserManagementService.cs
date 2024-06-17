using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.UserLocalization;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.ClientCenterStrategy;
using YA.Archive.Service.FileMdl.Imps;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.GroupUserMdl.Imps;
using YA.Archive.Service.GroupUserMdl.Mapping;
using YA.Archive.Service.GroupUserMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using Microsoft.Owin.Security;
using YA.Archive.Service.UserManagementMdl.Sterategy;
//using Microsoft.AspNet.Identity.Owin;

namespace YA.Archive.Service.UserManagementMdl.Imps
{
    public class UserManagementService
    {
        private ArchiveDataContext _db;
        private PermissionClaimsService _permissionClaimsService;
        private RoleManagementService _roleService;
        private GroupUserService _groupUserService;
        private FolderService _folderService;
        private FileService _fileService;

        public UserManagementService()
        {
            _db = new ArchiveDataContext();
            _permissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
            _groupUserService = new GroupUserService();
            _folderService = new FolderService();
            _fileService = new FileService();

        }

        public List<UserVM> AllUsers()
        {
            var m = UserManager.Users;
            return m.ToViewModelList().ToList();
        }
        private AppUserManager UserManager
        {
            get
            {
                AppUserManager manager = new AppUserManager(new AppUserStore(_db));

                return new AppUserManager(new AppUserStore(_db));
            }
        }


        public UserInsertResponse Insert(UserInsertRequest request)
        {
            try
            {

                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;
                UserModel user = new UserModel
                {
                    UserName = request.entity.UserName,

                    ClientCenterId = request.entity.ClientCenterId,
                    GroupUsers = request.entity.GroupUsers.ToModelList().ToList()
                };
                if (UserManager.FindByName(request.entity.UserName) != null)
                {
                    return new UserInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, UserResource.UserName),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                if (!request.entity.IsAdmin && request.entity.UserName.ToLower().Contains("admin"))
                {
                    return new UserInsertResponse
                    {
                        Message = String.Format(MessageResource.Limited, UserResource.UserName),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }

                IdentityResult result = UserManager.Create(user,
                       request.entity.Password);
                if (!result.Succeeded)
                {
                    return new UserInsertResponse
                    {
                        Message = result.Errors.FirstOrDefault(),
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };

                }
                if (request.entity.GroupUsers != null)
                    foreach (var g in request.entity.GroupUsers)
                    {
                        var groupClaim = _db.GroupClaim.Where(gc => gc.GroupId == g.GroupId);
                        foreach (var gc in groupClaim)
                        {
                            _permissionClaimsService.Insert(new PermissionClaimsGroupInsertRequest()
                            {
                                entity = gc.ToViewModel(),
                                User = user.ToViewModel()
                            });
                        }

                    }
                var createdUser = UserManager.FindByName(user.UserName);
                var client = _db.ClientCenter.FirstOrDefault(x => x.Name == ClientCenterR.MainCenterName);
                if (client.Id == request.entity.ClientCenterId)
                {
                    if (!UserManager.IsInRole(createdUser.Id, RolesT.MainCenter))
                    {
                        UserManager.AddToRole(createdUser.Id, RolesT.MainCenter);
                    }
                }
                _db.SaveChanges();
                var u = UserManager.FindByName(request.entity.UserName);
                if (u == null)
                    return new UserInsertResponse
                    {
                        IsSuccess = true,
                        Message = MessageResource.InsertSuccess,
                        ResponseType = ResponseType.Success,
                        entity = request.entity
                    };
                foreach (var t in u.GroupUsers)
                {
                    var tt = _db.Group.FirstOrDefault(x => x.Id == t.GroupId);
                    t.Group = tt;
                }
                return new UserInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = new RegistrationVM()
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        ClientCenterId = u.ClientCenterId,
                        GroupUsers = u.GroupUsers.ToViewModelList().ToList(),

                    }
                };

            }
            catch (Exception ex)
            {

                return new UserInsertResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public UserUpdateResponse Update(UserUpdateRequest request)
        {
            try
            {
                if (request.entity.UserName == "Admin")
                {
                    return new UserUpdateResponse
                    {
                        Message = String.Format(MessageResource.Limited),
                        ResponseType = ResponseType.Error,

                    };
                }

                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;

                UserModel user = UserManager.FindById(request.entity.Id);
                if (user != null)
                {
                    if (UserManager.FindByName(request.entity.UserName) != null &&
                        UserManager.FindByName(request.entity.UserName).Id != request.entity.Id)
                    {
                        return new UserUpdateResponse
                        {
                            Message = String.Format(MessageResource.Exist, UserResource.UserName),
                            ResponseType = ResponseType.Warning,
                            entity = request.entity
                        };
                    }
                    if (request.entity.UserName.ToLower().Contains("admin") &&
                        (!UserManager.IsInRole(request.entity.Id, RolesT.Administrators) ||
                         !UserManager.IsInRole(request.entity.Id, RolesT.AdminClient)))
                    {
                        return new UserUpdateResponse
                        {
                            Message = String.Format(MessageResource.Limited, UserResource.UserName),
                            ResponseType = ResponseType.Warning,
                            entity = request.entity
                        };
                    }
                    foreach (var g in user.GroupUsers)
                    {

                        var userGroup = request.entity.GroupUsers.FirstOrDefault(gu => gu.GroupId == g.GroupId);
                        if (userGroup == null)
                        {
                            var groupClaim = _db.GroupClaim.Where(gc => gc.GroupId == g.GroupId);
                            foreach (var gc in groupClaim)
                            {
                                _permissionClaimsService.Delete(new PermissionClaimsGroupDeleteRequest()
                                {
                                    entity = gc.ToViewModel(),
                                    User = user.ToViewModel()
                                });
                            }

                        }
                    }
                    if (request.entity.GroupUsers != null)
                    {
                        foreach (var g in request.entity.GroupUsers)
                        {
                            var userGroup = user.GroupUsers.FirstOrDefault(gu => gu.GroupId == g.GroupId);
                            if (userGroup == null)
                            {
                                var groupClaim = _db.GroupClaim.Where(gc => gc.GroupId == g.GroupId);
                                foreach (var gc in groupClaim)
                                {
                                    _permissionClaimsService.Insert(new PermissionClaimsGroupInsertRequest()
                                    {
                                        entity = gc.ToViewModel(),
                                        User = user.ToViewModel()
                                    });
                                }
                            }
                        }
                    }
                    user.Email = request.entity.Email;

                    if (request.entity.Password != null)
                    {

                        UserManager.RemovePassword(user.Id);
                        IdentityResult result1 = UserManager.AddPassword(user.Id, request.entity.Password);


                    }

                    user.ClientCenterId = request.entity.ClientCenterId.Value;
                    user.UserName = request.entity.UserName;
                    user.GroupUsers = request.entity.GroupUsers.ToModelList().ToList();
                    IdentityResult result = UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        foreach (var t in user.GroupUsers)
                        {
                            var tt = _db.Group.FirstOrDefault(x => x.Id == t.GroupId);
                            t.Group = tt;
                        }
                        return new UserUpdateResponse
                        {
                            IsSuccess = true,
                            Message = MessageResource.UpdateSuccess,
                            ResponseType = ResponseType.Success,
                            entity = new EditVM()
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                ClientCenterId = user.ClientCenterId,
                                GroupUsers = user.GroupUsers.ToViewModelList().ToList(),

                            }
                        };

                    }
                    return new UserUpdateResponse
                    {
                        IsSuccess = false,
                        Message = MessageResource.UpdateField,
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }
                return new UserUpdateResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.UpdateField,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new UserUpdateResponse
                {
                    Message = MessageResource.Field,

                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public UserDeleteResponse DeleteById(UserDeleteByIdRequest request)
        {
            try
            {
                var user = UserManager.FindById(request.Id);
                if (user != null)
                {
                    if (user.UserName == "Admin")
                    {
                        return new UserDeleteResponse
                        {
                            Message = String.Format(MessageResource.Limited),
                            ResponseType = ResponseType.Error,

                        };
                    }
                    var responseFolder = _folderService.FindByExpression(
                        new FolderFindByExpressionExpRequest()
                        {
                            RequsetCurrentUserId = request.RequsetCurrentUserId,
                            expression = (x => x.CreateUserId == request.Id
                                               || x.LastUpdateUserId == request.Id
                                               || (x.Comments.Any() && (x.Comments.Any(
                                                   c => c.CreateUserId == request.Id
                                                   || c.LastUpdateUserId == request.Id))
                               )),
                        });
                    if (responseFolder.Entities.Count() != 0)
                    {
                        return new UserDeleteResponse
                        {
                            Message = MessageResource.DeleteField,
                            ResponseType = ResponseType.Error,
                        };
                    }
                    var responseFile = _fileService.FindByExpression(
                        new FileFindByExpressionExpRequest()
                        {
                            RequsetCurrentUserId = request.RequsetCurrentUserId,
                            expression = x => x.CreateUserId == request.Id
                                              || x.LastUpdateUserId == request.Id

                        });
                    UserManager.GetClaims(user.Id).ToList().ForEach(claim => UserManager.RemoveClaim(user.Id, claim));
                    user.Roles.ToList().ForEach(role => user.Roles.Remove(role));

                    user.GroupUsers.ToList().ForEach(p => _db.Entry(p).State = EntityState.Deleted);

                    IdentityResult result = UserManager.Delete(user);

                    if (result.Succeeded)
                        return new UserDeleteResponse
                        {
                            IsSuccess = true,
                            Message = MessageResource.DeleteSuccess,
                            ResponseType = ResponseType.Success,
                        };
                    else
                    {
                        return new UserDeleteResponse
                        {
                            Message = MessageResource.DeleteField,
                            ResponseType = ResponseType.Error,
                        };
                    }
                }
                return new UserDeleteResponse
                {
                    Message = MessageResource.DeleteField,
                    ResponseType = ResponseType.Error,
                };
            }
            catch (Exception ex)
            {

                return new UserDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public UserFindAllResponse FindAll(UserFindAllRequest request)
        {

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

            return new UserFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                //Entities = ents
                Entities = c.Entities,
            };

        }
        public UserFindAllResponse FindAllPaging(UserFindAllRequest request)
        {

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

            return new UserFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                //Entities = ents
                Entities = c.Entities,
                TotalRecords = c.TotalRecords,
                Count = c.Count,
                Index = c.Index
            };

        }

        public UserFindByIdResponse FindById(UserFindByIdRequest request)
        {
            try
            {
                var e = UserManager.FindById(request.Id);
                var entity = e.ToViewModel();
                var claims = new List<ClaimVM>();
                if (e.Claims != null)
                {
                    foreach (var c in e.Claims)
                    {
                        claims.Add(new ClaimVM()
                        {
                            ClaimType = c.ClaimType,
                            ClaimValue = c.ClaimValue,
                            GroupId = c.GroupId,
                            UserId = c.UserId
                        });
                    }
                    entity.ClaimsG = claims;
                }
                return new UserFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity
                };

            }
            catch (Exception ex)
            {

                return new UserFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public UserFindByExpressionResponse FindByExpression(UserFindByExpressionRequest request)
        {


            var expression = ToViewModel(request.expression);
            var responseUserIsInRole = _roleService.FindUserIsInRole(
              new UserInRoleFindRequest()
              {
                  Id = request.RequsetCurrentUserId,
                  RoleName = "MainCenter",
              });
            IClientCenterStrategy clientCenterStrategy =
                ClientCenterFactory.GetClientCenterStrategyFor(
                    responseUserIsInRole.IsInRole ? ClientCenterType.Main : ClientCenterType.Other);
            request.RequsetCurrentClientCenterId = responseUserIsInRole.entity.ClientCenterId;
            var c = clientCenterStrategy.ApplyFindByExpressionTo(new UserFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,

            });

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = c.Entities

            };


        }
        public UserFindByExpressionResponse FindByExpression(UserFindByExpressionExpRequest request)
        {

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

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = c.Entities

            };

        }
        public UserFindByExpressionResponse FindByExpressionPaging(UserFindByExpressionExpRequest request)
        {

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

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = c.Entities,
                TotalRecords = c.TotalRecords,
                Count = c.Count,
                Index = c.Index

            };


        }
        public Expression<Func<UserModel, bool>> ToViewModel
            (Expression<Func<UserVM, bool>> User)
        {

            return Mapper.Map<Expression<Func<UserVM, bool>>,
                Expression<Func<UserModel, bool>>>(User);
        }


        public UserFindByExpressionExpResponse FindByExpressionExp(UserFindByExpressionExpRequest request)
        {


            var expression = ToViewModel(request.expression);
            return new UserFindByExpressionExpResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,

            };


        }

        public Expression<Func<UserModel, bool>> ToViewModel
            (Expression<Func<UserExpVM, bool>> User)
        {

            return Mapper.Map<Expression<Func<UserExpVM, bool>>,
                Expression<Func<UserModel, bool>>>(User);
        }

        public UserLoginResponse Login(UserLoginRequest request)
        {
            UserModel user = UserManager.Find(request.entity.UserName,
                request.entity.Password);
            if (user == null)
            {
                return new UserLoginResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.LoginField,
                    ResponseType = ResponseType.Error,
                    entity = request.entity
                };
            }
            else
            {
                foreach (var g in user.GroupUsers)
                {
                    if (g.Group.ValidityDate <= DateTime.Now)
                    {
                        return new UserLoginResponse
                        {
                            IsSuccess = false,
                            Message = MessageResource.LoginFieldAccess,
                            ResponseType = ResponseType.Error,
                            entity = request.entity
                        };
                    }
                }

                ClaimsIdentity ident = UserManager.CreateIdentity(user,
                    DefaultAuthenticationTypes.ApplicationCookie);


                return new UserLoginResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = new LoginVM()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Password = request.entity.Password
                    },
                    ident = ident
                };
            }

        }


        public UserUpdateResponse ChangePassword(UserUpdateRequest request)
        {
            try
            {


                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;
                UserModel user = UserManager.FindById(request.entity.Id);
                if (user != null)
                {
                    user.Email = request.entity.Email;

                    if (request.entity.Password != string.Empty)
                    {

                        IdentityResult result1 = UserManager.ChangePassword(user.Id, request.OldPassword,
                           request.entity.Password);
                        if (result1.Succeeded)


                            return new UserUpdateResponse
                            {
                                IsSuccess = true,
                                Message = MessageResource.UpdateSuccess,
                                ResponseType = ResponseType.Success,
                                entity = request.entity
                            };
                    }
                    return new UserUpdateResponse
                    {
                        IsSuccess = false,
                        Message = MessageResource.UpdateField,
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }
                else
                {
                    return new UserUpdateResponse
                    {
                        IsSuccess = false,
                        Message = MessageResource.UpdateField,
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };

                }
                return new UserUpdateResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new UserUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }
    }
}
