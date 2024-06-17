using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileFindByExpressionExpRequest: BaseExpressionRequest<Expression<Func<FileExpVM, bool>>>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }

}
