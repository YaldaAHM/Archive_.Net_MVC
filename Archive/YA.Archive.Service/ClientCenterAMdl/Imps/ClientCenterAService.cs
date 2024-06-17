using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.ClientCenterLocalization;
using YA.Archive.Model;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterAMdl.Mapping;
using YA.Archive.Service.ClientCenterAMdl.Messaging;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using YA.Archive.Service.ClientCenterAMdl.Sterategy;

namespace YA.Archive.Service.ClientCenterAMdl.Imps
{
    public class ClientCenterAService
    {
        private ArchiveDataContext _db;
        private UserManagementService _UserService;
        private RoleManagementService _roleService;
        private FolderService _folderService;
        private PermissionClaimsService _permissionClaimsService;
        private PermissionClaimsInitialize _permissionClaimsInitialize;
        public ClientCenterAService()
        {
            _db = new ArchiveDataContext();
            _UserService = new UserManagementService();
            _roleService = new RoleManagementService();
            _folderService = new FolderService();
            _permissionClaimsService=new PermissionClaimsService();
            _permissionClaimsInitialize = new PermissionClaimsInitialize();
        }

        public ClientCenterAService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public ClientCenterAInsertResponse Insert(ClientCenterAInsertRequest request)
        {
            try
            {
                if (_db.ClientCenter.Any(n => n.Name == request.entity.Name))
                {
                    return new ClientCenterAInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
         

                var d = request.entity.ToModel();
               var c= _db.ClientCenter.Add(d);
               var cl= _db.SaveChanges();
               // Random r = new Random();
               
                
                var result= _UserService.Insert(new UserInsertRequest()
                                    {
                    RequsetCurrentUserId = request.RequsetCurrentUserId,
                    entity = new RegistrationVM
                                                 {IsAdmin=true,
                                                     ClientCenterId = c.Id,
                                                     UserName = request.entity.UserName,
                                                     Password = request.entity.Password,
                                                 },
                    
                    
                                    });
                if (result.IsSuccess)
                {


                    var responseRole = _roleService.InsertUserToRole(new UserInRoleFindRequest()
                                                                     {
                                                                         Id = result.entity.Id,
                                                                         RoleName = RolesT.AdminClient 
                                                                     });
                    if (responseRole.IsSuccess)
                    {
                        foreach (var i in _permissionClaimsInitialize.PermissionClaimsInitializer())
                        {
                            _permissionClaimsService.Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    entity = new GroupClaimVM()
                                             {
                                                 ClaimType = i.ClaimId,
                                                 ClaimValue = i.CreateClaimId,
                                                 GroupId = null
                                             },
                                    User = new UserVM() {Id = result.entity.Id}
                                });
                            _permissionClaimsService.Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    entity = new GroupClaimVM()
                                             {
                                                 ClaimType = i.ClaimId,
                                                 ClaimValue = i.EditClaimId,
                                                 GroupId = null
                                             },
                                    User = new UserVM() {Id = result.entity.Id}
                                });

                            _permissionClaimsService.Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    entity = new GroupClaimVM()
                                             {
                                                 ClaimType = i.ClaimId,
                                                 ClaimValue = i.DeleteClaimId,
                                                 GroupId = null
                                             },
                                    User = new UserVM() {Id = result.entity.Id}
                                });

