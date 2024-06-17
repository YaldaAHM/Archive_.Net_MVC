using AutoMapper;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Mapping
{
    public static class PermissionClaimsMapper
    {
        public static PermissionClaimsModel ToModel(this PermissionClaimsVM PermissionClaims)
        {
            return Mapper.Map<PermissionClaimsVM, PermissionClaimsModel>(PermissionClaims);
        }
        public static PermissionClaimsVM ToViewModel(this PermissionClaimsModel PermissionClaims)
        {
            return Mapper.Map<PermissionClaimsModel, PermissionClaimsVM>(PermissionClaims);
        }
        public static IEnumerable<PermissionClaimsModel> ToModelList(this IEnumerable<PermissionClaimsVM> PermissionClaimss)
        {
            return Mapper.Map<IEnumerable<PermissionClaimsVM>, List<PermissionClaimsModel>>(PermissionClaimss);
        }
        public static IEnumerable<PermissionClaimsVM> ToViewModelList(this IEnumerable<PermissionClaimsModel> PermissionClaims)
        {
            return Mapper.Map<IEnumerable<PermissionClaimsModel>, List<PermissionClaimsVM>>(PermissionClaims);
        }
        public static GroupClaimModel ToModel(this GroupClaimVM GroupClaim)
        {
            return Mapper.Map<GroupClaimVM, GroupClaimModel>(GroupClaim);
        }
        public static GroupClaimVM ToViewModel(this GroupClaimModel GroupClaim)
        {
            return Mapper.Map<GroupClaimModel, GroupClaimVM>(GroupClaim);
        }
        public static IEnumerable<GroupClaimModel> ToModelList(this IEnumerable<GroupClaimVM> GroupClaim)
        {
            return Mapper.Map<IEnumerable<GroupClaimVM>, List<GroupClaimModel>>(GroupClaim);
        }
        public static IEnumerable<GroupClaimVM> ToViewModelList(this IEnumerable<GroupClaimModel> GroupClaim)
        {
            return Mapper.Map<IEnumerable<GroupClaimModel>, List<GroupClaimVM>>(GroupClaim);
        }
        public static Expression<Func<PermissionClaimsModel, bool>> ToViewModel
            (Expression<Func<PermissionClaimsVM, bool>> File)
        {

            return Mapper.Map<Expression<Func<PermissionClaimsVM, bool>>,
                Expression<Func<PermissionClaimsModel, bool>>>(File);
        }
    }
}
