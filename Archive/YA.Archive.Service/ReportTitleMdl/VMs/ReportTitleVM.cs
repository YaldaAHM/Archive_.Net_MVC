using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.ReportTitleMdl.VMs
{
    public class ReportTitleVM: ViewModelBase<int>
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string ImageUrl { get; set; }
    }
}
