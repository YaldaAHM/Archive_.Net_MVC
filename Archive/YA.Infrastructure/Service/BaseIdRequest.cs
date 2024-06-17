﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public class BaseIdRequest<TId>:RequestBase
    {
        public TId Id { get; set; }
    }
}
