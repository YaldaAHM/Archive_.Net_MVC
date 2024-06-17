using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FileMdl.VMs
{
    public class TypesOfFileVM  : ViewModelBase<int>
    {
        public virtual FileVM File { get; set; }
        public int FileId { get; set; }
        public virtual TypeofFileVM TypeofFile { get; set; }
        public int TypeofFileId { get; set; }
    }
}

