using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.Group;
using YA.Archive.Model.UserManagement;
using YA.Infrastructure;

namespace YA.Archive.Model.GroupUser
{
    [Table("GroupUser")]
    public class GroupUserModel : ModelBase<int>
    {
        [ForeignKey("GroupId")]
        public virtual GroupModel Group { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserModel User { get; set; }
        public string UserId { get; set; }
    }
}
