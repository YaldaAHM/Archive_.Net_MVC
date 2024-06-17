using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.FolderMdl.VMs
{
    public class CommentExpVM : ViewModelBase<int>
    {
        public string Code { get; set; }
        public virtual FolderExpVM Folder { get; set; }
        public int? FolderId { get; set; }

        public string Description { get; set; }
        public virtual UserVM CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }


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
