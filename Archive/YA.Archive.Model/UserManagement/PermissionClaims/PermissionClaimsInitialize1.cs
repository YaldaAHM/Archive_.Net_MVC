using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Model.UserManagement.PermissionClaims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Model.UserManagement.PermissionClaims
{
    public class PermissionClaimsInitialize1
    {
        public List<PermissionClaimsModel> PermissionClaimsInitializer()
        {
            List<PermissionClaimsModel> inlineDefault = new List<PermissionClaimsModel>
                {
                    new PermissionClaimsModel
                    {
                        ClaimId="ClientCenter",
                        CreateClaimId="CanCreateClientCenter",EditClaimId="CanEditClientCenter",
                        DeleteClaimId="CanDeleteClientCenter",ViewClaimId="CanViewClientCenter",
                        Create =false,Edit=false,Delete=false,View=false,Description="Centers",
                     

                    },
                    new PermissionClaimsModel
                    {
                        ClaimId="TypeOfFolder",
                        CreateClaimId="CanCreateTypeOfFolder",EditClaimId="CanEditTypeOfFolder",
                        DeleteClaimId="CanDeleteTypeOfFolder",ViewClaimId="CanViewTypeOfFolder",
                        Create =false,Edit=false,Delete=false,View=false,Description="Folder Type"

                    },
                    new PermissionClaimsModel
                    {
                        ClaimId="TypeOfFile",
                        CreateClaimId="CanCreateTypeOfFile",EditClaimId="CanEditTypeOfFile",
                        DeleteClaimId="CanDeleteTypeOfFile",ViewClaimId="CanViewTypeOfFile",
                        Create =false,Edit=false,Delete=false,View=false,Description="File Type "
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

                };
            return inlineDefault;
        }
    }
}
