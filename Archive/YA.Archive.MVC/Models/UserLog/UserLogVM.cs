using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.MVC2.Models.UserLog
{
    public class UserLogVM : ViewModelBase<int>
    {
        public string ProviderKey { get; set; }
        public UserVM User { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public string LoginTimeStr { get; set; }
        public string LogoutTimeStr { get; set; }
        public int ClientCenterId { get; set; }
        [Display(Name = "User Group ")]
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        [Display(Name = "User Group ")]
        public List<GroupVM> Groups { get; set; }
    }
}