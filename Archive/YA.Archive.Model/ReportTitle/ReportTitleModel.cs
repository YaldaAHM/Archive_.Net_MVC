using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure;

namespace YA.Archive.Model.ReportTitle
{
    [Table("ReportTitle")]
    public class ReportTitleModel : ModelBase<int>
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string ImageUrl { get; set; }
    }
}
