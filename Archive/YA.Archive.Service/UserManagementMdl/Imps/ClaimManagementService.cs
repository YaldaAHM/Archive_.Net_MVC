using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Model.UserManagement.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using Microsoft.AspNet.Identity;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Mapping;

namespace YA.Archive.Service.UserManagementMdl.Imps
{
    public class ClaimManagementService
    {
        private ArchiveDataContext _db;
        private FolderService _folderService;

        public ClaimManagementService()
        {
            _db = new ArchiveDataContext();
            _folderService = new FolderService();
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
                        if (p.Create)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.CreateClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.CreateClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.CreateClaimId));
                        }
                        if (p.Edit)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.EditClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.EditClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.EditClaimId));
                        }
                        if (p.Delete)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.DeleteClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.DeleteClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.DeleteClaimId));
                        }
                        if (p.View)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.ViewClaimId) == null)
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
            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            var lpc = pc.PermissionClaimsInitializer();
            foreach (var item in lpc)
            {

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.CreateClaimId) != null)
                {
                    item.Create = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.EditClaimId) != null)
                {
                    item.Edit = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.DeleteClaimId) != null)
                {
                    item.Delete = true;
                }

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
                Entities = lpc.ToViewModelList()
            };

        }

        public PermissionClaimsFindAllResponse FindUserClaims(PermissionClaimsFindAllRequest request)
        {

            var cc = request.User.Claims.ToList();
            PermissionClaimsInitialize pc = new PermissionClaimsInitialize();
            var lpc = pc.PermissionClaimsInitializer();
            foreach (var item in lpc)
            {

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.CreateClaimId) != null)
                {
                    item.Create = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.EditClaimId) != null)
                {
                    item.Edit = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.ClaimId
                                           && x.ClaimValue == item.DeleteClaimId) != null)
                {
                    item.Delete = true;
                }

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
                Entities = lpc.ToViewModelList()
            };


        }


        public PermissionClaimsFindAllResponse FindAllPermissionFolder(PermissionClaimsFindAllRequest request)
        {
            var responseFolder = _folderService
                .FindByExpression(new FolderFindByExpressionRequest()
                {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId()
                });
            var users = UserManager.Users;

            ClaimsIdentity user = new ClaimsIdentity();
            user = UserManager.CreateIdentity(request.User.ToModel(),
                DefaultAuthenticationTypes.ApplicationCookie);
            var u = user.Claims;
            var c = UserManager.GetClaims(request.User.Id).ToList();
            var cc = request.User.Claims.ToList();
            var permissionsModel = new List<PermissionClaimsModel>();

            foreach (var item in responseFolder.Entities)
            {
                var permissionModel = new PermissionClaimsModel();

                if (cc.FirstOrDefault(x => x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == item.Id.ToString() + "Create") != null)
                {
                    permissionModel.Create = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == item.Id.ToString() + "Edit") != null)
                {
                    permissionModel.Edit = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == item.Id.ToString() + "Delete") != null)
                {
                    permissionModel.Delete = true;
                }

                if (cc.FirstOrDefault(x => x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == item.Id.ToString() + "View") != null)
                {
                    permissionModel.View = true;
                }
                if (cc.FirstOrDefault(x => x.ClaimType == item.Id.ToString()
                                           && x.ClaimValue == item.Id.ToString() + "Lock") != null)
                {
                    permissionModel.View = true;
                }
                permissionsModel.Add(permissionModel);
            }

            return new PermissionClaimsFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = permissionsModel.ToViewModelList()
            };


        }

        public PermissionClaimsUpdateResponse UpdatePermissionFolder(PermissionClaimsUpdateRequest request)
        {
            try
            {

                if (request.User != null)
                {
                    foreach (var p in request.Entities)
                    {
                        if (p.Create)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.CreateClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.CreateClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.CreateClaimId));
                        }
                        if (p.Edit)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.EditClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.EditClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.EditClaimId));
                        }
                        if (p.Delete)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.DeleteClaimId) == null)
                                UserManager.AddClaim(request.User.Id,
                                    Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.DeleteClaimId));
                        }
                        else
                        {
                            UserManager.RemoveClaim(request.User.Id,
                                Model.UserManagement.Configuration.ClaimsProvider.CreateClaim(p.ClaimId, p.DeleteClaimId));
                        }
                        if (p.View)
                        {
                            if (
                                UserManager.GetClaims(p.User.Id)
                                    .FirstOrDefault(m => m.Type == p.ClaimId && m.Type == p.ViewClaimId) == null)
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
    }
}