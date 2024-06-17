using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterAMdl.VMs;

namespace YA.Archive.Service.ClientCenterAMdl.Mapping
{
    public static class ClientCenterAMapper
    { 
     public static ClientCenterModel ToModel(this ClientCenterAVM ClientCenter)
    {
        return Mapper.Map<ClientCenterAVM, ClientCenterModel>(ClientCenter);
    }
    public static ClientCenterAVM ToViewModel(this ClientCenterModel ClientCenter)
    {
        return Mapper.Map<ClientCenterModel, ClientCenterAVM>(ClientCenter);
    }
    public static IEnumerable<ClientCenterModel> ToModelList(this IEnumerable<ClientCenterAVM> ClientCenters)
    {
        return Mapper.Map<IEnumerable<ClientCenterAVM>, List<ClientCenterModel>>(ClientCenters);
    }
    public static IEnumerable<ClientCenterAVM> ToViewModelList(this IEnumerable<ClientCenterModel> ClientCenter)
    {
        return Mapper.Map<IEnumerable<ClientCenterModel>, List<ClientCenterAVM>>(ClientCenter);
    }
       
        public static Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterAVM, bool>> ClientCenter)
        {
   return Mapper.Map<Expression<Func<ClientCenterAVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }
    }
}
