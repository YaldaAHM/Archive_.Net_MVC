﻿using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ReportTitleMdl.VMs;

namespace YA.Archive.Service.ReportTitleMdl.Messaging
{
    public class ReportTitleUpdateRequest : BaseEntityResponse<ReportTitleVM>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
    }
}
