using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.GroupMdl.VMs
{
    public class GoupClaimVM : ViewModelBase<int>
    {
        public virtual GroupVM Group { get; set; }
        public int GroupId { get; set; }
        public string ClaimValue{ get; set; }
    }
}
