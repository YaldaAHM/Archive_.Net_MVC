using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RUserLogTimeMdl.VMs
{
    public class ReportUserLogTimeRVM
    {
        public int UserLogTimeId { get; set; }
        public string UserLogTimeName { get; set; }
        public ReportClientUserLogTimeRVM ReportClientUserLogTimeRVM { get; set; }
        public List<ReportCUserLogTimeRVM> ReportCUserLogTimeVMs { get; set; }
    }
}
