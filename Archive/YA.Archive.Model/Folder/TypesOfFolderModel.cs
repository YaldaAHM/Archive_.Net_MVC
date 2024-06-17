using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.TypeofFolder;
using YA.Infrastructure;

namespace YA.Archive.Model.Folder
{
    [Table("TypesOfFolder")]
    public class TypesOfFolderModel : ModelBase<int>
    {
        [ForeignKey("FolderId")]
        public virtual FolderModel Folder { get; set; }
        public int FolderId { get; set; }
        [ForeignKey("TypeofFolderId")]
        public virtual TypeofFolderModel TypeofFolder { get; set; }
        public int TypeofFolderId { get; set; }
    }
}
