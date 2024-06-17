using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FolderMdl.VMs
{
    public class FolderExpVM:ViewModelBase<int>
    {
        public string Code { get; set; }
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public string Path { get; set; }
        public virtual ClientCenterVM ClientCenter { get; set; }
        public int ClientCenterId { get; set; }
        public virtual List<TypesOfFolderVM> TypesOfFolders { get; set; }

        public string KeyWord { get; set; }
        public string Description { get; set; }
        public virtual UserVM CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
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


        public virtual List<FileVM> Files { get; set; }
        public virtual List<CommentExpVM> Comments { get; set; }


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
