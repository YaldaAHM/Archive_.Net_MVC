using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Infrastructure.Infrastructure.Permission
{
    public class PermissionClaimsReportsInitialize
    {
     
        public List<PermissionClaimsModel> PermissionClaimsReportsInitializer()
        {
            List<PermissionClaimsModel> inlineDefault = new List<PermissionClaimsModel>
                {
                    new PermissionClaimsModel
                    {
                        ClaimId="RTStatement",
                        ViewClaimId="CanViewRTStatement",
                        View=false,Description=" General Status Report"
                    },
                    new PermissionClaimsModel
                    {
                        ClaimId="ROdConflict",
                        ViewClaimId="CanViewROdConflict",
                        View=false,Description="Discrepancy Report"
                    },
                    new PermissionClaimsModel
                    {
                        ClaimId="ROdStatement",
                        ViewClaimId="CanViewROdStatement",
                        View=false,Description="Order Status Report"
                    },
                    new PermissionClaimsModel
                    {
                        ClaimId="ROdClientDaily",
                        ViewClaimId="CanViewROdClientDaily",
                        View=false,Description="Daily Report of Center Orders"

                    },
                  
                   

                };
            return inlineDefault;
        }
    }
}