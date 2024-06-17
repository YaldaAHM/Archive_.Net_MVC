using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RUserLogTimeMdl.VMs
{
    public class ReportCUserLogTimeRVM
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int UserLogTimeId { get; set; }
        public string UserLogTimeName { get; set; }
        public string GroupUser { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
    }
}
