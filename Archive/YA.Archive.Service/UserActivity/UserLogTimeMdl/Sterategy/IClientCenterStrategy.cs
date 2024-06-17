using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<UserLogTimeVM>, UserLogTimeFindAllRequest, UserLogTimeFindByExpressionExpRequest>
    {
           BaseListResponse<UserLogTimeVM> ApplyFindAllTo(UserLogTimeFindAllRequest request);

        BaseListResponse<UserLogTimeVM> ApplyFindByExpressionTo(UserLogTimeFindByExpressionExpRequest requestExp);
        BaseListResponse<UserLogTimeVM> ApplyFindAllPagingTo(UserLogTimeFindAllRequest request);

        BaseListResponse<UserLogTimeVM> ApplyFindByExpressionPagingTo(UserLogTimeFindByExpressionExpRequest requestExp);

    }
}
