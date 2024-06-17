using AutoMapper;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;


namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps
{
    public class PermissionClaimsService
    {
        private ArchiveDataContext _db;
        public PermissionClaimsService()
        {
            _db = new ArchiveDataContext();
        }
        public PermissionClaimsService(ArchiveDataContext db)
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
       

        public PermissionClaimsUpdateResponse Update(PermissionClaimsUpdateRequest request)
        {
            try
            {
                
                if (request.User != null)
                {
                    foreach (var p in request.Entities)
                    {
                        if (p.Create)
                        {
                            Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    User = request.User,
                                    entity = new GroupClaimVM()
                                             {
                                                 GroupId = null,
                                                 ClaimType = p.ClaimId,
                                                 ClaimValue = p.CreateClaimId,
                                             }
                                });
                        }
                        else
                        {
                            Delete(
                                     new PermissionClaimsGroupDeleteRequest()
                                     {
                                         User = request.User,
                                         entity = new GroupClaimVM()
                                         {
                                             ClaimType = p.ClaimId,
                                             ClaimValue = p.CreateClaimId,
                                             GroupId = null
                                         }
                                     });
                        }
                        if (p.Edit)
                        {
                            Insert(
                                 new PermissionClaimsGroupInsertRequest()
                                 {
                                     User = request.User,
                                     entity = new GroupClaimVM()
                                     {
                                         GroupId = null,
                                         ClaimType = p.ClaimId,
                                         ClaimValue = p.EditClaimId,
                                     }
                                 });
                        }
                        else
                        {
                            Delete(
                                      new PermissionClaimsGroupDeleteRequest()
                                      {
                                          User = request.User,
                                          entity = new GroupClaimVM()
                                          {
                                              ClaimType = p.ClaimId,
                                              ClaimValue = p.EditClaimId,
                                              GroupId = null
                                          }
                                      });
                        }
                        if (p.Delete)
                        {
                            Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    User = request.User,
                                    entity = new GroupClaimVM()
                                    {
                                        GroupId = null,
                                        ClaimType = p.ClaimId,
                                        ClaimValue = p.DeleteClaimId,
                                    }
                                });
                        }
                        else
                        {
                            Delete(
                                       new PermissionClaimsGroupDeleteRequest()
                                       {
                                           User = request.User,
                                           entity = new GroupClaimVM()
                                           {
                                               ClaimType = p.ClaimId,
                                               ClaimValue = p.DeleteClaimId,
                                               GroupId = null
                                           }
                                       });
                        }
                        if (p.View)
                        {
                            Insert(
                                new PermissionClaimsGroupInsertRequest()
                                {
                                    User = request.User,
                                    entity = new GroupClaimVM()
                                    {
                                        GroupId = null,
                                        ClaimType = p.ClaimId,
                                        ClaimValue = p.ViewClaimId,
                                    }
                                });
                        }
                        else
                        {
                            Delete(
                                     new PermissionClaimsGroupDeleteRequest()
                                     {
                                         User = request.User,
                                         entity = new GroupClaimVM()
                                         {
                                             ClaimType = p.ClaimId,
                                             ClaimValue = p.ViewClaimId,
                                             GroupId = null
                                         }
                                     });
                        }

                               }
                }
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
     
        public async Task<PermissionClaimsDeleteResponse> DeleteByUserId(PermissionClaimsDeleteRequest request)
        {
            try
            {
                var users = UserManager.Users;
                UserModel user = await UserManager.FindByIdAsync(request.entity.Id);
                UserManager.GetClaims(user.Id).ToList().ForEach(claim => UserManager.RemoveClaim(user.Id, claim));
               
                return new PermissionClaimsDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new PermissionClaimsDeleteResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public PermissionClaimsGroupInsertResponse Insert(PermissionClaimsGroupInsertRequest request)
        {
            try
            {
    var user = UserManager.FindById(request.User.Id);
               
                  var usersClaims = user.Claims.Where(n => 
                
                    n.ClaimType == request.entity.ClaimType
                    && n.ClaimValue == request.entity.ClaimValue
                     && n.GroupId == request.entity.GroupId).ToList();
                if (usersClaims.Count == 0)
                {
                  

                    var us = UserManager.FindById(user.Id);
                us.Claims.Add(new ApplicationUserClaim()
                {
                    ClaimType = request.entity.ClaimType,
                    ClaimValue = request.entity.ClaimValue,
                    GroupId = request.entity.GroupId,

                });





                _db.SaveChanges();

                    return new PermissionClaimsGroupInsertResponse
                           {
                               IsSuccess = true,
                               Message = MessageResource.DeleteSuccess,
                               ResponseType = ResponseType.Success,
                           };

                }
                return new PermissionClaimsGroupInsertResponse
                {
                   // Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
            catch (Exception ex)
            {

                return new PermissionClaimsGroupInsertResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }
        public PermissionClaimsGroupDeleteResponse Delete(PermissionClaimsGroupDeleteRequest request)
        {
            try
            {
                   var user = UserManager.FindById(request.User.Id);
                    var usersClaims = user.Claims.Where(n => n.ClaimType == request.entity.ClaimType
                    && n.ClaimValue == request.entity.ClaimValue
                     && n.GroupId == request.entity.GroupId).ToList();
                if (usersClaims.Count != 0)
                {
                    user.Claims.Where(c => c.ClaimType == request.entity.ClaimType &&
                                         c.ClaimValue == request.entity.ClaimValue &&
                                         c.GroupId == request.entity.GroupId ).ToList().ForEach(
                                             pp => _db.Entry(pp).State = EntityState.Deleted);

               
                    _db.SaveChanges();
                    }
                   
                
                return new PermissionClaimsGroupDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new PermissionClaimsGroupDeleteResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public async Task<PermissionClaimsDeleteByGroupIdResponse> DeleteByGroupId(PermissionClaimsDeleteByGroupIdRequest request)
        {
            try
            {
                var users = UserManager.Users;
                UserModel user = await UserManager.FindByIdAsync(request.entity.Id);

                  _db.SaveChanges();
                return new PermissionClaimsDeleteByGroupIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new PermissionClaimsDeleteByGroupIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public PermissionClaimsFindAllResponse FindAll(PermissionClaimsFindAllRequest request)
        {
           
            var cc = UserManager.FindById(request.User.Id).Claims;
            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            var lpc = pc.PermissionClaimsInitializer();
            foreach(var item in lpc)
            {
              
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                && x.ClaimValue == item.CreateClaimId && x.GroupId==null)!=null)
                {
                    item.Create = true;
                }
                
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.EditClaimId && x.GroupId == null) != null)
                {
                    item.Edit = true;
                }
              
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.DeleteClaimId && x.GroupId == null) != null)
                {
                    item.Delete = true;
                }
                
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.ViewClaimId && x.GroupId == null) != null)
                {
                    item.View = true;
                }
                
            }
           
            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
        };

            
        }

        public PermissionClaimsFindAllResponse FindUserClaims(PermissionClaimsFindAllRequest request)
        {
            var cc = request.User.Claims.ToList();
            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            var lpc = pc.PermissionClaimsInitializer();
            foreach (var item in lpc)
            {
                
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                && x.ClaimValue == item.CreateClaimId) != null)
                {
                    item.Create = true;
                }
                
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.EditClaimId) != null)
                {
                    item.Edit = true;
                }
               
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.DeleteClaimId) != null)
                {
                    item.Delete = true;
                }
                
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.ViewClaimId) != null)
                {
                    item.View = true;
                }
               
            }
          
            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
            };

           
        }

     
        public PermissionClaimsGroupUpdateResponse UpdateGroupUser(PermissionClaimsGroupUpdateRequest request)
        {
            try
            {
                
                return new PermissionClaimsGroupUpdateResponse
                {
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
        public PermissionClaimsGroupFindAllResponse FindAllByGroup(PermissionClaimsGroupFindAllRequest request)
        {
            
            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            var lpc = pc.PermissionClaimsInitializer();
            foreach (var item in lpc)
            {
                item.GroupId = request.Group.Id;
                
            }
            
            return new PermissionClaimsGroupFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
            };

           
        }

        public PermissionClaimsFindByExpressionResponse FindByExpression(
            PermissionClaimsFindByExpressionRequest request)
        {

            var expression = request.expression;
         
            var usersClaims =UserManager.Users.Where(x=>x.Claims.Any(new Func<ApplicationUserClaim, bool>(
                request.expression)));
            return new PermissionClaimsFindByExpressionResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities=usersClaims
                   };
           

        }
        public PermissionClaimsFindByExpressionResponse DeleteByClaimType(string request)
        {

            

            var usersClaims = UserManager.Users.Where(n => n.Claims.Any() &&
         n.Claims.Any(x => x.ClaimType == request)).ToList();

            if (usersClaims.Count != 0)
            {
                var usersClaimss = usersClaims.ToList();
                foreach (var u in usersClaims)
                {
                    var us = UserManager.FindById(u.Id);
                    us.Claims.Where(x => x.ClaimType == request).ToList().ForEach(
                                             pp => _db.Entry(pp).State = EntityState.Deleted);
                }
                _db.SaveChanges();
            }
            return new PermissionClaimsFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
            };
           
        }
        public PermissionClaimsFindByExpressionResponse DeleteByExpression(PermissionClaimsFindByExpressionRequest request)
        {

            var expression = request.expression;
          
                 var usersClaims = UserManager.Users.Where(n => n.Claims.Any() &&
              n.Claims.Any(x => x.ClaimType == "22")).ToList();
       
            if (usersClaims.Count != 0)
            {
                var usersClaimss = usersClaims.ToList();
                foreach (var u in usersClaims)
                {
                      var us = UserManager.FindById(u.Id);
                    us.Claims.Where(x => x.ClaimType == "22").ToList().ForEach(
                                             pp => _db.Entry(pp).State = EntityState.Deleted);
                }
                _db.SaveChanges();
            }  return new PermissionClaimsFindByExpressionResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                   };
          
        }
       
        public Expression<Func<PermissionClaimsModel, bool>> ToViewModel
            (Expression<Func<PermissionClaimsVM, bool>> Folder)
        {

            return Mapper.Map<Expression<Func<PermissionClaimsVM, bool>>,
                Expression<Func<PermissionClaimsModel, bool>>>(Folder);
        }
    }
}
