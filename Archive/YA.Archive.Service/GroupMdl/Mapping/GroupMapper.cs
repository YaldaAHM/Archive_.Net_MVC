using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.Group;
using YA.Archive.Service.GroupMdl.VMs;

namespace YA.Archive.Service.GroupMdl.Mapping
{
    public static class GroupMapper
    { 
     public static GroupModel ToModel(this GroupVM Group)
    {
        return Mapper.Map<GroupVM, GroupModel>(Group);
    }
    public static GroupVM ToViewModel(this GroupModel Group)
    {
        return Mapper.Map<GroupModel, GroupVM>(Group);
    }
    public static IEnumerable<GroupModel> ToModelList(this IEnumerable<GroupVM> Groups)
    {
        return Mapper.Map<IEnumerable<GroupVM>, List<GroupModel>>(Groups);
    }
    public static IEnumerable<GroupVM> ToViewModelList(this IEnumerable<GroupModel> Group)
    {
        return Mapper.Map<IEnumerable<GroupModel>, List<GroupVM>>(Group);
    }
        public static GroupModel ToModelExp(this GroupExpVM Group)
        {
            return Mapper.Map<GroupExpVM, GroupModel>(Group);
        }
        public static GroupExpVM ToViewModelExp(this GroupModel Group)
        {
            return Mapper.Map<GroupModel, GroupExpVM>(Group);
        }
        public static IEnumerable<GroupModel> ToModelListExp(this IEnumerable<GroupExpVM> Groups)
        {
            return Mapper.Map<IEnumerable<GroupExpVM>, List<GroupModel>>(Groups);
        }
        public static IEnumerable<GroupExpVM> ToViewModelListExp(this IEnumerable<GroupModel> Group)
        {
            return Mapper.Map<IEnumerable<GroupModel>, List<GroupExpVM>>(Group);
        }
        public static Expression<Func<GroupModel, bool>> ToViewModel
            (Expression<Func<GroupVM, bool>> Group)
        {

            return Mapper.Map<Expression<Func<GroupVM, bool>>,
                Expression<Func<GroupModel, bool>>>(Group);
        }



        public static GroupExpVM ToModelEVM(this GroupVM Group)
        {
            return Mapper.Map<GroupVM, GroupExpVM>(Group);
        }
        public static GroupVM ToViewModelEVM(this GroupExpVM Folder)
        {
            return Mapper.Map<GroupExpVM, GroupVM>(Folder);
        }
        public static IEnumerable<GroupExpVM> ToModelEVMList(this IEnumerable<GroupVM> Groups)
        {
            return Mapper.Map<IEnumerable<GroupVM>, List<GroupExpVM>>(Groups);
        }
        public static IEnumerable<GroupVM> ToViewModelEVMList(this IEnumerable<GroupExpVM> Groups)
        {
            return Mapper.Map<IEnumerable<GroupExpVM>, List<GroupVM>>(Groups);
        }
    }
}
