using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.DirectoryMdl.VMs
{
    public class DirectoryVM : ViewModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
