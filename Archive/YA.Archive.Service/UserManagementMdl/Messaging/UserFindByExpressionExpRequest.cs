using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.Messaging
{
    public class UserFindByExpressionExpRequest: BaseExpressionRequest<Expression<Func<UserExpVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        //public ClientCenterType ClientCenterType { get; set; }
    }

}
