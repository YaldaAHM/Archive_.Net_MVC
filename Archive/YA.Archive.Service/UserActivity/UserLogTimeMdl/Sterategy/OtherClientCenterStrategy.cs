
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
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Mapping;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<UserLogTimeVM> ApplyFindByExpressionTo(UserLogTimeFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<UserLogTimeModel, bool>> r = x => x.User.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
             ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<UserLogTimeModel, bool>>(body, p);

            var m = _db.UserLogTime.Where(ex).ToList();
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
            var f = ApplyFindByExpressionTo(new UserLogTimeFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId=request.RequsetCurrentClientCenterId,
                expression = e => e.User.ClientCenterId == request.RequsetCurrentClientCenterId
            });
             return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }


      
        public BaseListResponse<UserLogTimeVM> ApplyFindByExpressionPagingTo(UserLogTimeFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<UserLogTimeModel, bool>> r = x => x.User.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
           ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<UserLogTimeModel, bool>>(body, p);
            var total = _db.UserLogTime.Where(ex).Count();
            var m = _db.UserLogTime.Where(ex).OrderBy(pp => pp.Id).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList(); ;
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
            var f = ApplyFindByExpressionTo(new UserLogTimeFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.User.ClientCenterId == request.RequsetCurrentClientCenterId,
                Count = request.Count,
                Index = request.Index,
            });
            return new UserLogTimeFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities,
                Index = f.Index,
                Count = f.Count,
                TotalRecords = f.TotalRecords,
            };
        }

        

        public Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeExpVM, bool>> User)
        {

            return Mapper.Map<Expression<Func<UserLogTimeExpVM, bool>>,
                Expression<Func<UserLogTimeModel, bool>>>(User);
        }
      
    }
    internal class SubstExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression newValue;
            if (subst.TryGetValue(node, out newValue))
            {
                return newValue;
            }
            return node;
        }
    }
}
