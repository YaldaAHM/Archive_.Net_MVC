using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure
{
    public class ReturnClass<T> 
        where T : class, new()
    {
        public T Cls { get; set; }
        public ReturnClass()
        {
            Cls = new T();
        }
    }
}
