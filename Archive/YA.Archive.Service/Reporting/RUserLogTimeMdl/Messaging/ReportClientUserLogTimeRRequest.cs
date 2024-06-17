
using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.Reporting.RUserLogTimeMdl.Messaging
{
    public class ReportClientUserLogTimeRRequest : BaseEntityRequest<UserVM>
    {
        public string RequsetCurrentUserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
