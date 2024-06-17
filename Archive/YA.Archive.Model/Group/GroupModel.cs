using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.GroupUser;
using YA.Archive.Model.UserManagement;
using YA.Infrastructure;

namespace YA.Archive.Model.Group
{
    [Table("Group")]
    public class GroupModel : ModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ValidityDate { get; set; }

        [ForeignKey("ClientCenterId")]
        public virtual ClientCenterModel ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
        public virtual List<GroupUserModel> GroupUsers { get; set; }
        public virtual List<GroupClaimModel> GroupClaims { get; set; }

    }
}
