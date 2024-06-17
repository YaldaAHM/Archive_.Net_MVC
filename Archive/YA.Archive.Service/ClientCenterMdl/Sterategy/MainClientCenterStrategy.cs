using YA.Archive.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterMdl.Mapping;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.ClientCenterMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<ClientCenterVM> ApplyFindByExpressionTo(ClientCenterFindByExpressionRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
           var c = _db.ClientCenter.Where(expression).ToList();
            var ents = new List<ClientCenterVM>();

            foreach (var f in c)
            {
                var ent = new ClientCenterVM();
                ent = f.ToViewModel();
                 ents.Add(ent);
            }
           
            return new ClientCenterFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents

               

            };
        }

        public BaseListResponse<ClientCenterVM> ApplyFindAllTo(ClientCenterFindAllRequest request)
        {
            var c = _db.ClientCenter.ToList();
            var ents = new List<ClientCenterVM>();

            foreach (var f in c)
            {
                var ent = new ClientCenterVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            return new ClientCenterFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }



        public Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterVM, bool>> ClientCenter)
        {
            return Mapper.Map<Expression<Func<ClientCenterVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }
    }
}
