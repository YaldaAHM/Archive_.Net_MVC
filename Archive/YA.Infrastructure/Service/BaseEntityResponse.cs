using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseEntityResponse<T>:ResponseBase
        where T:class,new()
    {
        public T entity { get; set; }
        public BaseEntityResponse()
        {
            entity = new T();
        }

    }
}
