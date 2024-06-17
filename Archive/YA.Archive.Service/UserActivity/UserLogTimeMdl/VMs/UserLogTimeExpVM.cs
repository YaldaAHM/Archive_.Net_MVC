﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs
{
    public class UserLogTimeExpVM : ViewModelBase<int>
    {
        public string ProviderKey { get; set; }
        public virtual UserVM User { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }
}
