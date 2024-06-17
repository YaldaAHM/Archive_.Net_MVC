﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.Messaging
{
    public class UserInRoleFindRequest : BaseIdRequest<string>
    {
        public string RoleName { get; set; }
    }
}