using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RFolderMdl.Messaging
{
    public class ReportClientFolderRRequest : BaseEntityRequest<FolderExpVM>
    {
        public string RequsetCurrentUserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
