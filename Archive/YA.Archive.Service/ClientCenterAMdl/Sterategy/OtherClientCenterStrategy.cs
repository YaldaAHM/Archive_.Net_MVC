
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterAMdl.Mapping;
using YA.Archive.Service.ClientCenterAMdl.Messaging;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.ClientCenterAMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<ClientCenterAVM> ApplyFindByExpressionTo(ClientCenterAFindByExpressionRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<ClientCenterModel, bool>> r = x => x.Id == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<ClientCenterModel, bool>>(body, p);

            var c = _db.ClientCenter.Where(ex).ToList();
            var ents = new List<ClientCenterAVM>();

            foreach (var f in c)
            {
                var ent = new ClientCenterAVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            return new ClientCenterAFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents

            };
        }

        public BaseListResponse<ClientCenterAVM> ApplyFindAllTo(ClientCenterAFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new ClientCenterAFindByExpressionRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId=request.RequsetCurrentClientCenterId,
                expression = e => e.Id == request.RequsetCurrentClientCenterId
            });
         
            return new ClientCenterAFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }


        public Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterAVM, bool>> ClientCenter)
        {

            return Mapper.Map<Expression<Func<ClientCenterAVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
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
