
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.Folder;
using YA.Archive.Service.FolderMdl.Access;
using YA.Archive.Service.FolderMdl.Mapping;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.FolderMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<FolderVM> ApplyFindByExpressionTo(FolderFindByExpressionExpRequest requestExp)
        {
            var expression =ToViewModel( requestExp.expression);
            Expression<Func<FolderModel, bool>> r = x => x.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<FolderModel, bool>>(body, p);


            var c = _db.Folder.Where(ex).ToList();
             var ents = new List<FolderExpVM>();

            foreach (var f in c)
            {
                var ent = new FolderExpVM();
                ent = f.ToViewModelExp();
                ents.Add(ent);
            }
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                 Entities = ApplyAccess.ApplyFolderAccessList(ents
                .ToViewModelEVMList(), requestExp.RequsetCurrentUserId).ToList()

        };
        }

      

        public BaseListResponse<FolderVM> ApplyFindAllTo(FolderFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new FolderFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId=request.RequsetCurrentClientCenterId,
                expression = e => e.ClientCenterId == request.RequsetCurrentClientCenterId
            });
            f.Entities = ApplyAccess.ApplyFolderAccessList(f.Entities, request.RequsetCurrentUserId).ToList();
            return new FolderFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }
        public BaseListResponse<FolderVM> ApplyFindByExpressionPagingTo(FolderFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<FolderModel, bool>> r = x => x.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<FolderModel, bool>>(body, p);


             var total = _db.Folder.Where(ex).Count();
            var c = _db.Folder.Where(ex).OrderByDescending(pp => pp.Id).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList();
            var ents = new List<FolderExpVM>();

            foreach (var f in c)
            {
                var ent = new FolderExpVM();
                ent = f.ToViewModelExp();
                ents.Add(ent);
            }
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                 Entities = ApplyAccess.ApplyFolderAccessList(ents
                .ToViewModelEVMList(), requestExp.RequsetCurrentUserId).ToList(),
                Index = requestExp.Index,
                Count = requestExp.Count,
                TotalRecords = total,

            };
        }
        public BaseListResponse<FolderVM> ApplyFindAllPagingTo(FolderFindAllRequest request)
        {
            var f = ApplyFindByExpressionPagingTo(new FolderFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.ClientCenterId == request.RequsetCurrentClientCenterId,
                Count=request.Count,
                Index=request.Index,
            });
            f.Entities = ApplyAccess.ApplyFolderAccessList(f.Entities, request.RequsetCurrentUserId).ToList();
             return new FolderFindAllResponse
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

       

        public Expression<Func<FolderModel, bool>> ToViewModel
            (Expression<Func<FolderExpVM, bool>> Folder)
        {

            // Mapper.Map<Expression<Func<FoodVM, bool>>, Expression<Func<FoodModel, bool>>>(OrderedFolder);
            return Mapper.Map<Expression<Func<FolderExpVM, bool>>,
                Expression<Func<FolderModel, bool>>>(Folder);
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
