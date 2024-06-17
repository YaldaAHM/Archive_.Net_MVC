using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.GroupUser;
using YA.Archive.Service.GroupUserMdl.VMs;

namespace YA.Archive.Service.GroupUserMdl.Mapping
{
    public static class GroupUserMapper
    { 
     public static GroupUserModel ToModel(this GroupUserVM GroupUser)
    {
        return Mapper.Map<GroupUserVM, GroupUserModel>(GroupUser);
    }
    public static GroupUserVM ToViewModel(this GroupUserModel GroupUser)
    {
        return Mapper.Map<GroupUserModel, GroupUserVM>(GroupUser);
    }
    public static IEnumerable<GroupUserModel> ToModelList(this IEnumerable<GroupUserVM> GroupUsers)
    {
        return Mapper.Map<IEnumerable<GroupUserVM>, List<GroupUserModel>>(GroupUsers);
    }
    public static IEnumerable<GroupUserVM> ToViewModelList(this IEnumerable<GroupUserModel> GroupUser)
    {
        return Mapper.Map<IEnumerable<GroupUserModel>, List<GroupUserVM>>(GroupUser);
    }
       
        public static Expression<Func<GroupUserModel, bool>> ToViewModel
            (Expression<Func<GroupUserVM, bool>> GroupUser)
        {

            return Mapper.Map<Expression<Func<GroupUserVM, bool>>,
                Expression<Func<GroupUserModel, bool>>>(GroupUser);
        }
    }
}
