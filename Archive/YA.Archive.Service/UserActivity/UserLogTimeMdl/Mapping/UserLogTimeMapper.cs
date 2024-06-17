using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;

namespace YA.Archive.Service.UserActivity.UserLogTimeMdl.Mapping
{
    public static class UserLogTimeMapper
    {
        public static UserLogTimeModel ToModel(this UserLogTimeVM UserLogTime)
        {
            return Mapper.Map<UserLogTimeVM, UserLogTimeModel>(UserLogTime);
        }
        public static UserLogTimeVM ToViewModel(this UserLogTimeModel UserLogTime)
        {
            return Mapper.Map<UserLogTimeModel, UserLogTimeVM>(UserLogTime);
        }
        public static IEnumerable<UserLogTimeModel> ToModelList(this IEnumerable<UserLogTimeVM> UserLogTimes)
        {
            return Mapper.Map<IEnumerable<UserLogTimeVM>, List<UserLogTimeModel>>(UserLogTimes);
        }
        public static IEnumerable<UserLogTimeVM> ToViewModelList(this IEnumerable<UserLogTimeModel> UserLogTime)
        {
            return Mapper.Map<IEnumerable<UserLogTimeModel>, List<UserLogTimeVM>>(UserLogTime);
        }

        public static Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeVM, bool>> UserLogTime)
        {

            return Mapper.Map<Expression<Func<UserLogTimeVM, bool>>,
                Expression<Func<UserLogTimeModel, bool>>>(UserLogTime);
        }




        public static UserLogTimeModel ToModelExp(this UserLogTimeExpVM UserLogTime)
        {
            return Mapper.Map<UserLogTimeExpVM, UserLogTimeModel>(UserLogTime);
        }
        public static UserLogTimeExpVM ToViewModelExp(this UserLogTimeModel UserLogTime)
        {
            return Mapper.Map<UserLogTimeModel, UserLogTimeExpVM>(UserLogTime);
        }
        public static IEnumerable<UserLogTimeModel> ToModelListExp(this IEnumerable<UserLogTimeExpVM> UserLogTime)
        {
            return Mapper.Map<IEnumerable<UserLogTimeExpVM>, List<UserLogTimeModel>>(UserLogTime);
        }
        public static IEnumerable<UserLogTimeExpVM> ToViewModelListExp(this IEnumerable<UserLogTimeModel> UserLogTime)
        {
            return Mapper.Map<IEnumerable<UserLogTimeModel>, List<UserLogTimeExpVM>>(UserLogTime);
        }


        public static Expression<Func<UserLogTimeModel, bool>> ToViewModel
            (Expression<Func<UserLogTimeExpVM, bool>> UserLogTime)
        {

            return Mapper.Map<Expression<Func<UserLogTimeExpVM, bool>>,
              Expression<Func<UserLogTimeModel, bool>>>(UserLogTime);
        }
    }
}