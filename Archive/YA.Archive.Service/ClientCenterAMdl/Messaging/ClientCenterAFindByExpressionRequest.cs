using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterAMdl.VMs;

namespace YA.Archive.Service.ClientCenterAMdl.Messaging
{
    public class ClientCenterAFindByExpressionRequest : BaseExpressionRequest<Expression<Func<ClientCenterAVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
