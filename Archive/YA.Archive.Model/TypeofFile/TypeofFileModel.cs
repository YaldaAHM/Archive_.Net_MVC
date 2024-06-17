using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure;

namespace YA.Archive.Model.TypeofFile
{
    [Table("TypeofFile")]
    public class TypeofFileModel : ModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
