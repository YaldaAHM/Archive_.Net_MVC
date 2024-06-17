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
using System.Web.Mvc;

namespace YA.Archive.MVC2.Models.Folder
{
    public class ProjectViewModel
    {
        
        public int Id { get; set; }
        public string Code { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        [Display(Name = "Project Name")]
        public string Name { get; set; }
      
        public virtual FolderVM Folder { get; set; }
        public int? FolderId { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        [UIHint("GridForeignKey")]
        public int ClientCenterId { get; set; }
        [UIHint("TypeOfFolderEditor")]
        public List<TypeofFolderVM> TypeofFolders { get; set; }
        public virtual List<TypesOfFolderVM> TypesOfFolders { get; set; }
       
        [Display(Name = "Password")]
        [UIHint("TagInputEditor")]
        public string KeyWord { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual UserVM CreateUser { get; set; }
        
        public string CreateUserName { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
        
        public string LastUpdateUserName { get; set; }
       
      
        public string CreateDate { get; set; }
      
       
        public string LastUpdateDate { get; set; }
      
       
        public string EditLockDate { get; set; }
      
       
        public string RemoveLockDate { get; set; }
       
      
        public string EditFileLockDate { get; set; }
       
      
        public string RemoveFileLockDate { get; set; }
       
      
        public string EditCommentLockDate { get; set; }
       
      
        public string RemoveCommentLockDate { get; set; }
        
        public virtual List<FolderVM> Folders { get; set; }
        public virtual List<CommentVM> Comments { get; set; }
    }
}