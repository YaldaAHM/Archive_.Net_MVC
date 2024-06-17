using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model.Folder;
using YA.Archive.Service.FolderMdl.VMs;

namespace YA.Archive.Service.FolderMdl.Mapping
{
    public static class FolderMapper
    { 
     public static FolderModel ToModel(this FolderVM Folder)
    {
        return Mapper.Map<FolderVM, FolderModel>(Folder);
    }
    public static FolderVM ToViewModel(this FolderModel Folder)
    {
        return Mapper.Map<FolderModel, FolderVM>(Folder);
    }
    public static IEnumerable<FolderModel> ToModelList(this IEnumerable<FolderVM> Folders)
    {
        return Mapper.Map<IEnumerable<FolderVM>, List<FolderModel>>(Folders);
    }
    public static IEnumerable<FolderVM> ToViewModelList(this IEnumerable<FolderModel> Folder)
    {
        return Mapper.Map<IEnumerable<FolderModel>, List<FolderVM>>(Folder);
    }

        public static TypesOfFolderModel ToModelTypesOfFolder(this TypesOfFolderVM TypesOfFolder)
        {
            return Mapper.Map<TypesOfFolderVM, TypesOfFolderModel>(TypesOfFolder);
        }
        public static TypesOfFolderVM ToViewModelTypesOf(this TypesOfFolderModel TypesOfFolder)
        {
            return Mapper.Map<TypesOfFolderModel, TypesOfFolderVM>(TypesOfFolder);
        }
        public static IEnumerable<TypesOfFolderModel> ToModelListTypesOfFolder(this IEnumerable<TypesOfFolderVM> TypesOfFolders)
        {
            return Mapper.Map<IEnumerable<TypesOfFolderVM>, List<TypesOfFolderModel>>(TypesOfFolders);
        }
        public static IEnumerable<TypesOfFolderVM> ToViewModelListTypesOfFolder(this IEnumerable<TypesOfFolderModel> TypesOfFolder)
        {
            return Mapper.Map<IEnumerable<TypesOfFolderModel>, List<TypesOfFolderVM>>(TypesOfFolder);
        }


        public static CommentModel ToModel(this CommentVM Comment)
        {
            return Mapper.Map<CommentVM, CommentModel>(Comment);
        }
        public static CommentVM ToViewModel(this CommentModel Comment)
        {
            return Mapper.Map<CommentModel, CommentVM>(Comment);
        }
        public static IEnumerable<CommentModel> ToModelList(this IEnumerable<CommentVM> Comment)
        {
            return Mapper.Map<IEnumerable<CommentVM>, List<CommentModel>>(Comment);
        }
        public static IEnumerable<CommentVM> ToViewModelList(this IEnumerable<CommentModel> Comment)
        {
            return Mapper.Map<IEnumerable<CommentModel>, List<CommentVM>>(Comment);
        }

        public static FolderModel ToModelExp(this FolderExpVM Folder)
        {
            return Mapper.Map<FolderExpVM, FolderModel>(Folder);
        }
        public static FolderExpVM ToViewModelExp(this FolderModel Folder)
        {
            return Mapper.Map<FolderModel, FolderExpVM>(Folder);
        }
        public static IEnumerable<FolderModel> ToModelListExp(this IEnumerable<FolderExpVM> Folders)
        {
            return Mapper.Map<IEnumerable<FolderExpVM>, List<FolderModel>>(Folders);
        }
        public static IEnumerable<FolderExpVM> ToViewModelListExp(this IEnumerable<FolderModel> Folder)
        {
            return Mapper.Map<IEnumerable<FolderModel>, List<FolderExpVM>>(Folder);
        }





        public static CommentModel ToModelExp(this CommentExpVM Comment)
        {
            return Mapper.Map<CommentExpVM, CommentModel>(Comment);
        }
        public static CommentExpVM ToViewModelExp(this CommentModel Comment)
        {
            return Mapper.Map<CommentModel, CommentExpVM>(Comment);
        }
        public static IEnumerable<CommentModel> ToModelListExp(this IEnumerable<CommentExpVM> Comment)
        {
            return Mapper.Map<IEnumerable<CommentExpVM>, List<CommentModel>>(Comment);
        }
        public static IEnumerable<CommentExpVM> ToViewModelListExp(this IEnumerable<CommentModel> Comment)
        {
            return Mapper.Map<IEnumerable<CommentModel>, List<CommentExpVM>>(Comment);
        }


