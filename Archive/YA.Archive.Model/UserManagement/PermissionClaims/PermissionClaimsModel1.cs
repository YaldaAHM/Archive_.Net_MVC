using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Model.UserManagement.PermissionClaims
{
    public class PermissionClaimsModel1
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
        public UserModel User { get; set; }
    }
}
