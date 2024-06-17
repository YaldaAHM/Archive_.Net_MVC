
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.File;
using YA.Archive.Service.FileMdl.Mapping;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Service.FolderMdl.Access;

namespace YA.Archive.Service.FileMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public OtherClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<FileVM> ApplyFindByExpressionTo(FileFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<FileModel, bool>> r = x => x.Folder.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<FileModel, bool>>(body, p);
            var c = _db.File.Where(ex).ToList();
            var ents = new List<FileExpVM>();

            foreach (var f in c)
            {
                var ent = new FileExpVM();
                ent = f.ToViewModelExp();
                ents.Add(ent);
            }
            return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ApplyAccess.ApplyFileAccessList(ents
                .ToViewModelListExp(), requestExp.RequsetCurrentUserId).ToList()

            };
        }




        public BaseListResponse<FileVM> ApplyFindAllTo(FileFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new FileFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.Folder.ClientCenterId == request.RequsetCurrentClientCenterId
            });
            f.Entities = ApplyAccess.ApplyFileAccessList(f.Entities, request.RequsetCurrentUserId).ToList();
            return new FileFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = f.Entities
            };
        }

        public BaseListResponse<FileVM> ApplyFindByExpressionPagingTo(FileFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            Expression<Func<FileModel, bool>> r = x => x.Folder.ClientCenterId == requestExp.RequsetCurrentClientCenterId;
            ParameterExpression p = r.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[expression.Parameters[0]] = p;

            Expression body = Expression.AndAlso(r.Body, visitor.Visit(expression.Body));
            var ex = Expression.Lambda<Func<FileModel, bool>>(body, p);

            var total = _db.File.Where(ex).Count();
            var c = _db.File.Where(ex).OrderByDescending(pp => pp.Id).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList();
            var ents = new List<FileExpVM>();

            foreach (var f in c)
            {
                var ent = new FileExpVM();
                ent = f.ToViewModelExp();
                ents.Add(ent);
            }
            return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ApplyAccess.ApplyFileAccessList(ents
                .ToViewModelListExp(), requestExp.RequsetCurrentUserId).ToList(),
                Index = requestExp.Index,
                Count = requestExp.Count,
                TotalRecords = total,

            };
        }
        public BaseListResponse<FileVM> ApplyFindAllPagingTo(FileFindAllRequest request)
        {
            var f = ApplyFindByExpressionTo(new FileFindByExpressionExpRequest()
            {
                RequsetCurrentUserId = request.RequsetCurrentUserId,
                RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,
                expression = e => e.Folder.ClientCenterId == request.RequsetCurrentClientCenterId,
                Count = request.Count,
                Index = request.Index,
            });
            f.Entities = ApplyAccess.ApplyFileAccessList(f.Entities, request.RequsetCurrentUserId).ToList();
            return new FileFindAllResponse
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
        public Expression<Func<FileModel, bool>> ToViewModel
            (Expression<Func<FileExpVM, bool>> File)
        {
            return Mapper.Map<Expression<Func<FileExpVM, bool>>,
                          Expression<Func<FileModel, bool>>>(File);
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
