using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.File;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.Mapping
{
    public static class FileMapper
    { 
     public static FileModel ToModel(this FileVM File)
    {
        return Mapper.Map<FileVM, FileModel>(File);
    }
    public static FileVM ToViewModel(this FileModel File)
    {
        return Mapper.Map<FileModel, FileVM>(File);
    }
    public static IEnumerable<FileModel> ToModelList(this IEnumerable<FileVM> Files)
    {
        return Mapper.Map<IEnumerable<FileVM>, List<FileModel>>(Files);
    }
    public static IEnumerable<FileVM> ToViewModelList(this IEnumerable<FileModel> File)
    {
        return Mapper.Map<IEnumerable<FileModel>, List<FileVM>>(File);
    }
        public static FileModel ToModelExp(this FileExpVM File)
        {
            return Mapper.Map<FileExpVM, FileModel>(File);
        }
        public static FileExpVM ToViewModelExp(this FileModel File)
        {
            return Mapper.Map<FileModel, FileExpVM>(File);
        }
        public static IEnumerable<FileModel> ToModelListExp(this IEnumerable<FileExpVM> Files)
        {
            return Mapper.Map<IEnumerable<FileExpVM>, List<FileModel>>(Files);
        }
        public static IEnumerable<FileExpVM> ToViewModelListExp(this IEnumerable<FileModel> File)
        {
            return Mapper.Map<IEnumerable<FileModel>, List<FileExpVM>>(File);
        }


        public static FileExpVM ToModelExp(this FileVM File)
        {
            return Mapper.Map<FileVM, FileExpVM>(File);
        }
        public static FileVM ToViewModelExp(this FileExpVM File)
        {
            return Mapper.Map<FileExpVM, FileVM>(File);
        }
        public static IEnumerable<FileExpVM> ToModelListExp(this IEnumerable<FileVM> Files)
        {
            return Mapper.Map<IEnumerable<FileVM>, List<FileExpVM>>(Files);
        }
        public static IEnumerable<FileVM> ToViewModelListExp(this IEnumerable<FileExpVM> File)
        {
            return Mapper.Map<IEnumerable<FileExpVM>, List<FileVM>>(File);
        }
        public static Expression<Func<FileModel, bool>> ToViewModel
            (Expression<Func<FileVM, bool>> File)
        {

             return Mapper.Map<Expression<Func<FileVM, bool>>,
                Expression<Func<FileModel, bool>>>(File);
        }



        public static TypesOfFileModel ToModel(this TypesOfFileVM TypesOfFile)
        {
            return Mapper.Map<TypesOfFileVM, TypesOfFileModel>(TypesOfFile);
        }
        public static TypesOfFileVM ToViewModel(this TypesOfFileModel TypesOfFile)
        {
            return Mapper.Map<TypesOfFileModel, TypesOfFileVM>(TypesOfFile);
        }
        public static IEnumerable<TypesOfFileModel> ToModelList(this IEnumerable<TypesOfFileVM> TypesOfFiles)
        {
            return Mapper.Map<IEnumerable<TypesOfFileVM>, List<TypesOfFileModel>>(TypesOfFiles);
        }
        public static IEnumerable<TypesOfFileVM> ToViewModelList(this IEnumerable<TypesOfFileModel> TypesOfFile)
        {
            return Mapper.Map<IEnumerable<TypesOfFileModel>, List<TypesOfFileVM>>(TypesOfFile);
        }
    }
}
