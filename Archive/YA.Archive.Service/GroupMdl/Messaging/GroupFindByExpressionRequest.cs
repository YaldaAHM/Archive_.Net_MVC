using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.GroupMdl.VMs;

namespace YA.Archive.Service.GroupMdl.Messaging
{
    public class GroupFindByExpressionRequest : BaseExpressionRequest<Expression<Func<GroupVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
