using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.TypeofFile;
using YA.Infrastructure;

namespace YA.Archive.Model.File
{
    [Table("TypesOfFile")]
    public  class TypesOfFileModel: ModelBase<int>
    {
        [ForeignKey("FileId")]
        public virtual FileModel File { get; set; }
        public int FileId { get; set; }
        [ForeignKey("TypeofFileId")]
        public virtual TypeofFileModel TypeofFile { get; set; }
        public int TypeofFileId { get; set; }
    }
}
