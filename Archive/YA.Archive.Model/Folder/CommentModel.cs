using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Model.UserManagement;
using YA.Infrastructure;

namespace YA.Archive.Model.Folder
{
    [Table("Comment")]
    public class CommentModel : ModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Code { get; set; }
        [ForeignKey("FolderId")]
        public virtual FolderModel Folder { get; set; }

        public int? FolderId { get; set; }
        public string Description { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual UserModel CreateUser { get; set; }

        public string CreateUserId { get; set; }

        [ForeignKey("LastUpdateUserId")]
        public virtual UserModel LastUpdateUser { get; set; }

        public string LastUpdateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

    }
}
