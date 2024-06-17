using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Mapping;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps
{
    public class PermissionReportClaimsService
    {
        private ArchiveDataContext _db;
        public PermissionReportClaimsService()
        {
            _db = new ArchiveDataContext();
        }
        public PermissionReportClaimsService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore(_db));
            }
        }


        public PermissionClaimsUpdateResponse Update(PermissionClaimsUpdateRequest request)
        {
            try
            {


                if (request.User != null)
                {
                    foreach (var p in request.Entities)
                    {

                        if (p.View)
                        {
                            if (UserManager.GetClaims(p.User.Id).FirstOrDefault(m => m.Type == p.ClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                            Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.ViewClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                           Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.ViewClaimId));
                        }

                        _db.SaveChanges();


                    }
                }
                return new PermissionClaimsUpdateResponse
                {
                    //Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
            catch (Exception ex)
            {

                return new PermissionClaimsUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    Entities = request.Entities

                };
            }
        }

        public PermissionClaimsFindAllResponse FindAll(PermissionClaimsFindAllRequest request)
        {
            var users = UserManager.Users;

            ClaimsIdentity user = new ClaimsIdentity();
            user = UserManager.CreateIdentity(request.User.ToModel(),
                   DefaultAuthenticationTypes.ApplicationCookie);
            var u = user.Claims;
            var c = UserManager.GetClaims(request.User.Id).ToList();
            var cc = request.User.Claims.ToList();
            PermissionClaimsReportsInitialize pc = new PermissionClaimsReportsInitialize();
            var lpc = pc.PermissionClaimsReportsInitializer();
            foreach (var item in lpc)
            {

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.ViewClaimId) != null)
                {
                    item.View = true;
                }

            }

            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
            };


        }

        public PermissionClaimsFindAllResponse FindUser(PermissionClaimsFindAllRequest request)
        {

            var cc = request.User.Claims.ToList();
            PermissionClaimsReportsInitialize pc = new PermissionClaimsReportsInitialize();
            var lpc = pc.PermissionClaimsReportsInitializer();
            foreach (var item in lpc)
            {

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                && x.ClaimValue == item.CreateClaimId) != null)
                {
                    item.Create = true;
                }
                //   if (user.HasClaim(x => x.Type == item.EditClaimId
                //&& x.Value == item.EditClaimId))
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.EditClaimId) != null)
                {
                    item.Edit = true;
                }
                //   if (user.HasClaim(x => x.Type == item.DeleteClaimId
                //&& x.Value == item.DeleteClaimId))
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.DeleteClaimId) != null)
                {
                    item.Delete = true;
                }
                //   if (user.HasClaim(x => x.Type == item.ViewClaimId
                //&& x.Value == item.ViewClaimId))
                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                   && x.ClaimValue == item.ViewClaimId) != null)
                {
                    item.View = true;
                }
                // lpc.FirstOrDefault(m=>m.ClaimId==item.ClaimType).Create=
            }

            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = PermissionClaimsMapper.ToViewModelList(lpc)
            };

        }

    }
}
