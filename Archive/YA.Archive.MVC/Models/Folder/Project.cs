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
using YA.Archive.Infrastructure.Localization.ProjectLocalization;
using YA.Localization.ValidationLocalize;

namespace YA.Archive.MVC2.Models.Folder
{
    public class Project
    {
        
          [Display(ResourceType = typeof(ProjectResource), Name = "Id")]
        public int Id { get; set; }
          [Display(ResourceType = typeof(ProjectResource), Name = "Code")]
        public string Code { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ValidationResource), ErrorMessageResourceName = "Required"),
           StringLength(100, MinimumLength = 2, ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "StringLength"),
      Display(ResourceType = typeof(ProjectResource), Name = "Name")]
        public string Name { get; set; }
      
        public virtual FolderVM Folder { get; set; }
        public int? FolderId { get; set; }
          [Display(ResourceType = typeof(ProjectResource), Name = "ClientCenterId")]
        public virtual ClientCenterVM ClientCenter { get; set; }
        [UIHint("GridForeignKey")]
        [Display(ResourceType = typeof(ProjectResource), Name = "ClientCenterId")]
        public int ClientCenterId { get; set; }
        [UIHint("TypeOfFolderEditor")]
        [Display(ResourceType = typeof(ProjectResource), Name = "TypeofFolderId")]
        public List<TypeofFolderVM> TypeofFolders { get; set; }
        [Display(ResourceType = typeof(ProjectResource), Name = "TypeofFolderId")]
        public virtual List<TypesOfFolderVM> TypesOfFolders { get; set; }

        [Display(ResourceType = typeof(ProjectResource), Name = "KeyWord")]
         [StringLength(300, ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "MaxLength")]
        public string KeyWord { get; set; }
        [Display(ResourceType = typeof(ProjectResource), Name = "KeyWord")]
        public string KeyWorddd { get; set; }
        [Display(ResourceType = typeof(ProjectResource), Name = "Description")]
         [StringLength(400,  ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(ResourceType = typeof(ProjectResource), Name = "CreateUserId")]
        public virtual UserVM CreateUser { get; set; }
        
        [Display(ResourceType = typeof(ProjectResource), Name = "CreateUserId")]
        public string CreateUserName { get; set; }
        [Display(ResourceType = typeof(ProjectResource), Name = "LastUpdateUserId")]
        public virtual UserVM LastUpdateUser { get; set; }
        
        [Display(ResourceType = typeof(ProjectResource), Name = "LastUpdateUserId")]
        public string LastUpdateUserName { get; set; }
        [DataType(DataType.Date)]
       
        [Display(ResourceType = typeof(ProjectResource), Name = "CreateDate")]
        public string CreateDate { get; set; }
       
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(ProjectResource), Name = "LastUpdateDate")]
        public string LastUpdateDate { get; set; }
       
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(ProjectResource), Name = "EditLockDate")]
        public string EditLockDate { get; set; }
       
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(ProjectResource), Name = "RemoveLockDate")]

        public string RemoveLockDate { get; set; }
       [DataType(DataType.Date)]
      
        [Display(ResourceType = typeof(ProjectResource), Name = "EditFileLockDate")]
        public string EditFileLockDate { get; set; }
         [DataType(DataType.Date)]
       
        [Display(ResourceType = typeof(ProjectResource), Name = "RemoveFileLockDate")]
        public string RemoveFileLockDate { get; set; }
         [DataType(DataType.Date)]
       
        [Display(ResourceType = typeof(ProjectResource), Name = "EditCommentLockDate")]
        public string EditCommentLockDate { get; set; }
        [DataType(DataType.Date)]
        
        [Display(ResourceType = typeof(ProjectResource), Name = "RemoveCommentLockDate")]
        public string RemoveCommentLockDate { get; set; }


        public bool IsEditLockDate { get; set; }
        public bool IsRemoveLockDate { get; set; }
        public bool IsEditFileLockDate { get; set; }
        public bool IsRemoveFileLockDate { get; set; }
        public bool IsEditCommentLockDate { get; set; }
        public bool IsRemoveCommentLockDate { get; set; }

        public string EditLockDateH { get; set; }
        public string RemoveLockDateH { get; set; }
        public string EditFileLockDateH { get; set; }
        public string RemoveFileLockDateH { get; set; }
        public string EditCommentLockDateH { get; set; }
        public string RemoveCommentLockDateH { get; set; }




        //public string LastOwnerId { get; set; }
        //public DateTime LastUpadteOwnerDate { get; set; }
        public virtual List<FolderVM> Folders { get; set; }
        public virtual List<CommentVM> Comments { get; set; }


        public bool IsUpdateLock { get; set; }
        public bool IsDeleteLock { get; set; }
        public bool IsUpdateFileLock { get; set; }
        public bool IsDeleteFileLock { get; set; }
        public bool IsUpdateCommentLock { get; set; }
        public bool IsDeleteCommentLock { get; set; }
        public bool HasClaimView { get; set; }
        public bool HasClaimEdit { get; set; }
        public bool HasClaimDelete { get; set; }
        public bool HasClaimInsert { get; set; }
        public bool HasClaimUpdateLock { get; set; }
        public bool HasClaimDeleteLock { get; set; }
        public bool HasClaimUpdateFileLock { get; set; }
        public bool HasClaimDeleteFileLock { get; set; }
        public bool HasClaimUpdateCommentLock { get; set; }
        public bool HasClaimDeleteCommentLock { get; set; }


        public bool IsUpdateLockCreateUser { get; set; }
        public bool IsDeleteLockCreateUser { get; set; }
        public bool IsUpdateFileLockCreateUser { get; set; }
        public bool IsDeleteFileLockCreateUser { get; set; }
        public bool IsUpdateCommentLockCreateUser { get; set; }
        public bool IsDeleteCommentLockCreateUser { get; set; }
    }
}