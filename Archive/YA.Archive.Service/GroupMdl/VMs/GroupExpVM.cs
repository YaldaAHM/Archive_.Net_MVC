using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.GroupMdl.VMs
{
    public class GroupExpVM : ViewModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
    }
}
