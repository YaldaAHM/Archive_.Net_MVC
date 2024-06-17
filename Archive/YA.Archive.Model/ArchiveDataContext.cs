using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model.ClientCenter;
using YA.Archive.Model.Directory;
using YA.Archive.Model.File;
using YA.Archive.Model.Folder;
using YA.Archive.Model.Group;
using YA.Archive.Model.GroupUser;
using YA.Archive.Model.ReportTitle;
using YA.Archive.Model.TypeofFile;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Model.UserActivity.UserLogTime;
using YA.Archive.Model.UserManagement;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Model
{
    public class ArchiveDataContext: IdentityDbContext<UserModel
        , IdentityRole, string,
        IdentityUserLogin, IdentityUserRole, ApplicationUserClaim>
    {
        public DbSet<FolderModel> Folder { get; set; }
        public DbSet<FileModel> File { get; set; }
        public DbSet<TypeofFolderModel> TypeofFolder { get; set; }
        public DbSet<TypeofFileModel> TypeofFile { get; set; }
        public DbSet<GroupModel> Group { get; set; }
        public DbSet<GroupUserModel> GroupUser { get; set; }
        public DbSet<ClientCenterModel> ClientCenter { get; set; }
        public DbSet<GroupClaimModel> GroupClaim { get; set; }
        public DbSet<DirectoryModel> Directory { get; set; }
        public DbSet<ReportTitleModel> ReportTitle { get; set; }
        public DbSet<UserLogTimeModel> UserLogTime { get; set; }

        public ArchiveDataContext() : base("DefaultConnection")
        {

        }

        static ArchiveDataContext()
        {
            Database.SetInitializer<ArchiveDataContext>(null);
        }
        public static ArchiveDataContext Create()
        {
            return new ArchiveDataContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }
    }

    public class IdentityDbInit : NullDatabaseInitializer<ArchiveDataContext>
    {
    }
}
