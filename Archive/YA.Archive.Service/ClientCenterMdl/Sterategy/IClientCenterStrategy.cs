using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.ClientCenterMdl.Messaging;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.ClientCenterMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<ClientCenterVM>, ClientCenterFindAllRequest, ClientCenterFindByExpressionRequest>
    {
         BaseListResponse<ClientCenterVM> ApplyFindAllTo(ClientCenterFindAllRequest request);

        BaseListResponse<ClientCenterVM> ApplyFindByExpressionTo(ClientCenterFindByExpressionRequest requestExp);
    }
}
