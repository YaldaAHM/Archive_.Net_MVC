using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RCommentMdl.VMs
{
    public class ReportFolderRVM
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public ReportClientFolderCommentRVM ReportClientFolderRVM { get; set; }
        public List<ReportCFCommentRVM> ReportCFCommentRVMs { get; set; }
    }
}
