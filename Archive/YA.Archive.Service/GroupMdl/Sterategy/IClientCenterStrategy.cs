using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Infrastructure.Service;
using YA.Archive.Service.GroupMdl.Messaging;

namespace YA.Archive.Service.GroupMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<GroupVM>, GroupFindAllRequest, GroupFindByExpressionExpRequest>
    {
         BaseListResponse<GroupVM> ApplyFindAllTo(GroupFindAllRequest request);

        BaseListResponse<GroupVM> ApplyFindByExpressionTo(GroupFindByExpressionExpRequest requestExp);
    }
}