                            _permissionClaimsService.Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    entity = new GroupClaimVM()
                                             {
                                                 ClaimType = i.ClaimId,
                                                 ClaimValue = i.ViewClaimId,
                                                 GroupId = null
                                             },
                                    User = new UserVM() {Id = result.entity.Id}
                                });
                        }


                    }
                }
                else
                {
                     _db.ClientCenter.Remove(c);
                    _db.SaveChanges();
                    return new ClientCenterAInsertResponse
                    {
                        Message = MessageResource.Field,
                        ResponseType = ResponseType.Error,
                        entity = request.entity

                    };
                }

                var e = _db.ClientCenter.OrderByDescending(a => a.Id).FirstOrDefault();
                request.entity.UserId = result.entity.Id;
                request.entity.Id = e.Id;
                return new ClientCenterAInsertResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.InsertSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterAInsertResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }


        public ClientCenterAUpdateResponse Update(ClientCenterAUpdateRequest request)
        {
            try
            {
                if (request.entity.Name == ClientCenterR.MainCenterName)
                {
                    return new ClientCenterAUpdateResponse
                    {
                        Message = String.Format(MessageResource.Limited),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.ClientCenter.Any(n => n.Name == request.entity.Name && n.Id!=request.entity.Id))
                {
                    return new ClientCenterAUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
               
              var res=  _UserService.Update(new UserUpdateRequest()
                                    {
                                        entity=new EditVM()
                                               {
                                            Id=request.entity.UserId,
                                                   ClientCenterId=request.entity.Id,
                                                   UserName=request.entity.UserName,
                                                   Password=request.entity.Password
                                               },
                                        RequsetCurrentUserId=request.RequsetCurrentUserId
                                    });
                if (res.IsSuccess) {
                    var c = _db.ClientCenter.FirstOrDefault(n => n.Id == request.entity.Id);
                    Mapper.Map(request.entity, c);
                    _db.SaveChanges();
                    return new ClientCenterAUpdateResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.UpdateSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };
}
                
                    return new ClientCenterAUpdateResponse
                    {
                        IsSuccess = false,
                        Message = MessageResource.UpdateField,
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }
            catch (Exception ex)
            {

                return new ClientCenterAUpdateResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }

        public ClientCenterADeleteResponse DeleteById(ClientCenterADeleteByIdRequest request)
        {
            try
            {
                
                var c = _db.ClientCenter.Find(request.Id);
                if (c == null)
                {
                    return new ClientCenterADeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.Folder.Any(n => n.TypesOfFolders.Any(b => b.TypeofFolderId == request.Id)))
                {
                    return new ClientCenterADeleteResponse
                    {
                        Message = String.Format(ClientCenterResource.UsedClientCenter, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
               
                var responseFolder = _folderService.FindByExpression(
                    new FolderFindByExpressionRequest
                    {
                        RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                        expression =  x => x.ClientCenterId == request.Id,
                    });
                if (responseFolder.Entities.Count() != 0)
                {
                    foreach (var r in responseFolder.Entities)
                    {
                        _folderService.DeleteById(new FolderDeleteByIdRequest()
                                                  {
                                                      Id=r.Id
                                                  });
                    }
                    
                }
                var responseUsers= _UserService.FindByExpression(new UserFindByExpressionRequest
                                              {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x => x.ClientCenterId == request.Id,
                                              
                                              });
                if (responseUsers.Entities.Count() != 0)
                {
                    foreach(var u in responseUsers.Entities)
                    {
                        _UserService.DeleteById(new UserDeleteByIdRequest()
                                                {
                                                    Id = u.Id
                                                });
                    }}
                _db.ClientCenter.Remove(c);
                _db.SaveChanges();
                return new ClientCenterADeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterADeleteResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,

                       };
            }
        }
        public ClientCenterADeleteResponse Delete(ClientCenterADeleteRequest request)
        {
            try
            {
                if (request.entity.Name == ClientCenterR.MainCenterName)
                {
                    return new ClientCenterADeleteResponse
                    {
                        Message = String.Format(MessageResource.Limited),
                        ResponseType = ResponseType.Error,

                    };
                }


                var c = _db.ClientCenter.Find(request.entity.Id);
                if (c == null)
                {
                    return new ClientCenterADeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
             
               
                if (_db.Folder.Any(b => b.ClientCenterId == request.entity.Id) 
                    || _db.Group.Any(b => b.ClientCenterId == request.entity.Id)
                     ||( _db.Users.Any(b => b.ClientCenterId == request.entity.Id &&  b.Id!=request.entity.UserId))
                    )
                {
                    return new ClientCenterADeleteResponse
                    {
                        Message = String.Format(ClientCenterResource.UsedClientCenter, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
                var res = _UserService.DeleteById(new UserDeleteByIdRequest()
                                                  {
                                                      Id=request.entity.UserId,
                                                      RequsetCurrentUserId = request.RequsetCurrentUserId
                                                  });
                if (res.IsSuccess){
                    _db.ClientCenter.Remove(c);
                    var r = _db.SaveChanges();
                    return new ClientCenterADeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                }; }

                return new ClientCenterADeleteResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.DeleteField,
                    ResponseType = ResponseType.Error,
                };
            }
            catch (Exception ex)
            {

                return new ClientCenterADeleteResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public ClientCenterAFindAllResponse FindAll(ClientCenterAFindAllRequest request)
        {
            var roleId = _roleService.FindByName(new RoleFindByIdRequest() {Id= RolesT.AdminClient}).entity.Id;
            var cl = ChooseSterategy.ClientCenterFindAll(request).Entities;
            foreach (var c in cl)
            {
                var user = _db.Users.Where(x => x.ClientCenterId == c.Id && (x.Roles.Any() &&
                                                                             x.Roles.Select(m => m.RoleId)
                                                                                 .Contains(roleId)));
                if (user.Any()) {
                c.UserName = user.FirstOrDefault().UserName;
                c.UserId = user.FirstOrDefault().Id;
                }
            }
            return new ClientCenterAFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                      Entities = cl 
            };

        }


        public ClientCenterAFindByIdResponse FindById(ClientCenterAFindByIdRequest request)
        {
            try
            {
                return new ClientCenterAFindByIdResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.FindSuccess,
                           ResponseType = ResponseType.Success,
                           entity = _db.ClientCenter.Find(request.Id).ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterAFindByIdResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }
        public ClientCenterAFindByExpressionResponse FindByExpression(ClientCenterAFindByExpressionRequest request)
        {  var expression = ToViewModel(request.expression);
            return new ClientCenterAFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.ClientCenterFindByExpression(request).Entities
              
            };

        }

        public Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterAVM, bool>> ClientCenter)
        {

            return Mapper.Map<Expression<Func<ClientCenterAVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }


      
    }
}

