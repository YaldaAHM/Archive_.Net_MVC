using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Infrastructure.Localization.FileLocalization;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Localization.ValidationLocalize;

namespace YA.Archive.MVC2.Models.File
{
    public class File
    {
        public int FolderHiddenId { get; set; }
        
        // [Required, StringLength(100, MinimumLength = 2)]
        [Display(ResourceType = typeof(FileResource), Name = "Id")]
        public int Id { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "Code")]
        public string Code { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "FolderId")]
        public virtual FolderVM Folder { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "FolderId")]
        [UIHint("GridForeignKey")]
        
        public int FolderId { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "FileName")]
    
        public string FileName { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "OrginalName")]
        public string OrginalName { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "FileSize")]
        public string FileSize { get; set; }
    
        [Display(ResourceType = typeof(FileResource), Name = "KeyWord")]
        [StringLength(300, ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "MaxLength")]
        [UIHint("TagInputEditor")]
        public string KeyWord { get; set; }
        public string KeyWordd { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "KeyWord")]
        public string KeyWorddd { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, ErrorMessageResourceType = typeof(ValidationResource),
           ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "Path")]
        public string Path { get; set; }
       
        [UIHint("TypeOfFileEditor")]
        [Display(ResourceType = typeof(FileResource), Name = "TypeofFileId")]
        public List<TypeofFileVM> TypeofFiles { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "TypeofFileId")]
        public virtual List<TypesOfFileVM> TypesOfFiles { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "CreateUserId")]
        public virtual UserVM CreateUser { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "CreateUserId")]
        public string CreateUserId { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "LastUpdateUserId")]
        public virtual UserVM LastUpdateUser { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "LastUpdateUserId")]
        public string LastUpdateUserId { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "CreateDate")]
        public string CreateDate { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "LastUpdateDate")]
        public string LastUpdateDate { get; set; }

       // [Display(ResourceType = typeof(FileResource), Name = "FolderId")]
        [UIHint("GridForeignKey")]
        public int ClientCenterId { get; set; }
        [Display(ResourceType = typeof(FileResource), Name = "CreateUserId")]
        public string CreateUserName { get; set; }
       
        
        [Display(ResourceType = typeof(FileResource), Name = "LastUpdateUserId")]
        public string LastUpdateUserName { get; set; }

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

    }
}