using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.VMs
{
    public class GroupClaimVM : ViewModelBase<int>
    {
        public virtual GroupVM Group { get; set; }
        public int? GroupId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }

    
    }
}
