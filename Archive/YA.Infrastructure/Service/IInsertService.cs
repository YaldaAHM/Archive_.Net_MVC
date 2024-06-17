using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public interface IInsertService<TSource, TDest, TId> //: IService
    {
        TDest Insert(TSource entity);
    }
}
