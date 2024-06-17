using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FolderMdl.VMs
{
    public class TypesOfFolderVM : ViewModelBase<int>
    {
        public virtual FolderVM Folder { get; set; }
        public int FolderId { get; set; }
        public virtual TypeofFolderVM TypeofFolder { get; set; }
        public int TypeofFolderId { get; set; }
    }
}
