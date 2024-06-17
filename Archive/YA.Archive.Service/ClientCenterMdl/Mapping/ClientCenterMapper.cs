using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Service.ClientCenterMdl.VMs;

namespace YA.Archive.Service.ClientCenterMdl.Mapping
{
    public static class ClientCenterMapper
    { 
     public static ClientCenterModel ToModel(this ClientCenterVM ClientCenter)
    {
        return Mapper.Map<ClientCenterVM, ClientCenterModel>(ClientCenter);
    }
    public static ClientCenterVM ToViewModel(this ClientCenterModel ClientCenter)
    {
        return Mapper.Map<ClientCenterModel, ClientCenterVM>(ClientCenter);
    }
    public static IEnumerable<ClientCenterModel> ToModelList(this IEnumerable<ClientCenterVM> ClientCenters)
    {
        return Mapper.Map<IEnumerable<ClientCenterVM>, List<ClientCenterModel>>(ClientCenters);
    }
    public static IEnumerable<ClientCenterVM> ToViewModelList(this IEnumerable<ClientCenterModel> ClientCenter)
    {
        return Mapper.Map<IEnumerable<ClientCenterModel>, List<ClientCenterVM>>(ClientCenter);
    }
       
        public static Expression<Func<ClientCenterModel, bool>> ToViewModel
            (Expression<Func<ClientCenterVM, bool>> ClientCenter)
        {

             return Mapper.Map<Expression<Func<ClientCenterVM, bool>>,
                Expression<Func<ClientCenterModel, bool>>>(ClientCenter);
        }
    }
}
