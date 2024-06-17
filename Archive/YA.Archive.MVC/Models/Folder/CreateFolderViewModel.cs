using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.TypeofFolderMdl.VMs;

namespace YA.Archive.MVC2.Models.Folder
{
    public class CreateFolderViewModel
    {
        public  IEnumerable<FolderVM> Folders { get; set; }
        public FolderVM Folder { get; set; }
        public IEnumerable<ClientCenterVM> ClientCenters { get; set; }
        public IEnumerable<TypeofFolderVM> TypeOfFolders { get; set; }
    }
}