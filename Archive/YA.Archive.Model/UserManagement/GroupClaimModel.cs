using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.Group;
using YA.Infrastructure;

namespace YA.Archive.Model.UserManagement
{
    [Table("GroupClaim")]
    public class GroupClaimModel:ModelBase<int>
    {
        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }
        public int GroupId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
         }
}
