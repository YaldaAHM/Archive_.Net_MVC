namespace YA.Archive.Model.Migrations
{
    using YA.Archive.Infrastructure.Infrastructure.Permission;
    using YA.Archive.Model.ClientCenter;
    using YA.Archive.Model.UserManagement.Configuration;
    using YA.Archive.Model.UserManagement;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
     using Microsoft.AspNet.Identity;
    using ClaimsProvider = Infrastructure.Infrastructure.Permission.ClaimsProvider;

    internal sealed class Configuration : DbMigrationsConfiguration<YA.Archive.Model.ArchiveDataContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ArchiveDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            ArchiveDataContext _db = new ArchiveDataContext();
            var r = _db.ClientCenter.Add(new ClientCenterModel() { Name = ClientCenterR.MainCenterName });
            _db.SaveChanges();
            AppUserManager userMgr = new AppUserManager(new AppUserStore(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<RoleModel>(context));
            string roleName2 = RolesT.MainCenter;
            string roleName1 = RolesT.AdminClient;
            string roleName = RolesT.Administrators;
            string userName = "Admin";
            string password = "Admin123";
            string email = "admin@example.com";
            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new RoleModel(roleName));
            }
            if (!roleMgr.RoleExists(roleName1))
            {
                roleMgr.Create(new RoleModel(roleName1));
            }
            if (!roleMgr.RoleExists(roleName2))
            {
                roleMgr.Create(new RoleModel(roleName2));
            }
            string roleNameclient = RolesT.ClientUser;
            if (!roleMgr.RoleExists(roleNameclient))
            {
                roleMgr.Create(new RoleModel(roleNameclient));
            }
            if (!roleMgr.RoleExists(RolesT.MainCenterUsers))
            {
                roleMgr.Create(new RoleModel(RolesT.MainCenterUsers));
            }
            UserModel user = userMgr.FindByName(userName);
            if (user == null)
            {

                userMgr.Create(new UserModel
                {
                    UserName = userName,
                    Email = email,
                    ClientCenterId = r.Id,

                },
                password);

                user = userMgr.FindByName(userName);
            }
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }
            if (!userMgr.IsInRole(user.Id, roleName1))
            {
                userMgr.AddToRole(user.Id, roleName1);
            }
            if (!userMgr.IsInRole(user.Id, roleName2))
            {
                userMgr.AddToRole(user.Id, roleName2);
            }
            PermissionClaimsInitialize p = new PermissionClaimsInitialize();
            
            foreach (var c in p.PermissionClaimsInitializer())
            {
                if (userMgr.GetClaims(user.Id)
                                    .FirstOrDefault(m => m.Type == c.ClaimId && m.Type == c.CreateClaimId) == null)
                    userMgr.AddClaim(user.Id, ClaimsProvider.CreateClaim(c.ClaimId, c.CreateClaimId));
                if (userMgr.GetClaims(user.Id)
                                    .FirstOrDefault(m => m.Type == c.ClaimId && m.Type == c.EditClaimId) == null)
                    userMgr.AddClaim(user.Id, ClaimsProvider.CreateClaim(c.ClaimId, c.EditClaimId));
                if (userMgr.GetClaims(user.Id)
                                    .FirstOrDefault(m => m.Type == c.ClaimId && m.Type == c.DeleteClaimId) == null)
                    userMgr.AddClaim(user.Id, ClaimsProvider.CreateClaim(c.ClaimId, c.DeleteClaimId));
                if (userMgr.GetClaims(user.Id)
                                    .FirstOrDefault(m => m.Type == c.ClaimId && m.Type == c.ViewClaimId) == null)
                    userMgr.AddClaim(user.Id, ClaimsProvider.CreateClaim(c.ClaimId, c.ViewClaimId));
            }

        }
    }
}