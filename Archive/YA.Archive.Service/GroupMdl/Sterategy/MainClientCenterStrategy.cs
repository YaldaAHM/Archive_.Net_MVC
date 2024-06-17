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
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupMdl.Messaging;
using YA.Archive.Model.Group;
using YA.Archive.Service.GroupMdl.Mapping;

namespace YA.Archive.Service.GroupMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<GroupVM> ApplyFindByExpressionTo(GroupFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
           var c = _db.Group.Where(expression).ToList();
             var ents = new List<GroupVM>();

            foreach (var f in c)
            {
                var ent = new GroupVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }

            return new GroupFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents


            };
        }

        public BaseListResponse<GroupVM> ApplyFindAllTo(GroupFindAllRequest request)
        {
            var c = _db.Group.ToList();
            var ents = new List<GroupVM>();

            foreach (var f in c)
            {
                var ent = new GroupVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            return new GroupFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }



        public Expression<Func<GroupModel, bool>> ToViewModel
            (Expression<Func<GroupExpVM, bool>> Folder)
        {

            return Mapper.Map<Expression<Func<GroupExpVM, bool>>,
                Expression<Func<GroupModel, bool>>>(Folder);
        }

    }
}
