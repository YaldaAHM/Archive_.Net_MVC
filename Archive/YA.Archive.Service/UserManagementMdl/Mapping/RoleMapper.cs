using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.VMs.Role;

namespace YA.Archive.Service.UserManagementMdl.Mapping
{
   public static class RoleMapper
    {
        public static RoleModel ToRoleModel(this RoleVM Role)
        {
            return Mapper.Map<RoleVM, RoleModel>(Role);
        }
        public static RoleVM ToRoleViewModel(this RoleModel Role)
        {
            return Mapper.Map<RoleModel, RoleVM>(Role);
        }
        public static IEnumerable<RoleModel> ToRoleModelList(this IEnumerable<RoleVM> Roles)
        {
            return Mapper.Map<IEnumerable<RoleVM>, List<RoleModel>>(Roles);
        }
        public static IEnumerable<RoleVM> ToRoleViewModelList(this IEnumerable<RoleModel> Role)
        {
            return Mapper.Map<IEnumerable<RoleModel>, List<RoleVM>>(Role);
        }
    }
}
