using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.TypeofFile;
using YA.Archive.Service.TypeofFileMdl.VMs;

namespace YA.Archive.Service.TypeofFileMdl.Mapping
{
    public static class TypeofFileMapper
    {
        public static TypeofFileModel ToModel(this TypeofFileVM TypeofFile)
        {
            return Mapper.Map<TypeofFileVM, TypeofFileModel>(TypeofFile);
        }
        public static TypeofFileVM ToViewModel(this TypeofFileModel TypeofFile)
        {
            return Mapper.Map<TypeofFileModel, TypeofFileVM>(TypeofFile);
        }
        public static IEnumerable<TypeofFileModel> ToModelList(this IEnumerable<TypeofFileVM> TypeofFiles)
        {
            return Mapper.Map<IEnumerable<TypeofFileVM>, List<TypeofFileModel>>(TypeofFiles);
        }
        public static IEnumerable<TypeofFileVM> ToViewModelList(this IEnumerable<TypeofFileModel> TypeofFile)
        {
            return Mapper.Map<IEnumerable<TypeofFileModel>, List<TypeofFileVM>>(TypeofFile);
        }

        public static Expression<Func<TypeofFileModel, bool>> ToViewModel
            (Expression<Func<TypeofFileVM, bool>> TypeofFile)
        {
            return Mapper.Map<Expression<Func<TypeofFileVM, bool>>,
                           Expression<Func<TypeofFileModel, bool>>>(TypeofFile);
        }
    }
}
