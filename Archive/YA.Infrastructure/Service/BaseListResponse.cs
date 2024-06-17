using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseListResponse<T>:ResponseBase
        where T:class
    {
        public IEnumerable<T> Entities { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public int TotalRecords { get; set; }
    }
}
