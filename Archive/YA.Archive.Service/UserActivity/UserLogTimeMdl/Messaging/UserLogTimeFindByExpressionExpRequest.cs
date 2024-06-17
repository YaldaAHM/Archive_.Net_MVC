using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging
{
    public class UserLogTimeFindByExpressionExpRequest : BaseExpressionRequest<Expression<Func<UserLogTimeExpVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
