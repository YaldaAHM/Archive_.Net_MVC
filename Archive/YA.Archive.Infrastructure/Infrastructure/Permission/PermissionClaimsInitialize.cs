using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Infrastructure.Infrastructure.Permission
{
    public class PermissionClaimsInitialize
    {
        public List<PermissionClaimsModel> PermissionClaimsInitializer()
        {
            List<PermissionClaimsModel> inlineDefault = new List<PermissionClaimsModel>
                {
                    
                    new PermissionClaimsModel
                    {
                        ClaimId="TypeOfFolder",
                        CreateClaimId="CanCreateTypeOfFolder",EditClaimId="CanEditTypeOfFolder",
                        DeleteClaimId="CanDeleteTypeOfFolder",ViewClaimId="CanViewTypeOfFolder",
                        Create =false,Edit=false,Delete=false,View=false,Description="Folder Type"

                    },
                    
                    new PermissionClaimsModel
                    {
                        ClaimId="Group",
                        CreateClaimId="CanCreateGroup",EditClaimId="CanEditGroup",
                        DeleteClaimId="CanDeleteGroup",ViewClaimId="CanViewGroup",
                        Create =false,Edit=false,Delete=false,View=false,Description="Group"

                    },
                   
                     new PermissionClaimsModel
                    {
                        ClaimId="User",
                        CreateClaimId="CanCreateUser",EditClaimId="CanEditUser",
                        DeleteClaimId="CanDeleteUser",ViewClaimId="CanViewUser",
                        Create =false,Edit=false,Delete=false,View=false,Description="Users"

                    },
                       new PermissionClaimsModel
                    {
                        ClaimId="Project",
                        CreateClaimId="CanCreateProject",EditClaimId="CanEditProject",
                        DeleteClaimId="CanDeleteProject",ViewClaimId="CanViewProject",
                        Create =false,Edit=false,Delete=false,View=false,Description="Document"

                    },
                        new PermissionClaimsModel
                    {
                        ClaimId="File",
                        CreateClaimId="CanCreateFile",EditClaimId="CanEditFile",
                        DeleteClaimId="CanDeleteFile",ViewClaimId="CanViewFile",
                        Create =false,Edit=false,Delete=false,View=false,Description="Attachment"

                    },
                        new PermissionClaimsModel
                    {
                        ClaimId="Comment",
                        CreateClaimId="CanCreateComment",EditClaimId="CanEditComment",
                        DeleteClaimId="CanDeleteComment",ViewClaimId="CanViewComment",
                        Create =false,Edit=false,Delete=false,View=false,Description="Comment"

                    },
                   
                };
            return inlineDefault;
        }
    }
}
