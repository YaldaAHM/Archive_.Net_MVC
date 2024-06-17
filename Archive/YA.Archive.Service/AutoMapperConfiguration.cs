using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.UserManagement;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs.Role;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.File;
using YA.Archive.Model.Folder;
using YA.Archive.Model.Group;
using YA.Archive.Model.GroupUser;
using YA.Archive.Service.ClientCenterMdl.VMs;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.VMs;
using YA.Archive.Model.TypeofFile;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Archive.Service.ReportTitleMdl.VMs;
using YA.Archive.Model.ReportTitle;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Service.ClientCenterAMdl.VMs;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;

namespace YA.Archive.Service
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            #region Mapping

            Mapper.Initialize(cfg =>
                //  {
                //  var b=  new MapperConfiguration(cfg =>
                              {
                                
                                  #region TypeOfFolder

                                  cfg.CreateMap<TypeofFolderModel, TypeofFolderVM>()
                                      .ReverseMap();
                                  cfg.CreateMap<TypesOfFolderModel, TypesOfFolderVM>()
                                     .ReverseMap();
                              
                                  #endregion

                                  #region TypeOfFile

                                  cfg.CreateMap<TypesOfFileModel, TypesOfFileVM>()
                                      .ReverseMap();
                                  cfg.CreateMap<TypeofFileModel, TypeofFileVM>()
                                      .ReverseMap();
                                  #endregion

                                  #region ClientCenter

                                  cfg.CreateMap<ClientCenterModel, ClientCenterVM>()
                                      .ReverseMap();

                                  #endregion

                                  #region ClientCenterA

                                  cfg.CreateMap<ClientCenterModel, ClientCenterAVM>()
                                      .PreserveReferences().ReverseMap();

                                  #endregion

                                  #region Group
                                  cfg.CreateMap<GroupModel, GroupExpVM>()
                                     .ReverseMap();
                                  cfg.CreateMap<GroupModel, GroupVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                      .ForMember(dest => dest.ValidityDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.ValidityDate))
                                          .PreserveReferences()
                                          ;
                                  cfg.CreateMap<GroupVM, GroupModel>()
                                     .ForMember(dest => dest.CreateDate,
                                         opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                     .ForMember(dest => dest.ValidityDate,
                                         opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.ValidityDate))
                                         .PreserveReferences()
                                         ;
                                  cfg.CreateMap<GroupClaimModel, GroupClaimVM>().PreserveReferences()
                                     ;
                                  cfg.CreateMap<GroupClaimVM, GroupClaimModel>().PreserveReferences();
                                
                                  cfg.CreateMap<GroupUserModel, GroupUserVM>()
                                    .ReverseMap().PreserveReferences();
                                  
                                  cfg.CreateMap<GroupVM, GroupExpVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                      .ForMember(dest => dest.ValidityDate,
                                        opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.ValidityDate))
                                      .PreserveReferences();
                                  cfg.CreateMap<GroupExpVM, GroupVM>()
                                     .ForMember(dest => dest.CreateDate,
                                         opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                     .ForMember(dest => dest.ValidityDate,
                                       opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.ValidityDate))
                                     .PreserveReferences();
                                  #endregion
                                  #region UserLogTime
                                  cfg.CreateMap<UserLogTimeModel, UserLogTimeExpVM>().PreserveReferences();
                                  cfg.CreateMap<UserLogTimeExpVM,UserLogTimeModel >().PreserveReferences();

                                  cfg.CreateMap<UserLogTimeModel, UserLogTimeVM>()
                                      .ForMember(dest => dest.LoginTime,
                                          opt => opt.ResolveUsing<GroToPersianDateTimeResolver, DateTime>(s => s.LoginTime))
                                      .ForMember(dest => dest.LogoutTime,
                                          opt => opt.ResolveUsing<GroToPersianDateTimeResolver, DateTime>(s => s.LogoutTime))
                                   
                                          .PreserveReferences();


                                  cfg.CreateMap<UserLogTimeVM, UserLogTimeModel>()
                                  .ForMember(dest => dest.LoginTime,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LoginTime))
                                  .ForMember(dest => dest.LogoutTime,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LogoutTime))
                                 
                                      .PreserveReferences();
                                  #endregion



                                  #region File

                                  cfg.CreateMap<FileModel, FileExpVM>().ReverseMap().PreserveReferences();
                                  
                                  cfg.CreateMap<FileModel, FileVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                      .ForMember(dest => dest.LastUpdateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))
                                      .PreserveReferences();


                                  cfg.CreateMap<FileVM, FileModel>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                 .PreserveReferences();
                                  cfg.CreateMap<FileExpVM, FileVM>()
                                     .ForMember(dest => dest.CreateDate,
                                         opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                     .ForMember(dest => dest.LastUpdateDate,
                                         opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))
                                     .PreserveReferences();


                                  cfg.CreateMap<FileVM, FileExpVM>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                 .PreserveReferences();
                                  #endregion


                                  #region Folder

                                  cfg.CreateMap<FolderModel, FolderExpVM>()
                                      .PreserveReferences();
                                  cfg.CreateMap<FolderExpVM,FolderModel >()
                                  
                                     .PreserveReferences();
                                  cfg.CreateMap<FolderModel, FolderVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                      .ForMember(dest => dest.LastUpdateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))
                                      .ForMember(dest => dest.EditLockDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.EditLockDate))
                                      .ForMember(dest => dest.RemoveLockDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.RemoveLockDate))
                                      .ForMember(dest => dest.EditCommentLockDate,
                                          opt =>
                                              opt.ResolveUsing<GroToPersianResolver, DateTime>(
                                                  s => s.EditCommentLockDate))
                                      .ForMember(dest => dest.RemoveCommentLockDate,
                                          opt =>
                                              opt.ResolveUsing<GroToPersianResolver, DateTime>(
                                                  s => s.RemoveCommentLockDate))
                                      .ForMember(dest => dest.EditFileLockDate,
                                          opt =>
                                              opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.EditFileLockDate))
                                      .ForMember(dest => dest.RemoveFileLockDate,
                                          opt =>
                                              opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.RemoveFileLockDate))

                                      .PreserveReferences();
                                  cfg.CreateMap<FolderVM, FolderExpVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                      .ForMember(dest => dest.LastUpdateDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                      .ForMember(dest => dest.EditLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.EditLockDate))
                                      .ForMember(dest => dest.RemoveLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.RemoveLockDate))
                                          .ForMember(dest => dest.EditCommentLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.EditCommentLockDate))
                                          .ForMember(dest => dest.RemoveCommentLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.RemoveCommentLockDate))
                                          .ForMember(dest => dest.EditFileLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.EditFileLockDate))
                                          .ForMember(dest => dest.RemoveFileLockDate,
                                          opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.RemoveFileLockDate))
                                       .ForMember(dest => dest.CreateUser,
                                         (s => s.MapFrom(v=>v.CreateUser)))
                                      .ForMember(dest => dest.LastUpdateUser,
                                         (s => s.MapFrom(v => v.LastUpdateUser)))
                                          .PreserveReferences();
                                  cfg.CreateMap<FolderExpVM, FolderVM>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))
                                  .ForMember(dest => dest.EditLockDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.EditLockDate))
                                  .ForMember(dest => dest.RemoveLockDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.RemoveLockDate))
                                  .ForMember(dest => dest.EditCommentLockDate,
                                      opt =>
                                          opt.ResolveUsing<GroToPersianResolver, DateTime>(
                                              s => s.EditCommentLockDate))
                                  .ForMember(dest => dest.RemoveCommentLockDate,
                                      opt =>
                                          opt.ResolveUsing<GroToPersianResolver, DateTime>(
                                              s => s.RemoveCommentLockDate))
                                  .ForMember(dest => dest.EditFileLockDate,
                                      opt =>
                                          opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.EditFileLockDate))
                                  .ForMember(dest => dest.RemoveFileLockDate,
                                      opt =>
                                          opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.RemoveFileLockDate))

                                  .PreserveReferences();
                                  cfg.CreateMap<FolderVM, FolderModel>()
                                   .ForMember(dest => dest.CreateDate,
                                       opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                   .ForMember(dest => dest.LastUpdateDate,
                                       opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                   .ForMember(dest => dest.EditLockDate,
                                       opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.EditLockDate))
                                   .ForMember(dest => dest.RemoveLockDate,
                                       opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.RemoveLockDate))
                                   .ForMember(dest => dest.EditCommentLockDate,
                                       opt =>
                                           opt.ResolveUsing<PersianToGroResolver, string>(
                                               s => s.EditCommentLockDate))
                                   .ForMember(dest => dest.RemoveCommentLockDate,
                                       opt =>
                                           opt.ResolveUsing<PersianToGroResolver, string>(
                                               s => s.RemoveCommentLockDate))
                                   .ForMember(dest => dest.EditFileLockDate,
                                       opt =>
                                           opt.ResolveUsing<PersianToGroResolver, string>(s => s.EditFileLockDate))
                                   .ForMember(dest => dest.RemoveFileLockDate,
                                       opt =>
                                           opt.ResolveUsing<PersianToGroResolver, string>(s => s.RemoveFileLockDate))

                                   .PreserveReferences();
                                  cfg.CreateMap<CommentModel, CommentExpVM>()
                                      .PreserveReferences();
                                  cfg.CreateMap<CommentExpVM,CommentModel >()
                                     .PreserveReferences();
                                  cfg.CreateMap<CommentVM, CommentExpVM>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                     .PreserveReferences();
                                  cfg.CreateMap<CommentExpVM, CommentVM>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))

                                     .PreserveReferences();
                                  cfg.CreateMap<CommentModel, CommentVM>()
                                      .ForMember(dest => dest.CreateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.CreateDate))
                                      .ForMember(dest => dest.LastUpdateDate,
                                          opt => opt.ResolveUsing<GroToPersianResolver, DateTime>(s => s.LastUpdateDate))
                                      .PreserveReferences();

                                  cfg.CreateMap<CommentVM, CommentModel>()
                                  .ForMember(dest => dest.CreateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.CreateDate))
                                  .ForMember(dest => dest.LastUpdateDate,
                                      opt => opt.ResolveUsing<PersianToGroResolver, string>(s => s.LastUpdateDate))
                                 .PreserveReferences();

                                 
                                  #endregion
                                  

                                  #region User
                                  cfg.CreateMap<UserModel, RegistrationVM>()
                                      .ReverseMap().PreserveReferences();
                                  cfg.CreateMap<UserModel, EditVM>()
                                      .ReverseMap().PreserveReferences();
                                  cfg.CreateMap<UserModel, UserExpVM>()
                                      .ReverseMap();
                                  cfg.CreateMap<UserModel, UserVM>().PreserveReferences()
                                      ;
                                  cfg.CreateMap<UserVM, UserModel>().PreserveReferences()
                                     ;
                                 
                                  cfg.CreateMap<RoleModel, RoleVM>().ReverseMap().PreserveReferences()
                                      ;
                                  cfg.CreateMap<PermissionClaimsModel, PermissionClaimsVM>()
                                  .ForMember(t => t.User, opt => opt.Ignore()).PreserveReferences();
                                  cfg.CreateMap<PermissionClaimsVM, PermissionClaimsModel>()
                                  .ForMember(t => t.User, opt => opt.Ignore()).PreserveReferences();
                                  #endregion


                                  #region ReportTitle

                                  cfg.CreateMap<ReportTitleModel, ReportTitleVM>()
                                      .ReverseMap();
                                  cfg.CreateMap<ReportTitleModel, ReportTitleVM>()
                                      .ReverseMap();
                                  #endregion
                              });
            

            #endregion


        }

       
        public class
            MoneyToStringResolver :
                IMemberValueResolver<object, object, decimal, string>
        {
            public
                string Resolve
                (object source, object destination, decimal sourceMember, string destMember, ResolutionContext context)
            {
                return sourceMember.ToString();
            }
        }

        public class
            StringToMoneyResolver :
                IMemberValueResolver<object, object, string, decimal>
        {
            public
                decimal Resolve
                (object source, object destination, string sourceMember, decimal destMember, ResolutionContext context)
            {
                decimal c = Convert.ToDecimal(sourceMember);
                return c;
            }
        }

        public class
            GroToPersianResolver :
                IMemberValueResolver<object, object, DateTime, string>
        {
            public
                string Resolve
                (object source, object destination, DateTime sourceMember, string destMember, ResolutionContext context)
            {
                return (fpub.convert2shamsi(sourceMember)).ToString("yyyy/MM/dd");
            }
        }
        public class
           GroToPersianDateTimeResolver :
               IMemberValueResolver<object, object, DateTime, string>
        {
            public
                string Resolve
                (object source, object destination, DateTime sourceMember, string destMember, ResolutionContext context)
            {
                return fpub.convert2shamsiSTime(sourceMember);
            }
        }
        public class
            PersianToGroResolver :
                IMemberValueResolver<object, object, string, DateTime>
        {
            public
                DateTime Resolve
                (object source, object destination, string sourceMember, DateTime destMember, ResolutionContext context)
            {
                return fpub.convert2miladi(sourceMember.ToString());
            }
        }
    }

    

}
