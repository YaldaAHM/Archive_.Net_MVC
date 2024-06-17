using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.Folder;
using YA.Archive.Model.TypeofFile;
using YA.Archive.Model.UserManagement;
using YA.Infrastructure;

namespace YA.Archive.Model.File
{
    [Table("File")]
    public class FileModel : ModelBase<int>
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Code { get; set; }
        [ForeignKey("FolderId")]
        public virtual FolderModel Folder { get; set; }
        public int FolderId { get; set; }
        public string FileName { get; set; }
        public string OrginalName { get; set; }
        public int FileSize { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public virtual List<TypesOfFileModel> TypesOfFiles { get; set; }
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
