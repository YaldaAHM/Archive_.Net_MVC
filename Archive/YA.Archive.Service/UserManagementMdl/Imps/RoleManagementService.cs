using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs.Role;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.Imps
{
    public class RoleManagementService
    {
        private ArchiveDataContext _db;

        public RoleManagementService()
        {
            _db = new ArchiveDataContext();
        }

       
        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore(_db));
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return new AppRoleManager(new RoleStore<RoleModel>(_db));
                   }
        }

        public async Task<RoleInsertResponse> Insert(RoleInsertRequest request)
        {
            try
            {
                
                IdentityResult result
= await RoleManager.CreateAsync(new RoleModel(request.RoleName));
                return new RoleInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                  
                };

            }
            catch (Exception ex)
            {

                return new RoleInsertResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    //entity = request.entity

                };
            }
        }


        public async Task<RoleUpdateResponse> Update(RoleUpdateRequest request)
        {
            try
            {

              
                var role = await RoleManager.FindByIdAsync(request.Id);
                if (role != null)
                {
                    role.Name = request.RoleName;
                   
                        IdentityResult result = await RoleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            return new RoleUpdateResponse
                            {
                                IsSuccess = true,
                                Message = MessageResource.UpdateSuccess,
                                ResponseType = ResponseType.Success,
                                RoleName = request.RoleName
                            };
                        }
                        else
                        {
                            return new RoleUpdateResponse
                            {
                                IsSuccess = true,
                                Message = result.Errors.FirstOrDefault(),
                                ResponseType = ResponseType.Error,
                                RoleName = request.RoleName
                            };
                        }
                    }
                return new RoleUpdateResponse
                {
                    IsSuccess = true,
                    //Message = result.Errors,
                    ResponseType = ResponseType.Error,
                    RoleName = request.RoleName
                };

            }
            catch (Exception ex)
            {

                return new RoleUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public async Task<RoleDeleteResponse> DeleteById(RoleDeleteByIdRequest request)
        {
            try
            {

                RoleModel role = await RoleManager.FindByIdAsync(request.Id);
                if (role != null)
                {
                    role.Users.Clear();
              
                    IdentityResult result = await RoleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return new RoleDeleteResponse
                        {
                            IsSuccess = true,
                            Message = MessageResource.DeleteSuccess,
                            ResponseType = ResponseType.Success,
                        };
                    else
                    {
                        return new RoleDeleteResponse
                        {
                            Message = MessageResource.DeleteField,
                            ResponseType = ResponseType.Error,
                        };
                    }
                }
                return new RoleDeleteResponse
                {
                    Message = MessageResource.DeleteField,
                    ResponseType = ResponseType.Error,
                };
            }
            catch (Exception ex)
            {

                return new RoleDeleteResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public RoleFindAllResponse FindAll(RoleFindAllRequest request)
        {
            
            var r = RoleManager.Roles.ToList();
            var ents = r.ToRoleViewModelList().ToList();
            return new RoleFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
           
        }


        public async Task<RoleFindByIdResponse> FindById(RoleFindByIdRequest request)
        {
            try
            {
                var entity = await RoleManager.FindByIdAsync(request.Id);
                return new RoleFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                     entity = entity.ToRoleViewModel()
                };

            }
            catch (Exception ex)
            {

                return new RoleFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public RoleFindByIdResponse FindByName(RoleFindByIdRequest request)
        {
            try
            {
                var entity = RoleManager.FindByName(request.Id);
                return new RoleFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity.ToRoleViewModel()
                };

            }
            catch (Exception ex)
            {

                return new RoleFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public async Task<UserFindByExpressionResponse> FindUsersInRoleName(RoleFindUserRequest request)
        {
           
            RoleModel role = RoleManager.FindByName(request.Id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<UserModel> members
            = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            
            return new UserFindByExpressionResponse()
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = members.ToList().ToViewModelList()
            };
          
        }

        public async Task<RoleFindUserResponse> FindUsersInRole(RoleFindUserRequest request)
        {
           
            RoleModel role = await RoleManager.FindByIdAsync(request.Id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<UserModel> members
            = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<UserModel> nonMembers = UserManager.Users.Except(members);
            var roleEdit = new RoleEditVM()
            {
                Role = role,
                Members = members.ToList().ToViewModelList(),
                NonMembers = nonMembers.ToList().ToViewModelList()
            };
            return new RoleFindUserResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                entity = roleEdit
            };

        }

        public async Task<RoleUpdateUserResponse> UpdateUsersInRole(RoleUpdateUserRequest request)
        {
            
            IdentityResult result;
            foreach (string userId in request.entity.IdsToAdd ?? new string[] { })
            {
                result = await UserManager.AddToRoleAsync(userId, request.entity.RoleName);
                if (!result.Succeeded)
                {
                    return new RoleUpdateUserResponse
                    {
                        Message = result.Errors.FirstOrDefault(),
                        ResponseType = ResponseType.Error,

                    };
                }
            }
            foreach (string userId in request.entity.IdsToDelete ?? new string[] { })
            {
                result = await UserManager.RemoveFromRoleAsync(userId,
                request.entity.RoleName);
                if (!result.Succeeded)
                {
                    return new RoleUpdateUserResponse
                    {
                        Message = result.Errors.FirstOrDefault(),
                        ResponseType = ResponseType.Error,

                    };
                }
            }
            return new RoleUpdateUserResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
             
            };
           
        }

        public  UserInRoleFindResponse FindUserIsInRole(UserInRoleFindRequest request)
        {



            var result = UserManager.IsInRole(request.Id, request.RoleName);
            if (result.Equals(true))
            {
                return new UserInRoleFindResponse
                {
                    ResponseType = ResponseType.Success,
                    Message = MessageResource.FindSuccess,
                    IsSuccess =true,
                    IsInRole=true,
                    entity=UserManager.FindById(request.Id).ToViewModel()
                };
            }
        
            return new UserInRoleFindResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                IsInRole = false,
                entity = UserManager.FindById(request.Id).ToViewModel()
            };
}
        public  UserInRoleFindResponse InsertUserToRole(UserInRoleFindRequest request)
        {
            if (!UserManager.IsInRole(request.Id, request.RoleName))
            {
               var r= UserManager.AddToRole(request.Id, request.RoleName);
                _db.SaveChanges();
            
            
            return new UserInRoleFindResponse
            {
                IsSuccess = r.Succeeded,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
        
            };
        }
            return new UserInRoleFindResponse
            {
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
            };
        }

    }
}
