using YA.Archive.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.File;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FileMdl.Mapping;
using YA.Archive.Service.FolderMdl.Access;

namespace YA.Archive.Service.FileMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<FileVM> ApplyFindByExpressionTo(FileFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
           var c = _db.File.Where(expression).ToList();
            var ents = new List<FileVM>();

            foreach (var f in c)
            {
                var ent = new FileVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFileAccessList(ents,requestExp.RequsetCurrentUserId)
                .ToList();
            return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents

              

            };
        }

        

        public BaseListResponse<FileVM> ApplyFindAllTo(FileFindAllRequest request)
        {
            var c = _db.File.ToList();
            var ents = new List<FileVM>();

            foreach (var f in c)
            {
                var ent = new FileVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFileAccessList(ents, request.RequsetCurrentUserId) 
                .ToList()
               ;
            return new FileFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
            };
        }




        public BaseListResponse<FileVM> ApplyFindByExpressionPagingTo(FileFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var total = _db.File.Where(expression).Count();
            var c = _db.File.Where(expression).OrderByDescending(p => p.Id).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList();
            var ents = new List<FileVM>();

            foreach (var f in c)
            {
                var ent = new FileVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFileAccessList(ents, requestExp.RequsetCurrentUserId)
                .ToList();
            return new FileFindByExpressionResponse
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
        public BaseListResponse<FileVM> ApplyFindAllPagingTo(FileFindAllRequest request)
        { var total = _db.File.Select(p => p.Id).Count();
            var c = _db.File.OrderByDescending(p => p.Id).Skip(request.Count * (request.Index - 1)).Take(request.Count).ToList();
           
             var ents = new List<FileVM>();

            foreach (var f in c)
            {
                var ent = new FileVM();
                ent = f.ToViewModel();
               ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFileAccessList(ents, request.RequsetCurrentUserId)
                .ToList()
               ;
            return new FileFindAllResponse
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
        public Expression<Func<FileModel, bool>> ToViewModel
            (Expression<Func<FileExpVM, bool>> File)
        {
            return Mapper.Map<Expression<Func<FileExpVM, bool>>,
                Expression<Func<FileModel, bool>>>(File);
        }
    }
}
