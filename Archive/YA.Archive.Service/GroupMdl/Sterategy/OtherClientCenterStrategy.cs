
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
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupMdl.Mapping;
using YA.Archive.Model.Group;

namespace YA.Archive.Service.GroupMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<GroupVM> ApplyFindByExpressionTo(GroupFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<GroupModel, bool>> r = x => x.ClientCenterId == requestExp.RequsetCurrentClientCenterId;

            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<GroupModel, bool>>(body, p);


            var c = _db.Group.Where(ex).ToList();

            var ents = new List<GroupExpVM>();

            foreach (var f in c)
            {
                var ent = new GroupExpVM();
                ent = f.ToViewModelExp();

                ents.Add(ent);
            }
            return new GroupFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents.ToViewModelEVMList()

            };
        }

        public BaseListResponse<GroupVM> ApplyFindAllTo(GroupFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new GroupFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.ClientCenterId == request.RequsetCurrentClientCenterId
            });
            return new GroupFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }


        public Expression<Func<GroupModel, bool>> ToViewModel
            (Expression<Func<GroupExpVM, bool>> Folder)
        {

            return Mapper.Map<Expression<Func<GroupExpVM, bool>>,
                Expression<Func<GroupModel, bool>>>(Folder);
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
