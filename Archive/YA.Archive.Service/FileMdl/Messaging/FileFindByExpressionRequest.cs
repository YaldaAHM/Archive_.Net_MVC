using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileFindByExpressionRequest : BaseExpressionRequest<Expression<Func<FileVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
