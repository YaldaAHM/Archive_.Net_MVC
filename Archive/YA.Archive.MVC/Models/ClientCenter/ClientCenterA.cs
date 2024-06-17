using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Infrastructure.Localization.ClientCenterLocalization;
using YA.Infrastructure.Service;
using YA.Localization.ValidationLocalize;

namespace YA.Archive.MVC2.Models.ClientCenter
{
    public class ClientCenterA : ViewModelBase<int>
    {


        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required"),
            StringLength(100, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationResource),
            ErrorMessageResourceName = "StringLength"),
       Display(ResourceType = typeof(ClientCenterResource), Name = "Name")]
        public string Name { get; set; }



        public string UserId { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Password ")]
        public string Password { get; set; }
        public int ClientCenterId2 { get; set; }
        [Display(Name = " Client ")]
        public int ClientCenterId { get; set; }
    }
}