using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs
{
    public class PermissionClaimsVM
    {
        public string Id { get; set; }
        public string ClaimId { get; set; }
        public string Description { get; set; }
        public string CreateClaimId { get; set; }
        public bool Create { get; set; }
        public string EditClaimId { get; set; }
       
        public bool Edit { get; set; }
        public string DeleteClaimId { get; set; }
        public bool Delete { get; set; }
        public string ViewClaimId { get; set; }
        public bool View { get; set; }

        public string UpdateLockClaimId { get; set; }
        public bool UpdateLock { get; set; }
        public string DeleteLockClaimId { get; set; }
        public bool DeleteLock { get; set; }
        public string UpdateFileLockClaimId { get; set; }
        public bool UpdateFileLock { get; set; }
        public string DeleteFileLockClaimId { get; set; }
        public bool DeleteFileLock { get; set; }

        public string UpdateCommentLockClaimId { get; set; }
        public bool UpdateCommentLock { get; set; }
        public string DeleteCommentLockClaimId { get; set; }
        public bool DeleteCommentLock { get; set; }
        public UserVM User { get; set; }
    }
}
