using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Mapping;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Sterategy;
using Kendo.Mvc.Extensions;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps
{
    public class UserLogTimeService
    {
        private ArchiveDataContext _db;

        public UserLogTimeService()
        {
            _db = new ArchiveDataContext();
        }

        public UserLogTimeService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public UserLogTimeInsertResponse Insert(UserLogTimeInsertRequest request)
        {
            try
            {
                _db.UserLogTime.Add(new UserLogTimeModel()
                {
                    UserId = request.entity.UserId,
                    LoginTime = DateTime.Now,
                    LogoutTime = DateTime.Now,
                    ProviderKey = request.entity.ProviderKey

                });
                _db.SaveChanges();

                return new UserLogTimeInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new UserLogTimeInsertResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public UserLogTimeUpdateResponse Update(UserLogTimeUpdateRequest request)
        {
            try
            {

                var c = _db.UserLogTime.FirstOrDefault(n => n.Id == request.entity.Id);
                c.LogoutTime = DateTime.Now;
                _db.SaveChanges();
                return new UserLogTimeUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new UserLogTimeUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public UserLogTimeDeleteResponse DeleteById(UserLogTimeDeleteByIdRequest request)
        {
            try
            {

                var c = _db.UserLogTime.Find(request.Id);

                _db.UserLogTime.Remove(c);
                _db.SaveChanges();
                return new UserLogTimeDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new UserLogTimeDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public UserLogTimeDeleteResponse Delete(UserLogTimeDeleteRequest request)
        {
            try
            {

                var c = _db.UserLogTime.Find(request.entity.Id);


                _db.UserLogTime.Remove(c);
                _db.SaveChanges();
                return new UserLogTimeDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new UserLogTimeDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public UserLogTimeFindAllResponse FindAll(UserLogTimeFindAllRequest request)
        {


            return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.UserLogTimeFindAll(request).Entities//;ents
            };

        }
        public UserLogTimeFindAllResponse FindAllPaging(UserLogTimeFindAllRequest request)
        {

            var res = ChooseSterategy.UserLogTimeFindAllPaging(request);
            return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.UserLogTimeFindAll(request).Entities,//;ents
                TotalRecords = res.TotalRecords,
                Count = res.Count,
                Index = res.Index
            };

        }

        public UserLogTimeFindByIdResponse FindById(UserLogTimeFindByIdRequest request)
        {
            try
            {
                return new UserLogTimeFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = _db.UserLogTime.Find(request.Id).ToViewModel()
                };

            }
            catch (Exception ex)
            {

                return new UserLogTimeFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public UserLogTimeFindByExpressionResponse FindByExpression(UserLogTimeFindByExpressionRequest request)
        {

            var expression = ToViewModel(request.expression);
            return new UserLogTimeFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = _db.UserLogTime.Where(expression).ToList().ToViewModelList()

            };

        }

        public UserLogTimeFindByExpressionResponse FindByExpression(UserLogTimeFindByExpressionExpRequest request)
        {

            var res = ChooseSterategy.UserLogTimeFindByExpression(request);
            return new UserLogTimeFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = res.Entities,
            };


        }
        public UserLogTimeFindByExpressionResponse FindByExpressionPaging(UserLogTimeFindByExpressionExpRequest request)
        {

            var res = ChooseSterategy.UserLogTimeFindByExpressionPaging(request);
            return new UserLogTimeFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = res.Entities,
                TotalRecords = res.TotalRecords,
                Count = res.Count,
                Index = res.Index

            };


        }
        public Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeVM, bool>> UserLogTime)
        {

            return Mapper.Map<Expression<Func<UserLogTimeVM, bool>>,
                Expression<Func<UserLogTimeModel, bool>>>(UserLogTime);
        }

        public UserLogTimeFindByExpressionExpResponse FindByExpressionExp(UserLogTimeFindByExpressionExpRequest request)
        {

            var expression = ToViewModel(request.expression);
            return new UserLogTimeFindByExpressionExpResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = _db.UserLogTime.Where(expression).ToList().ToViewModelListExp()

            };


        }


        public UserLogTimeFindByExpressionExpResponse FindByVMPaging(UserLogTimeFindByVMRequest request)
        {
            var groupId = request.entity.User.GroupUsers.Select(c => c.GroupId).ToArray();
            var tp = (groupId.Any()) ? groupId.ToList().Count() : 0;


            var total = _db.UserLogTime.Where(x =>
                (request.entity.User.UserName == "" || x.User.UserName.Contains(request.entity.User.UserName)) &&
                (request.entity.User.ClientCenterId == 0 || x.User.ClientCenterId == request.entity.User.ClientCenterId) &&
                (tp == 0 ||
                 (x.User.GroupUsers.Any()

                  && x.User.GroupUsers.Count() != 0 &&
                  x.User.GroupUsers.Any(s => groupId.Contains(s.GroupId))))
                && (request.entity.LoginTime == null || x.LoginTime >= request.entity.LoginTime) &&
                (request.entity.LogoutTime == null || x.LogoutTime <= request.entity.LogoutTime)).Count();



            List<UserLogTimeModel> uuu = _db.UserLogTime.Where(x =>
                (request.entity.User.UserName == "" || x.User.UserName.Contains(request.entity.User.UserName)) &&
                (request.entity.User.ClientCenterId == 0 || x.User.ClientCenterId == request.entity.User.ClientCenterId) &&
                (tp == 0 ||
                 (x.User.GroupUsers.Any()

                  && x.User.GroupUsers.Count() != 0 &&
                  x.User.GroupUsers.Any(s => groupId.Contains(s.GroupId))))
                && (request.entity.LoginTime == null || x.LoginTime >= request.entity.LoginTime) &&
                (request.entity.LogoutTime == null || x.LogoutTime <= request.entity.LogoutTime))
                .Skip(request.Count * (request.Index - 1)).Take(request.Count).ToList();




            return new UserLogTimeFindByExpressionExpResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = uuu.ToViewModelListExp(),
                TotalRecords = total,
                Index = request.Index,
                Count = request.Count



            };


        }


        public Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeExpVM, bool>> UserLogTime)
        {

            return Mapper.Map<Expression<Func<UserLogTimeExpVM, bool>>,
               Expression<Func<UserLogTimeModel, bool>>>(UserLogTime);
        }

    }
}

