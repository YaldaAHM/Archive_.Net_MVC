using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseListRequest<T> : RequestBase
        where T : class
    {
        public IEnumerable<T> Entities { get; set; }
        public int Index { get; set; }
        public int Count { get; set; }
        public int TotalRecords { get; set; }

    }
}