        public static Expression<Func<FolderModel, bool>> ToViewModel
            (Expression<Func<FolderVM, bool>> Folder)
        {

            // Mapper.Map<Expression<Func<FoodVM, bool>>, Expression<Func<FoodModel, bool>>>(OrderedFolder);
            return Mapper.Map<Expression<Func<FolderVM, bool>>,
                Expression<Func<FolderModel, bool>>>(Folder);
        }
        public static Expression<Func<FolderExpVM>> ToViewModel
             (this Expression<Func<FolderModel>> Folder)
        {
            return Mapper.Map<Expression<Func<FolderModel>>,
                Expression<Func<FolderExpVM>>>(Folder);
        }
        public static Expression<Func<FolderModel>> ToViewModel
            (this Expression<Func<FolderExpVM>> Folder)
        {
            return Mapper.Map<Expression<Func<FolderExpVM>>, 
                Expression<Func<FolderModel>>>(Folder);
        }


        public static TypesOfFolderModel ToModel(this TypesOfFolderVM TypesOfFolder)
        {
            return Mapper.Map<TypesOfFolderVM, TypesOfFolderModel>(TypesOfFolder);
        }
        public static TypesOfFolderVM ToViewModel(this TypesOfFolderModel Folder)
        {
            return Mapper.Map<TypesOfFolderModel, TypesOfFolderVM>(Folder);
        }
        public static IEnumerable<TypesOfFolderModel> ToModelList(this IEnumerable<TypesOfFolderVM> TypesOfFolders)
        {
            return Mapper.Map<IEnumerable<TypesOfFolderVM>, List<TypesOfFolderModel>>(TypesOfFolders);
        }
        public static IEnumerable<TypesOfFolderVM> ToViewModelList(this IEnumerable<TypesOfFolderModel> TypesOfFolder)
        {
            return Mapper.Map<IEnumerable<TypesOfFolderModel>, List<TypesOfFolderVM>>(TypesOfFolder);
        }


        /////////////////////////
        //public static TypesOfFolderModel ToModelExp(this TypesOfFolderExpVM TypesOfFolder)
        //{
        //    return Mapper.Map<TypesOfFolderExpVM, TypesOfFolderModel>(TypesOfFolder);
        //}
        //public static TypesOfFolderExpVM ToViewModelExp(this TypesOfFolderModel Folder)
        //{
        //    return Mapper.Map<TypesOfFolderModel, TypesOfFolderExpVM>(Folder);
        //}
        //public static IEnumerable<TypesOfFolderModel> ToModelListExp(this IEnumerable<TypesOfFolderExpVM> TypesOfFolders)
        //{
        //    return Mapper.Map<IEnumerable<TypesOfFolderExpVM>, List<TypesOfFolderModel>>(TypesOfFolders);
        //}
        //public static IEnumerable<TypesOfFolderExpVM> ToViewModelListExp(this IEnumerable<TypesOfFolderModel> TypesOfFolder)
        //{
        //    return Mapper.Map<IEnumerable<TypesOfFolderModel>, List<TypesOfFolderExpVM>>(TypesOfFolder);
        //}
        /// </summary>
        /// <param name="Folder"></param>
        /// <returns></returns>
        //////////////////////////////////////
        public static FolderExpVM ToModelEVM(this FolderVM Folder)
        {
            return Mapper.Map<FolderVM, FolderExpVM>(Folder);
        }
        public static FolderVM ToViewModelEVM(this FolderExpVM Folder)
        {
            return Mapper.Map<FolderExpVM, FolderVM>(Folder);
        }
        public static IEnumerable<FolderExpVM> ToModelEVMList(this IEnumerable<FolderVM> Folders)
        {
            return Mapper.Map<IEnumerable<FolderVM>, List<FolderExpVM>>(Folders);
        }
        public static IEnumerable<FolderVM> ToViewModelEVMList(this IEnumerable<FolderExpVM> Folder)
        {
            return Mapper.Map<IEnumerable<FolderExpVM>, List<FolderVM>>(Folder);
        }
    }
}
