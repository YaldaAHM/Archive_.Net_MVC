using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.ClientCenterAMdl.Messaging;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.ClientCenterAMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<ClientCenterAVM>, ClientCenterAFindAllRequest, ClientCenterAFindByExpressionRequest>
    {
        BaseListResponse<ClientCenterAVM> ApplyFindAllTo(ClientCenterAFindAllRequest request);

        BaseListResponse<ClientCenterAVM> ApplyFindByExpressionTo(ClientCenterAFindByExpressionRequest requestExp);
    }
}
