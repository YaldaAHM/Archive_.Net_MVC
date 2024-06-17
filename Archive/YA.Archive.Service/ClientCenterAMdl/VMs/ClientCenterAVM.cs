using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;
using YA.Localization.ValidationLocalize;
using YA.Archive.Infrastructure.Localization.ClientCenterLocalization;

namespace YA.Archive.Service.ClientCenterAMdl.VMs
{
    public class ClientCenterAVM : ViewModelBase<int>
    {


        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required"),
            StringLength(100, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationResource),
            ErrorMessageResourceName = "StringLength"),
       Display(ResourceType = typeof(ClientCenterResource), Name = "Name")]
        public string Name { get; set; }



        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ClientCenterId { get; set; }
    }
}
