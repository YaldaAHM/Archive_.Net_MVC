using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.TypeofFolderMdl.VMs;

namespace YA.Archive.Service.TypeofFolderMdl.Messaging
{
    public class TypeofFolderFindByExpressionRequest : BaseExpressionRequest<Expression<Func<TypeofFolderVM, bool>>>
    {
    }
}
