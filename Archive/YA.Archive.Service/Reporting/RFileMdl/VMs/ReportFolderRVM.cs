using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RFileMdl.VMs
{
    public class ReportFolderRVM
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public ReportClientFolderFileRVM ReportClientFolderRVM { get; set; }
        public List<ReportCFFileRVM> ReportCFFileRVMs { get; set; }
    }
}
