using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class ResponseBase
    {
        
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }

    }
}
