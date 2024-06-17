using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.ClientCenterAMdl.Messaging
{
    public class ClientCenterAFindByExpressionExpRequest: BaseExpressionRequest<Expression<Func<ClientCenterAVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        //public ClientCenterAType ClientCenterAType { get; set; }
    }

}
