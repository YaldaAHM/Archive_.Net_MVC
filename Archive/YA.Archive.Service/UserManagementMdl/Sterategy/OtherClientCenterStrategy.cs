
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
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.UserManagementMdl.Mapping;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Service.UserManagementMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<UserVM> ApplyFindByExpressionTo(UserFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<UserModel, bool>> r = x => x.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
           
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<UserModel, bool>>(body, p);

            var m = UserManager.Users.Where(ex).ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                 Entities = ents

            };
        }

       
        public BaseListResponse<UserVM> ApplyFindAllTo(UserFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new UserFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId=request.RequsetCurrentClientCenterId,
                expression = e => e.ClientCenterId == request.RequsetCurrentClientCenterId
            });
            return new UserFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }


      
        public BaseListResponse<UserVM> ApplyFindByExpressionPagingTo(UserFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<UserModel, bool>> r = x => x.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<UserModel, bool>>(body, p);
            var total = UserManager.Users.Where(ex).Count();
            var m = UserManager.Users.Where(ex).OrderBy(pp => pp.UserName).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList(); ;
            var ents = m.ToViewModelList().ToList();

            return new UserFindByExpressionResponse
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
        public BaseListResponse<UserVM> ApplyFindAllPagingTo(UserFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new UserFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.ClientCenterId == request.RequsetCurrentClientCenterId,
                Count = request.Count,
                Index = request.Index,
            });
            return new UserFindAllResponse
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

        
        public Expression<Func<UserModel, bool>> ToViewModel
            (Expression<Func<UserExpVM, bool>> User)
        {

            return Mapper.Map<Expression<Func<UserExpVM, bool>>,
                Expression<Func<UserModel, bool>>>(User);
        }
        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore(_db));
            }
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
