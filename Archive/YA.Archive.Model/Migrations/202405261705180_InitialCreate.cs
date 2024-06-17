namespace Idepardaz.Archive.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientCenter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Directory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        FolderId = c.Int(nullable: false),
                        FileName = c.String(),
                        OrginalName = c.String(),
                        FileSize = c.Int(nullable: false),
                        KeyWord = c.String(),
                        Description = c.String(),
                        Path = c.String(),
                        CreateUserId = c.String(maxLength: 128),
                        LastUpdateUserId = c.String(maxLength: 128),
                        CreateDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreateUserId)
                .ForeignKey("dbo.Folder", t => t.FolderId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdateUserId)
                .Index(t => t.FolderId)
                .Index(t => t.CreateUserId)
                .Index(t => t.LastUpdateUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientCenterId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientCenter", t => t.ClientCenterId)
                .Index(t => t.ClientCenterId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GroupUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Group", t => t.GroupId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreateDate = c.DateTime(nullable: false),
                        ValidityDate = c.DateTime(nullable: false),
                        ClientCenterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientCenter", t => t.ClientCenterId)
                .Index(t => t.ClientCenterId);
            
            CreateTable(
                "dbo.GroupClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Group", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Folder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Path = c.String(),
                        ClientCenterId = c.Int(nullable: false),
                        KeyWord = c.String(),
                        Description = c.String(),
                        CreateUserId = c.String(maxLength: 128),
                        LastUpdateUserId = c.String(maxLength: 128),
                        CreateDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                        EditLockDate = c.DateTime(nullable: false),
                        RemoveLockDate = c.DateTime(nullable: false),
                        EditFileLockDate = c.DateTime(nullable: false),
                        RemoveFileLockDate = c.DateTime(nullable: false),
                        EditCommentLockDate = c.DateTime(nullable: false),
                        RemoveCommentLockDate = c.DateTime(nullable: false),
                        IsEditLockDate = c.Boolean(nullable: false),
                        IsRemoveLockDate = c.Boolean(nullable: false),
                        IsEditFileLockDate = c.Boolean(nullable: false),
                        IsRemoveFileLockDate = c.Boolean(nullable: false),
                        IsEditCommentLockDate = c.Boolean(nullable: false),
                        IsRemoveCommentLockDate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientCenter", t => t.ClientCenterId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreateUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdateUserId)
                .Index(t => t.ClientCenterId)
                .Index(t => t.CreateUserId)
                .Index(t => t.LastUpdateUserId);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        FolderId = c.Int(),
                        Description = c.String(),
                        CreateUserId = c.String(maxLength: 128),
                        LastUpdateUserId = c.String(maxLength: 128),
                        CreateDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreateUserId)
                .ForeignKey("dbo.Folder", t => t.FolderId)
                .ForeignKey("dbo.AspNetUsers", t => t.LastUpdateUserId)
                .Index(t => t.FolderId)
                .Index(t => t.CreateUserId)
                .Index(t => t.LastUpdateUserId);
            
            CreateTable(
                "dbo.TypesOfFolder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FolderId = c.Int(nullable: false),
                        TypeofFolderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Folder", t => t.FolderId)
                .ForeignKey("dbo.TypeofFolder", t => t.TypeofFolderId)
                .Index(t => t.FolderId)
                .Index(t => t.TypeofFolderId);
            
            CreateTable(
                "dbo.TypeofFolder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TypesOfFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        TypeofFileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.File", t => t.FileId)
                .ForeignKey("dbo.TypeofFile", t => t.TypeofFileId)
                .Index(t => t.FileId)
                .Index(t => t.TypeofFileId);
            
            CreateTable(
                "dbo.TypeofFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportTitle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title1 = c.String(),
                        Title2 = c.String(),
                        Title3 = c.String(),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserLogTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProviderKey = c.String(),
                        UserId = c.String(maxLength: 128),
                        LoginTime = c.DateTime(nullable: false),
                        LogoutTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLogTime", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TypesOfFile", "TypeofFileId", "dbo.TypeofFile");
            DropForeignKey("dbo.TypesOfFile", "FileId", "dbo.File");
            DropForeignKey("dbo.File", "LastUpdateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TypesOfFolder", "TypeofFolderId", "dbo.TypeofFolder");
            DropForeignKey("dbo.TypesOfFolder", "FolderId", "dbo.Folder");
            DropForeignKey("dbo.Folder", "LastUpdateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.File", "FolderId", "dbo.Folder");
            DropForeignKey("dbo.Folder", "CreateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comment", "LastUpdateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comment", "FolderId", "dbo.Folder");
            DropForeignKey("dbo.Comment", "CreateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Folder", "ClientCenterId", "dbo.ClientCenter");
            DropForeignKey("dbo.File", "CreateUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUser", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupUser", "GroupId", "dbo.Group");
            DropForeignKey("dbo.GroupClaim", "GroupId", "dbo.Group");
            DropForeignKey("dbo.Group", "ClientCenterId", "dbo.ClientCenter");
            DropForeignKey("dbo.AspNetUsers", "ClientCenterId", "dbo.ClientCenter");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserLogTime", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TypesOfFile", new[] { "TypeofFileId" });
            DropIndex("dbo.TypesOfFile", new[] { "FileId" });
            DropIndex("dbo.TypesOfFolder", new[] { "TypeofFolderId" });
            DropIndex("dbo.TypesOfFolder", new[] { "FolderId" });
            DropIndex("dbo.Comment", new[] { "LastUpdateUserId" });
            DropIndex("dbo.Comment", new[] { "CreateUserId" });
            DropIndex("dbo.Comment", new[] { "FolderId" });
            DropIndex("dbo.Folder", new[] { "LastUpdateUserId" });
            DropIndex("dbo.Folder", new[] { "CreateUserId" });
            DropIndex("dbo.Folder", new[] { "ClientCenterId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.GroupClaim", new[] { "GroupId" });
            DropIndex("dbo.Group", new[] { "ClientCenterId" });
            DropIndex("dbo.GroupUser", new[] { "UserId" });
            DropIndex("dbo.GroupUser", new[] { "GroupId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "ClientCenterId" });
            DropIndex("dbo.File", new[] { "LastUpdateUserId" });
            DropIndex("dbo.File", new[] { "CreateUserId" });
            DropIndex("dbo.File", new[] { "FolderId" });
            DropTable("dbo.UserLogTime");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportTitle");
            DropTable("dbo.TypeofFile");
            DropTable("dbo.TypesOfFile");
            DropTable("dbo.TypeofFolder");
            DropTable("dbo.TypesOfFolder");
            DropTable("dbo.Comment");
            DropTable("dbo.Folder");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.GroupClaim");
            DropTable("dbo.Group");
            DropTable("dbo.GroupUser");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.File");
            DropTable("dbo.Directory");
            DropTable("dbo.ClientCenter");
        }
    }
}
