using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.GroupUserMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.VMs
{
    public class EditVM
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        public int? ClientCenterId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
