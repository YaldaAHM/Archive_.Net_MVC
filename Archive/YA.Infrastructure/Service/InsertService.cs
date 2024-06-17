using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class InsertService<TSource, TDest, TId> //: IService<TSource, TDest, TId>
    {
        public TDest Delete(TId Id)
        {
            throw new NotImplementedException();
        }

        public TDest Delete(TSource Entity)
        {
            throw new NotImplementedException();
        }

        public TDest FindAll(TSource Entity)
        {
            throw new NotImplementedException();
        }

        public TDest FindById(TId Id)
        {
            throw new NotImplementedException();
        }

        public TDest Insert(TSource Entity)
        {
            throw new NotImplementedException();
        }

        public TDest Update(TSource Entity)
        {
            throw new NotImplementedException();
        }
    }
}
