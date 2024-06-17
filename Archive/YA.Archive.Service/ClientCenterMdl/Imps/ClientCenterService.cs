using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.ClientCenterLocalization;
using YA.Archive.Model;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterMdl.Mapping;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
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
using YA.Archive.Service.ClientCenterMdl.Sterategy;

namespace YA.Archive.Service.ClientCenterMdl.Imps
{
    public class ClientCenterService
    {
        private ArchiveDataContext _db;
        private UserManagementService _UserService;
        private RoleManagementService _roleService;
        private FolderService _folderService;
        private PermissionClaimsService _permissionClaimsService;
        private PermissionClaimsInitialize _permissionClaimsInitialize;
        public ClientCenterService()
        {
            _db = new ArchiveDataContext();
            _UserService = new UserManagementService();
            _roleService = new RoleManagementService();
            _folderService = new FolderService();
            _permissionClaimsService=new PermissionClaimsService();
            _permissionClaimsInitialize = new PermissionClaimsInitialize();
        }

        public ClientCenterService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public ClientCenterInsertResponse Insert(ClientCenterInsertRequest request)
        {
            try
            {
                if (_db.ClientCenter.Any(n => n.Name == request.entity.Name))
                {
                    return new ClientCenterInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
      

                var d = request.entity.ToModel();
               var c= _db.ClientCenter.Add(d);
               var cl= _db.SaveChanges();
                Random r = new Random();
               
                
                var result= _UserService.Insert(new UserInsertRequest()
                                    {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    entity = new RegistrationVM
                                                 {
                                                     ClientCenterId = c.Id,
                                                     UserName = "Admin" + r.Next(1000, 7000),
                                                     Password = "Admin123",
                                                 }
                                    });
                if (result.IsSuccess)
                {
                    var responseRole = _roleService.InsertUserToRole(new UserInRoleFindRequest()
                                                                     {
                                                                         Id = result.entity.Id,
                                                                         RoleName = "AdminClient"
                                                                     });
                    if (responseRole.IsSuccess)
                    {
                        foreach (var i in _permissionClaimsInitialize.PermissionClaimsInitializer())
                        {  _permissionClaimsService.Insert(
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
                                       User = new UserVM() { Id = result.entity.Id }
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
                                       User = new UserVM() { Id = result.entity.Id }
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
                                       User = new UserVM() { Id = result.entity.Id }
                                   });
                        }
                    
                   
                    }
                }

                var e = _db.ClientCenter.OrderByDescending(a => a.Id).FirstOrDefault();

                request.entity.Id = e.Id;
                return new ClientCenterInsertResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.InsertSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterInsertResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }


        public ClientCenterUpdateResponse Update(ClientCenterUpdateRequest request)
        {
            try
            {
                if (_db.TypeofFolder.Any(n => n.Name == request.entity.Name))
                {
                    return new ClientCenterUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var c = _db.ClientCenter.FirstOrDefault(n => n.Id == request.entity.Id);
                Mapper.Map(request.entity, c);
                 _db.SaveChanges();
                return new ClientCenterUpdateResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.UpdateSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterUpdateResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }

        public ClientCenterDeleteResponse DeleteById(ClientCenterDeleteByIdRequest request)
        {
            try
            { var c = _db.ClientCenter.Find(request.Id);
                if (c == null)
                {
                    return new ClientCenterDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.Folder.Any(n => n.TypesOfFolders.Any(b => b.TypeofFolderId == request.Id)))
                {
                    return new ClientCenterDeleteResponse
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
                return new ClientCenterDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                       };
            }
        }
        public ClientCenterDeleteResponse Delete(ClientCenterDeleteRequest request)
        {
            try
            {
                var c = _db.ClientCenter.Find(request.entity.Id);
                if (c == null)
                {
                    return new ClientCenterDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ClientCenterResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.ClientCenter.Any(b => b.Id == request.entity.Id))
                {
                    return new ClientCenterDeleteResponse
                    {
                        Message = String.Format(ClientCenterResource.UsedClientCenter, ClientCenterResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
                _db.ClientCenter.Remove(c);
                _db.SaveChanges();
                return new ClientCenterDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new ClientCenterDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public ClientCenterFindAllResponse FindAll(ClientCenterFindAllRequest request)
        {
               return new ClientCenterFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = ChooseSterategy.ClientCenterFindAll(request).Entities//ents
            };

          
        }


        public ClientCenterFindByIdResponse FindById(ClientCenterFindByIdRequest request)
        {
            try
            {
                return new ClientCenterFindByIdResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.FindSuccess,
                           ResponseType = ResponseType.Success,
                           entity = _db.ClientCenter.Find(request.Id).ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new ClientCenterFindByIdResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }
        public ClientCenterFindByExpressionResponse FindByExpression(ClientCenterFindByExpressionRequest request)
        {
           

            var expression = ToViewModel(request.expression);
            return new ClientCenterFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.ClientCenterFindByExpression(request).Entities
               

            };

        }

        public Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterVM, bool>> ClientCenter)
        {

            return Mapper.Map<Expression<Func<ClientCenterVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }


      
    }
}

