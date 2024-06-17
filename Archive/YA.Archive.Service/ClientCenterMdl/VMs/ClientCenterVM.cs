using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;
using YA.Localization.ValidationLocalize;
using YA.Archive.Infrastructure.Localization.ClientCenterLocalization;

namespace YA.Archive.Service.ClientCenterMdl.VMs
{
    public class ClientCenterVM : ViewModelBase<int>
    {
        
 
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required"),
            StringLength(100, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationResource),
            ErrorMessageResourceName = "StringLength"),
       Display(ResourceType = typeof(ClientCenterResource), Name = "Name")]
        public string Name { get; set; }
    }
}
