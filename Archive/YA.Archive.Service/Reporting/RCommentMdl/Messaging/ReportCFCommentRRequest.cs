using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.Reporting.RCommentMdl.VMs;

namespace YA.Archive.Service.Reporting.RCommentMdl.Messaging
{
    public class ReportCFCommentRRequest : BaseEntityRequest<CommentExpVM>
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RequsetCurrentUserId { get; set; }
    }
}
