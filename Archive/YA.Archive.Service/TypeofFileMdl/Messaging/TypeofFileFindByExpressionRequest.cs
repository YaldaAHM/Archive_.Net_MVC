using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.TypeofFileMdl.VMs;

namespace YA.Archive.Service.TypeofFileMdl.Messaging
{
    public class TypeofFileFindByExpressionRequest : BaseExpressionRequest<Expression<Func<TypeofFileVM, bool>>>
    {
    }
}
