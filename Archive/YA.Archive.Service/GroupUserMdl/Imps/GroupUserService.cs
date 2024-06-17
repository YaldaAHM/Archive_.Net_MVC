using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.GroupUser;
using YA.Archive.Service.GroupUserMdl.Mapping;
using YA.Archive.Service.GroupUserMdl.Messaging;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.GroupUserMdl.Imps
{
    public class GroupUserService
    {
        private ArchiveDataContext _db;

        public GroupUserService()
        {
            _db = new ArchiveDataContext();
        }

        public GroupUserService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public GroupUserInsertResponse Insert(GroupUserInsertRequest request)
        {
            try
            {
                var d = request.entity.ToModel();
                _db.GroupUser.Add(d);
                _db.SaveChanges();
                var e = _db.GroupUser.OrderByDescending(a => a.Id).FirstOrDefault();

                request.entity.Id = e.Id;
                return new GroupUserInsertResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.InsertSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };

            }
            catch (Exception ex)
            {

                return new GroupUserInsertResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }


        public GroupUserUpdateResponse Update(GroupUserUpdateRequest request)
        {
            try
            {

                var c = _db.GroupUser.FirstOrDefault(n => n.Id == request.entity.Id);
                Mapper.Map(request.entity, c);
                _db.SaveChanges();
                return new GroupUserUpdateResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.UpdateSuccess,
                           ResponseType = ResponseType.Success,
                           entity = request.entity
                       };

            }
            catch (Exception ex)
            {

                return new GroupUserUpdateResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }

        public GroupUserDeleteResponse DeleteById(GroupUserDeleteByIdRequest request)
        {
            try
            {

                var c = _db.GroupUser.Find(request.Id);
                _db.GroupUser.Remove(c);
               var result= _db.SaveChanges();
                return new GroupUserDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

            }
            catch (Exception ex)
            {

                return new GroupUserDeleteResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupUserDeleteResponse Delete(GroupUserDeleteRequest request)
        {
            try
            {

                var c = _db.GroupUser.Find(request.entity.Id);
                _db.GroupUser.Remove(c);
                _db.SaveChanges();
                return new GroupUserDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

            }
            catch (Exception ex)
            {

                return new GroupUserDeleteResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupUserFindAllResponse FindAll(GroupUserFindAllRequest request)
        {
            //try
            //{
            var ents = _db.GroupUser.ToList().ToViewModelList();
            return new GroupUserFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = ents
                   };

        }


        public GroupUserFindByIdResponse FindById(GroupUserFindByIdRequest request)
        {
            try
            {
                return new GroupUserFindByIdResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.FindSuccess,
                           ResponseType = ResponseType.Success,
                           entity = _db.GroupUser.Find(request.Id).ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new GroupUserFindByIdResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }

        public GroupUserFindByExpressionResponse FindByExpression(GroupUserFindByExpressionRequest request)
        {
            

            var expression = ToViewModel(request.expression);
            return new GroupUserFindByExpressionResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                       Entities = _db.GroupUser.Where(expression).ToList().ToViewModelList()

                       //  entity = _db.ClientCenter.Find(request.Id).ToViewModel()

                   };

            
        }

        public Expression<Func<GroupUserModel, bool>> ToViewModel
            (Expression<Func<GroupUserVM, bool>> GroupUser)
        {

            return Mapper.Map<Expression<Func<GroupUserVM, bool>>,
                Expression<Func<GroupUserModel, bool>>>(GroupUser);
        }




        public async Task<GroupUserFindByIdResponse> FindUsersInGroupUser(GroupUserFindByIdRequest request)
        {
          
            var g = _db.GroupUser.Find(request.Id);
            
            return new GroupUserFindByIdResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                      
                   };

          
        }

        public GroupUserUpdateResponse UpdateUsersInGroupUser(GroupUserUpdateRequest request)
        {
            
            return new GroupUserUpdateResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
              
            };

        }
    }
}
