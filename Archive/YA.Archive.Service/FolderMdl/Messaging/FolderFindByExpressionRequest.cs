using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.FolderMdl.VMs;

namespace YA.Archive.Service.FolderMdl.Messaging
{
    public class FolderFindByExpressionRequest : BaseExpressionRequest<Expression<Func<FolderVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        //public ClientCenterType ClientCenterType { get; set; }
    }
}
