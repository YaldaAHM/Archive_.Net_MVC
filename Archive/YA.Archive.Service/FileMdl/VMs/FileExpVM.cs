using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FileMdl.VMs
{
    public class FileExpVM : ViewModelBase<int>
    {
        public string Code { get; set; }
        public virtual FolderVM Folder { get; set; }
        public int FolderId { get; set; }
        public string FileName { get; set; }
        public string OrginalName { get; set; }
        public int FileSize { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public virtual List<TypesOfFileVM> TypesOfFiles { get; set; }

        public virtual UserVM CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
        public string LastUpdateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }


        public bool IsAllowEdit { get; set; }
        public bool IsAllowDelete { get; set; }
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
