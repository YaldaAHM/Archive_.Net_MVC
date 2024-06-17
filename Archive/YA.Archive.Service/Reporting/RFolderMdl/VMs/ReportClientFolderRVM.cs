using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RFolderMdl.VMs
{
    public class ReportClientFolderRVM
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string TodayDate { get; set; }
        public List<ReportCFolderRVM> ReportCFolderVMs { get; set; }
    }
}
