using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;

namespace YA.Archive.MVC2.Models.User
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    
        public string Email { get; set; }
      
        [Display(Name = "Password ")]
        public string Password { get; set; }
        public int ClientCenterId2 { get; set; }
        [Display(Name = " Client ")]
        public int ClientCenterId { get; set; }
        [Display(Name = "User Group ")]
        public virtual List<GroupUserVM> GroupUsers { get; set; }
        [Display(Name = "User Group ")]
        public List<GroupVM> Groups { get; set; }
    }
}