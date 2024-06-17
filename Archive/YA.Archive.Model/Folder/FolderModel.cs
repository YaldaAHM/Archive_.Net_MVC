using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.File;
using YA.Infrastructure;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Model.UserManagement;

namespace YA.Archive.Model.Folder
{
    [Table("Folder")]
    public class FolderModel : ModelBase<int>
    {
        public string Code { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public string Path { get; set; }

        [ForeignKey("ClientCenterId")]
        public virtual ClientCenterModel ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
        public virtual List<TypesOfFolderModel> TypesOfFolders { get; set; }
 
        public string KeyWord { get; set; }
        public string Description { get; set; }
        [ForeignKey("CreateUserId")]
        public virtual UserModel CreateUser { get; set; }
        public string CreateUserId { get; set; }
        [ForeignKey("LastUpdateUserId")]
        public virtual UserModel LastUpdateUser { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime EditLockDate { get; set; }
        public DateTime RemoveLockDate { get; set; }
        public DateTime EditFileLockDate { get; set; }
        public DateTime RemoveFileLockDate { get; set; }
        public DateTime EditCommentLockDate { get; set; }
        public DateTime RemoveCommentLockDate { get; set; }


        public bool IsEditLockDate { get; set; }
        public bool IsRemoveLockDate { get; set; }
        public bool IsEditFileLockDate { get; set; }
        public bool IsRemoveFileLockDate { get; set; }
        public bool IsEditCommentLockDate { get; set; }
        public bool IsRemoveCommentLockDate { get; set; }


        public virtual List<FileModel> Files { get; set; }
        public virtual List<CommentModel> Comments { get; set; }

    }
}
