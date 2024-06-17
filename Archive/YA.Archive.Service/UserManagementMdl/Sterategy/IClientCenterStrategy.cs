using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Infrastructure.Service;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Messaging;

namespace YA.Archive.Service.UserManagementMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<UserVM>, UserFindAllRequest, UserFindByExpressionExpRequest>
    {
         BaseListResponse<UserVM> ApplyFindAllTo(UserFindAllRequest request);

        BaseListResponse<UserVM> ApplyFindByExpressionTo(UserFindByExpressionExpRequest requestExp);
        BaseListResponse<UserVM> ApplyFindAllPagingTo(UserFindAllRequest request);

        BaseListResponse<UserVM> ApplyFindByExpressionPagingTo(UserFindByExpressionExpRequest requestExp);

    }
}
