using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Model.UserManagement
{
    public class RoleModel : IdentityRole
    {
        public RoleModel() : base() { }
        public RoleModel(string name) : base(name) { }
    }
}
