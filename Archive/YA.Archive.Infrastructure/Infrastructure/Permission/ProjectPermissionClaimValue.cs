using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Infrastructure.Infrastructure.Permission
{
    public class ProjectPermissionClaimValue
    {
        public static string CanCreate => "CanCreate";
        public static string CanEdit => "CanEdit";
        public static string CanDelete => "CanDelete";
        public static string CanView => "CanView";
        public static string CanUpdateLock => "CanUpdateLock";
        public static string CanDeleteLock => "CanDeleteLock";
        public static string CanUpdateFileLock => "CanUpdateFileLock";
        public static string CanDeleteFileLock => "CanDeleteFileLock";
        public static string CanUpdateCommentLock => "CanUpdateCommentLock";
        public static string CanDeleteCommentLock => "CanDeleteCommentLock";
        
    }
}
