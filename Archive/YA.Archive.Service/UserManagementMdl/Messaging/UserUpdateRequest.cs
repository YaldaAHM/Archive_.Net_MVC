﻿using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.Messaging
{
    public class UserUpdateRequest : BaseEntityResponse<EditVM>
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        public string OldPassword { get; set; }
    }
}
