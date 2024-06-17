using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.VMs
{
    public class FileTVM<TFile> 
    {
        public TFile File { get; set; }

    }
}
