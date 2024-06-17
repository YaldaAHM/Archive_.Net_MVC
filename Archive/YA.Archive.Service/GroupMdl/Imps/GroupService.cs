using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.Group;
using YA.Archive.Model.GroupUser;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.GroupMdl.Mapping;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.GroupLocalization;
using YA.Archive.Service.ClientCenterStrategy;
using YA.Archive.Service.GroupMdl.Sterategy;

namespace YA.Archive.Service.GroupMdl.Imps
{
    public class GroupService
    {
        private ArchiveDataContext _db;
        private UserManagementService _userManagementService;
        PermissionClaimsService _PermissionClaimsService;

        private RoleManagementService _roleService;
        public GroupService()
        {
            _db = new ArchiveDataContext();
            _userManagementService = new UserManagementService();
            _PermissionClaimsService = new PermissionClaimsService();
            _roleService = new RoleManagementService();
        }

        public GroupService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public GroupInsertResponse Insert(GroupInsertRequest request)
        {
            try
            {
                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;
                if (_db.Group.Any(n => n.Name == request.entity.Name && n.ClientCenterId == request.entity.ClientCenterId))
                {
                    return new GroupInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, GroupResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                request.entity.CreateDate= fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                var d = request.entity.ToModel();
             var r=   _db.Group.Add(d);
                _db.SaveChanges();
                var e = _db.Group.OrderByDescending(a => a.Id).FirstOrDefault();
                
                request.entity.Id = e.Id;
                return new GroupInsertResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.InsertSuccess,
                           ResponseType = ResponseType.Success,
                           entity =r.ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new GroupInsertResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }


        public GroupUpdateResponse Update(GroupUpdateRequest request)
        {
            try
            {
                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;
                if (_db.Group.Any(n => n.Name == request.entity.Name && 
                n.ClientCenterId == request.entity.ClientCenterId && n.Id!= request.entity.Id))
                {
                    return new GroupUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, GroupResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var c = _db.Group.FirstOrDefault(n => n.Id == request.entity.Id);
              
                c.Name = request.entity.Name;
                c.ValidityDate =fpub.convert2miladi( request.entity.ValidityDate);
                _db.SaveChanges();
                return new GroupUpdateResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.UpdateSuccess,
                           ResponseType = ResponseType.Success,
                           entity = c.ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new GroupUpdateResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }

        public GroupDeleteResponse DeleteById(GroupDeleteByIdRequest request)
        {
            try
            {

                var c = _db.Group.Find(request.Id);
                foreach (var u in c.GroupUsers)
                {
                    var responseDeleteClaims = _PermissionClaimsService.DeleteByGroupId(
                    new PermissionClaimsDeleteByGroupIdRequest() { entity = u.User.ToViewModel(),GroupId=request.Id });
                }
                c.GroupUsers.ToList()
              .ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                
                _db.Group.Remove(c);
                _db.SaveChanges();
                return new GroupDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };
               
            }
            catch (Exception ex)
            {

                return new GroupDeleteResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupDeleteResponse Delete(GroupDeleteRequest request)
        {
            try
            {
                
                var c = _db.Group.Find(request.entity.Id);
               
                    foreach(var u in c.GroupUsers)
                    {
                        _PermissionClaimsService.DeleteByUserId(new PermissionClaimsDeleteRequest()
                               {
                                           entity = u.User.ToViewModel()
                                       });
                    }
                _db.Group.ToList().FirstOrDefault(n => n.Id == request.entity.Id).GroupUsers.ToList()
             .ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                _db.Group.ToList().FirstOrDefault(n => n.Id == request.entity.Id).GroupClaims.ToList()
             .ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                _db.Group.Remove(c);
                _db.SaveChanges();
                return new GroupDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

        
            }
            catch (Exception ex)
            {

                return new GroupDeleteResponse
                {
                    Message = MessageResource.Field,
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupFindAllResponse FindAll(GroupFindAllRequest request)
        {
          
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
            var c = clientCenterStrategy.ApplyFindAllTo(request);
            return new GroupFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = c.Entities
                   };

        }


        public GroupFindByIdResponse FindById(GroupFindByIdRequest request)
        {
            try
            {
                return new GroupFindByIdResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.FindSuccess,
                           ResponseType = ResponseType.Success,
                           entity = _db.Group.Find(request.Id).ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new GroupFindByIdResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupFindByExpressionResponse FindByExpression(GroupFindByExpressionRequest request)
        {
            

            var expression = ToViewModel(request.expression);
            return new GroupFindByExpressionResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = _db.Group.Where(expression).ToList().ToViewModelList()

                       
                   };

        }

        public Expression<Func<GroupModel, bool>> ToViewModel
            (Expression<Func<GroupVM, bool>> Group)
        {

              return Mapper.Map<Expression<Func<GroupVM, bool>>,
                Expression<Func<GroupModel, bool>>>(Group);
        }


        public GroupFindByExpressionResponse FindByExpression(GroupFindByExpressionExpRequest request)
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
            return new GroupFindByExpressionResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                    
Entities = c.Entities
                   };

           
        }

        public Expression<Func<GroupModel, bool>> ToViewModel
            (Expression<Func<GroupExpVM, bool>> Group)
        {

            return Mapper.Map<Expression<Func<GroupExpVM, bool>>,
                Expression<Func<GroupModel, bool>>>(Group);
        }


        public GroupUserFindResponse FindGroupsUser(GroupUserFindRequest request)
        {
           
            var group = FindById(new GroupFindByIdRequest() {Id = request.Id});
            string[] memberIDs = group.entity.GroupUsers.Select(x => x.UserId).ToArray();

            var responseUser = _userManagementService.FindByExpression(new UserFindByExpressionRequest()
                                                                       {expression = x => memberIDs.Any(y => y == x.Id)});
            IEnumerable<UserVM> members = responseUser.Entities;
            var responseAllUser = _userManagementService.FindAll(new UserFindAllRequest());
             List<UserVM> nonMembers = new List<UserVM>();
            foreach (var e in responseAllUser.Entities)
            {
                bool isMember = false;
                foreach (var m in members)
                {
                    if (e.Id == m.Id)
                    {
                        isMember = true;
                    }
                }
                if (isMember == false)
                {
                    nonMembers.Add(e);
                }
            }
            var gropEdit = new GroupEditVM()
                           {
                               Group = group.entity,
                               Members = members,
                               NonMembers = nonMembers,
                               GroupId=group.entity.Id
                           };
            return new GroupUserFindResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       entity = gropEdit
                   };

           
        }

        public GroupUserUpdateResponse UpdateUsersInGroup(GroupUserUpdateRequest request)
        {
            
            foreach (string userId in request.entity.IdsToAdd ?? new string[] {})
            {
                   _db.Group.ToList().FirstOrDefault(n => n.Id == request.entity.GroupId).GroupUsers
                    .Add(new GroupUserModel()
                         {
                             GroupId = request.entity.GroupId,
                             UserId = userId,
                         });
                
            }
            foreach (string userId in request.entity.IdsToDelete ?? new string[] {})
            {
               
                _db.Group.ToList().FirstOrDefault(n => n.Id == request.entity.GroupId).GroupUsers
                    .Remove(new GroupUserModel()
                            {
                                GroupId = request.entity.GroupId,
                                UserId = userId,
                            });
                _db.Group.ToList().FirstOrDefault(n => n.Id == request.entity.GroupId).GroupUsers.ToList()
                    .Where(g=>g.GroupId == request.entity.GroupId && g.UserId==userId).ToList()
               .ForEach(pp =>  _db.Entry(pp).State = EntityState.Deleted);
                
            } _db.SaveChanges();
            return new GroupUserUpdateResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                      
                   };

        }
    }

}