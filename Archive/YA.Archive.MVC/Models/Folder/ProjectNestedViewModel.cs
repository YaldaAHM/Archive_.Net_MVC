using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.MVC2.Models.Folder
{
    public class ProjectNestedViewModel
    {
        
        [Required, StringLength(100, MinimumLength = 2)]
        public string Code { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }
      
        public virtual FolderVM Folder { get; set; }
        public int? FolderId { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        [UIHint("GridForeignKey")]
        public int ClientCenterId { get; set; }
        public virtual TypeofFolderVM TypeofFolder { get; set; }
        [UIHint("GridForeignKey")]
        public int TypeofFolderId { get; set; }
        [Display(Name = "Password")]
        [UIHint("TagInputEditor")]
        public string KeyWord { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual UserVM CreateUser { get; set; }
      
        public string CreateUserId { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
      
        public string LastUpdateUserId { get; set; }
        [DataType(DataType.Date), ReadOnly(true)]
      
        public string CreateDate { get; set; }
      
        [DataType(DataType.Date), ReadOnly(true)]
        public string LastUpdateDate { get; set; }
      
        [DataType(DataType.Date)]
        public string EditLockDate { get; set; }
        
       [DataType(DataType.Date)]
        public string RemoveLockDate { get; set; }
         [DataType(DataType.Date)]
      
        public string EditFileLockDate { get; set; }
         [DataType(DataType.Date)]
      
        public string RemoveFileLockDate { get; set; }
        [DataType(DataType.Date)]
        
        public string EditCommentLockDate { get; set; }
        [DataType(DataType.Date)]
      
        public string RemoveCommentLockDate { get; set; }
    
        public virtual List<FolderVM> Folders { get; set; }
        public virtual List<CommentVM> Comments { get; set; }
    }
}