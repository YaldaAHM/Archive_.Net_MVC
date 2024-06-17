using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.MVC2.Models.UserManagement
{
    public class RegistrationViewModel
    {

        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public int ClientCenterId { get; set; }
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        [UIHint("FruitsEditor")]
        public List<SelectListItem> Fruits { get; set; }
        public int[] FruitIds { get; set; } 
        public List<GroupVM> Groups { get; set; }
    }
}