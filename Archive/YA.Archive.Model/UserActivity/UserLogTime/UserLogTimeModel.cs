using YA.Archive.Model.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure;

namespace YA.Archive.Model.UserActivity.UserLogTime
{
    [Table("UserLogTime")]
    public class UserLogTimeModel : ModelBase<int>
    {
          public string ProviderKey { get; set; }
        public virtual UserModel User { get; set; }
        public string UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
      

    }
}
