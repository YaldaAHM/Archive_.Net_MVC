using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.VMs;

namespace YA.Archive.Service.UserManagementMdl.Mapping
{
    public static class UserMapper
    { 
     public static UserModel ToModel(this UserVM User)
    {
        return Mapper.Map<UserVM, UserModel>(User);
    }
    public static UserVM ToViewModel(this UserModel User)
    {
        return Mapper.Map<UserModel, UserVM>(User);
    }
    public static IEnumerable<UserModel> ToModelList(this IEnumerable<UserVM> Users)
    {
        return Mapper.Map<IEnumerable<UserVM>, List<UserModel>>(Users);
    }
    public static IEnumerable<UserVM> ToViewModelList(this IEnumerable<UserModel> User)
    {
        return Mapper.Map<IEnumerable<UserModel>, List<UserVM>>(User);
    }
        public static UserModel ToModelExp(this UserExpVM User)
        {
            return Mapper.Map<UserExpVM, UserModel>(User);
        }
        public static UserExpVM ToViewModelExp(this UserModel User)
        {
            return Mapper.Map<UserModel, UserExpVM>(User);
        }
        public static IEnumerable<UserModel> ToModelListExp(this IEnumerable<UserExpVM> Users)
        {
            return Mapper.Map<IEnumerable<UserExpVM>, List<UserModel>>(Users);
        }
        public static IEnumerable<UserExpVM> ToViewModelListExp(this IEnumerable<UserModel> User)
        {
            return Mapper.Map<IEnumerable<UserModel>, List<UserExpVM>>(User);
        }
        public static Expression<Func<UserModel, bool>> ToViewModel
            (Expression<Func<UserVM, bool>> User)
        {
             return Mapper.Map<Expression<Func<UserVM, bool>>,
                Expression<Func<UserModel, bool>>>(User);
        }




        //////Registration
        public static UserModel ToModel(this RegistrationVM Registeration)
        {
            return Mapper.Map<RegistrationVM, UserModel>(Registeration);
        }
        public static RegistrationVM ToRegisterationViewModel(this UserModel Registeration)
        {
            return Mapper.Map<UserModel, RegistrationVM>(Registeration);
        }
        public static IEnumerable<UserModel> ToModelList(this IEnumerable<RegistrationVM> Registerations)
        {
            return Mapper.Map<IEnumerable<RegistrationVM>, List<UserModel>>(Registerations);
        }
        public static IEnumerable<RegistrationVM> ToRegisterationViewModelList(this IEnumerable<UserModel> Registeration)
        {
            return Mapper.Map<IEnumerable<UserModel>, List<RegistrationVM>>(Registeration);
        }
    }
}
