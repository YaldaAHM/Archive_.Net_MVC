using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FolderMdl.Sterategy
{
    public interface IClientCenterStrategy : IClientCenterStrategy<BaseListResponse<FolderVM>, FolderFindAllRequest, FolderFindByExpressionExpRequest>
    {
         BaseListResponse<FolderVM> ApplyFindAllTo(FolderFindAllRequest request);

        BaseListResponse<FolderVM> ApplyFindByExpressionTo(FolderFindByExpressionExpRequest requestExp);
        BaseListResponse<FolderVM> ApplyFindAllPagingTo(FolderFindAllRequest request);

        BaseListResponse<FolderVM> ApplyFindByExpressionPagingTo(FolderFindByExpressionExpRequest requestExp);
    }
}
