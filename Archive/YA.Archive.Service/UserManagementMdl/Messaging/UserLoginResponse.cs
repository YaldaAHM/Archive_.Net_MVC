using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.UserManagementMdl.Messaging
{
    public class UserLoginResponse : BaseEntityResponse<LoginVM>
    {
        public ClaimsIdentity ident;
    }
}
