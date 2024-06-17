using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.Group;
using YA.Archive.Model.GroupUser;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Model.UserManagement.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Model.UserManagement
{
    public class UserModel : IdentityUser <string, IdentityUserLogin,
        IdentityUserRole, ApplicationUserClaim>
    {
        public UserModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        [ForeignKey("ClientCenterId")]
        public virtual ClientCenterModel ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
         public virtual List<GroupUserModel> GroupUsers { get; set; }
      
    }
}
