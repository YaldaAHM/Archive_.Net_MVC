using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseFunkRequest<T> 
    {
        public T expression { get; set; }
        //public T entity { get; set; }
        //public BaseEntityRequest()
        //{
        //    entity = new T();
        //}
    }
}
