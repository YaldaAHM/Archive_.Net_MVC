using YA.Archive.Service;
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
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<FolderVM> ApplyFindByExpressionTo(FolderFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
           var c = _db.Folder.Where(expression).ToList();
            var ents = new List<FolderVM>();

            foreach (var f in c)
            {
                var ent = new FolderVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFolderAccessList(ents,requestExp.RequsetCurrentUserId)
                .ToList();
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents

            };
        }

       

        public BaseListResponse<FolderVM> ApplyFindAllTo(FolderFindAllRequest request)
        {
            var c = _db.Folder.ToList();
            var ents = new List<FolderVM>();

            foreach (var f in c)
            {
                var ent = new FolderVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFolderAccessList(ents, request.RequsetCurrentUserId) 
                .ToList()
               ;
            return new FolderFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
            };
        }
public BaseListResponse<FolderVM> ApplyFindByExpressionPagingTo(FolderFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var total = _db.Folder.Where(expression).Count();
            var c = _db.Folder.Where(expression).OrderByDescending(p => p.Id).Skip(requestExp.Count*(requestExp.Index-1)).Take(requestExp.Count).ToList();
             var ents = new List<FolderVM>();

            foreach (var f in c)
            {
                var ent = new FolderVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFolderAccessList(ents, requestExp.RequsetCurrentUserId)
                .ToList();
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
                Index=requestExp.Index,
                Count = requestExp.Count,
                TotalRecords = total,
                
                

            };
        }
 public BaseListResponse<FolderVM> ApplyFindAllPagingTo(FolderFindAllRequest request)
        {
            var total = _db.Folder.Select(p => p.Id).Count();
            var c = _db.Folder.OrderByDescending(p => p.Id).Skip(request.Count * (request.Index - 1)).Take(request.Count).ToList();
            var ents = new List<FolderVM>();

            foreach (var f in c)
            {
                var ent = new FolderVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            ents = ApplyAccess.ApplyFolderAccessList(ents, request.RequsetCurrentUserId)
                .ToList()
               ;
            return new FolderFindAllResponse
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

        

        public Expression<Func<FolderModel, bool>> ToViewModel
            (Expression<Func<FolderExpVM, bool>> Folder)
        {
            return Mapper.Map<Expression<Func<FolderExpVM, bool>>,
                Expression<Func<FolderModel, bool>>>(Folder);
        }
    }
}
