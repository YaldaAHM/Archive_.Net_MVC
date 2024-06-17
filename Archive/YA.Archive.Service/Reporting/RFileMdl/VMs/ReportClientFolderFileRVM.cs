using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RFileMdl.VMs
{
    public class ReportClientFolderFileRVM
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string TodayDate { get; set; }
        public List<ReportFolderRVM> ReportFolderRVMs { get; set; }
    }
}
