using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseEntityRequest<T>:RequestBase
        where T:class,new()
    {
        public T entity { get; set; }
        public BaseEntityRequest()
        {
            entity = new T();
        }
    }
}
