using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RFolderMdl.VMs
{
    public class ReportFolder1RVM
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public ReportClientFolderRVM ReportClientFolderRVM { get; set; }
        public List<ReportCFolderRVM> ReportCFolderVMs { get; set; }
    }
}
