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
    public class UserLogTimeFindByVMRequest : BaseEntityRequest<UserLogTimeExpVM>
    {
        public int Count { get; set; }
        public int Index { get; set; }
        public int TotalRecord { get; set; }
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
