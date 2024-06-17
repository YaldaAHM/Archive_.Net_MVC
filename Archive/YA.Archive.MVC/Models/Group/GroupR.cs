using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.MVC2.Models.Group
{
    public class GroupR : ViewModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        [Display(Name = "Group Name")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public string CreateDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Validity Date")]
        public string ValidityDate { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        public virtual List<GoupClaimVM> GoupClaims { get; set; }
        public string RequsetCurrentUserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}