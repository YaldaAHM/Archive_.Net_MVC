using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Localization.TypeOfProjectLocalization;
using YA.Infrastructure.Service;
using YA.Localization.ValidationLocalize;

namespace YA.Archive.Service.TypeofFolderMdl.VMs
{
    public class TypeofFolderVM : ViewModelBase<int>
    {

        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required"),
           StringLength(100, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "StringLength"),
      Display(ResourceType = typeof(TypeOfProjectResource), Name = "Name")]
        public string Name { get; set; }
    }
}
