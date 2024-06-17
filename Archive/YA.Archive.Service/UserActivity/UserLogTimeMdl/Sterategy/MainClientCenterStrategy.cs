using YA.Archive.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity.EntityFramework;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Mapping;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy

    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }

        public BaseListResponse<UserLogTimeVM> ApplyFindByExpressionTo(UserLogTimeFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var m = _db.UserLogTime.Where(expression).OrderBy(p => p.Id).ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserLogTimeFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents


            };
        }


        public BaseListResponse<UserLogTimeVM> ApplyFindAllTo(UserLogTimeFindAllRequest request)
        {
            var m = _db.UserLogTime.OrderBy(p => p.Id).ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }


        public BaseListResponse<UserLogTimeVM> ApplyFindByExpressionPagingTo(
            UserLogTimeFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var total = _db.UserLogTime.Where(expression).Count();
            var m =
                _db.UserLogTime.Where(expression)
                    .OrderBy(p => p.Id)
                    .Skip(requestExp.Count * (requestExp.Index - 1))
                    .Take(requestExp.Count)
                    .ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserLogTimeFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
                Index = requestExp.Index,
                Count = requestExp.Count,
                TotalRecords = total,

            };
        }

        public BaseListResponse<UserLogTimeVM> ApplyFindAllPagingTo(UserLogTimeFindAllRequest request)
        {
            var total = _db.UserLogTime.Count();
            var m =
                _db.UserLogTime.OrderByDescending(p => p.Id)
                    .Skip(request.Count * (request.Index - 1))
                    .Take(request.Count)
                    .ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
                Index = request.Index,
                Count = request.Count,
                TotalRecords = total,
            };
        }



        public Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeExpVM, bool>> User)
        {
            return Mapper.Map<Expression<Func<UserLogTimeExpVM, bool>>,
                          Expression<Func<UserLogTimeModel, bool>>>(User);
        }

    }
}
