using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseExpressionRequest<T> : RequestBase where T : Expression
    {
        public T expression { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public int TotalRecords { get; set; }
      
    }
}
