using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Infrastructure.ClientCenterStrategy
{
    public interface IClientCenterStrategy<TResponse, TRequest, TRequestExp>
    {
        TResponse ApplyFindAllTo(TRequest request);
        TResponse ApplyFindByExpressionTo(TRequestExp requestExp);
    }
}
