using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Service.TypeofFolderMdl.VMs;

namespace YA.Archive.Service.TypeofFolderMdl.Mapping
{
    public static class TypeofFolderMapper
    { 
     public static TypeofFolderModel ToModel(this TypeofFolderVM TypeofFolder)
    {
        return Mapper.Map<TypeofFolderVM, TypeofFolderModel>(TypeofFolder);
    }
    public static TypeofFolderVM ToViewModel(this TypeofFolderModel TypeofFolder)
    {
        return Mapper.Map<TypeofFolderModel, TypeofFolderVM>(TypeofFolder);
    }
    public static IEnumerable<TypeofFolderModel> ToModelList(this IEnumerable<TypeofFolderVM> TypeofFolders)
    {
        return Mapper.Map<IEnumerable<TypeofFolderVM>, List<TypeofFolderModel>>(TypeofFolders);
    }
    public static IEnumerable<TypeofFolderVM> ToViewModelList(this IEnumerable<TypeofFolderModel> TypeofFolder)
    {
        return Mapper.Map<IEnumerable<TypeofFolderModel>, List<TypeofFolderVM>>(TypeofFolder);
    }
       
        public static Expression<Func<TypeofFolderModel, bool>> ToViewModel
            (Expression<Func<TypeofFolderVM, bool>> TypeofFolder)
        {

             return Mapper.Map<Expression<Func<TypeofFolderVM, bool>>,
                Expression<Func<TypeofFolderModel, bool>>>(TypeofFolder);
        }
    }
}
