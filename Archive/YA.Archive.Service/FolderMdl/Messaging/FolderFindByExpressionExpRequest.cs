using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FolderMdl.Messaging
{
    public class FolderFindByExpressionExpRequest: BaseExpressionRequest<Expression<Func<FolderExpVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        //public ClientCenterType ClientCenterType { get; set; }
    }

}
