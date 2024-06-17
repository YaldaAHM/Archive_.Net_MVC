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
using YA.Archive.Service.ClientCenterAMdl.Mapping;
using YA.Archive.Service.ClientCenterAMdl.Messaging;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.ClientCenterAMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<ClientCenterAVM> ApplyFindByExpressionTo(ClientCenterAFindByExpressionRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
           var c = _db.ClientCenter.Where(expression).ToList();
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
            var c = _db.ClientCenter.ToList();
            var ents = new List<ClientCenterAVM>();

            foreach (var f in c)
            {
                var ent = new ClientCenterAVM();
                ent = f.ToViewModel();
                ents.Add(ent);
            }
            return new ClientCenterAFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }



        public Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterAVM, bool>> ClientCenter)
        {

           return Mapper.Map<Expression<Func<ClientCenterAVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }
    }
}
