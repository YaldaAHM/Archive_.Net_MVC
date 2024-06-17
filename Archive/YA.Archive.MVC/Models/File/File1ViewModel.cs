using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.TypeofFileMdl.VMs;

namespace YA.Archive.MVC2.Models.File
{
    public class File1ViewModel
    {
        public int YearId { get; set; }
        public IEnumerable<ClientCenterVM> ClientCenters { get; set; }
        public IEnumerable<FolderVM> Folders { get; set; }
        public IEnumerable<FolderVM> Projects { get; set; }
        public FileVM File { get; set; }
        public IEnumerable<TypeofFileVM> TypeOfFiles { get; set; }
    }
}