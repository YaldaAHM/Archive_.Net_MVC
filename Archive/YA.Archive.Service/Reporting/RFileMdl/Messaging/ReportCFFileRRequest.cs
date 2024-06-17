using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.Reporting.RFileMdl.Messaging
{
    public class ReportCFFileRRequest : BaseEntityResponse<FileExpVM>
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
