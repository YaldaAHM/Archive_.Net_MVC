using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.GroupMdl.VMs
{
    public class GroupVM : ViewModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        [Display(Name = "Group Name")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Create Date")]
        public string CreateDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Validity Date")]
        public string ValidityDate { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        public virtual List<GoupClaimVM> GoupClaims { get; set; }
    }
}
