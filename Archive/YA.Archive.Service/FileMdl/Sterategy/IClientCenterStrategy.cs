using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Infrastructure.Service;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FileMdl.Messaging;

namespace YA.Archive.Service.FileMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<FileVM>, FileFindAllRequest, FileFindByExpressionExpRequest>
    {
         BaseListResponse<FileVM> ApplyFindAllTo(FileFindAllRequest request);

        BaseListResponse<FileVM> ApplyFindByExpressionTo(FileFindByExpressionExpRequest requestExp);

        BaseListResponse<FileVM> ApplyFindAllPagingTo(FileFindAllRequest request);

        BaseListResponse<FileVM> ApplyFindByExpressionPagingTo(FileFindByExpressionExpRequest requestExp);

    }
}
