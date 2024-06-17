using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BasePagingRequest:RequestBase
    {
        public int Index { get; set; }
        public int Count { get; set; }

    }
}
