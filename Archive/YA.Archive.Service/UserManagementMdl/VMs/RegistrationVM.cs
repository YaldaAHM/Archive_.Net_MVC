using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YA.Archive.Service.GroupUserMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.VMs
{
    public class RegistrationVM
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int ClientCenterId { get; set; }
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        [UIHint("FruitsEditor")]
        public List<SelectListItem> Fruits { get; set; }
        public int[] FruitIds { get; set; }
        public bool IsAdmin { get; set; }
    }
}
