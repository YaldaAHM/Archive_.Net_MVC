using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.File;
using YA.Archive.Model.TypeofFile;
using YA.Infrastructure;

namespace YA.Archive.Model.Attachment
{
    [Table("File")]
    public class AttachmentModel : ModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Subject { get; set; }
        [ForeignKey("FileId")]
        public virtual FileModel File { get; set; }
        public int FileId { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }
        [ForeignKey("TypeofFileId")]
        public virtual TypeofFileModel TypeofFile { get; set; }
        public int TypeofFileId { get; set; }
        public string CreateUserId { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime EditLockDate { get; set; }
        public DateTime RemoveLockDate { get; set; }
        public string LastOwnerId { get; set; }
        public DateTime LastUpadteOwnerDate { get; set; }

    }
}
