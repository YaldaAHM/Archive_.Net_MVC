using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.MVC2.Models.Folder
{
    public class CommentViewModel
    {
        public int ClientCenterId { get; set; }
        public int FolderHiddenId { get; set; }
        [Display(Name = "Id")]
        public string Code { get; set; }
        public int Id { get; set; }
        public FolderVM Folder { get; set; }
        public int FolderId { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public virtual UserVM CreateUser { get; set; }
        public string CreateUserName { get; set; }
        public virtual UserVM LastUpdateUser { get; set; }
        public string LastUpdateUserName { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }




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