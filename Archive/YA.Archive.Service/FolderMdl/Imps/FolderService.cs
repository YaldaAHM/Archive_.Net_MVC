using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Archive.Infrastructure.Localization.MessageLocalization;
using YA.Archive.Infrastructure.Localization.ProjectLocalization;
using YA.Archive.Model;
using YA.Archive.Model.Folder;
using YA.Archive.Service.DirectoryMdl.Imps;
using YA.Archive.Service.DirectoryMdl.VMs;
using YA.Archive.Service.FolderMdl.Mapping;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.Sterategy;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.CommentLocalization;
using YA.Archive.Service.ClientCenterStrategy;
using YA.Localization.AccessLocalize;
using YA.Archive.Service.FolderMdl.Access;
using YA.Archive.Service.UserManagementMdl.Mapping;

namespace YA.Archive.Service.FolderMdl.Imps
{
    public class FolderService
    {
        private ArchiveDataContext _db;
        private DirectoryService _directoryService;

        private RoleManagementService _roleService;
        private PermissionClaimsService _permissionClaimsService;
        private PermissionClaimsProjectService _permissionClaimsProjectService;
        private IClientCenterStrategy _clientCenterStrategy = new MainClientCenterStrategy();

        public FolderService()
        {
            _db = new ArchiveDataContext();
            _directoryService = new DirectoryService();

            _roleService = new RoleManagementService();
            _permissionClaimsService = new PermissionClaimsService();


        }

        public FolderService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public void SetClientCenterStrategyTo(IClientCenterStrategy clientCenterStrategy)
        {
            _clientCenterStrategy = clientCenterStrategy;
        }
        public string GenerateProjectCode()
        {
            string code = "";
            var date = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd").Substring(2, 2);
            if (!_db.Folder.ToList().Any())
            {
                code = (int.Parse(date)).ToString() + "0010";
                return code;
            }


            var folder = _db.Folder.OrderByDescending(f => f.Id);

            var oldCode = folder.FirstOrDefault().Code;
            if (oldCode.Substring(0, 2) == date)
            {
                string c = (int.Parse(oldCode.Substring(2)) + 1).ToString();
                if (c.Length == 2)
                    c = "00" + c;
                else if (c.Length == 3)
                    c = "0" + c;
                code = date + c;
            }
            else
            {

            }
            return code;
        }
        public string GenerateCommentCode()
        {
            string code;
            var folderr = _db.Folder;
            int i = 0;
            var p = new CommentModel();
            foreach (var f in folderr)
            {
                if (i == 0)
                {
                    p = f.Comments.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                }
                else
                {
                    if (f.Comments.OrderByDescending(x => x.CreateDate).FirstOrDefault().CreateDate
                        >= p.CreateDate)
                    {
                        p = f.Comments.OrderByDescending(x => x.CreateDate).FirstOrDefault();
                    }
                }

            }

            var oldCode = p.Code;
            var date = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd").Substring(2, 2);
            if (oldCode.Substring(0, 2) == date)
            {
                string c = (int.Parse(oldCode.Substring(2)) + 1).ToString();
                if (c.Length == 2)
                    c = "00" + c;
                else if (c.Length == 3)
                    c = "0" + c;
                code = date + c;
            }
            else
            {
                code = (int.Parse(date) + 1).ToString() + "0010";
            }
            return code;
        }
        public FolderInsertResponse Insert(FolderInsertRequest request)
        {
            try
            {
                var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                    ? request.entity.ClientCenterId
                    : cl.ClientCenterId;
                if (_db.Folder.Any(n => n.Code.Substring(0, 2) == request.entity.Code.Substring(0, 2)
                                        && n.Name == request.entity.Name))
                {
                    return new FolderInsertResponse
                    {
                        IsSuccess = false,
                        Message = String.Format(MessageResource.Exist, ProjectResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                if (_db.Folder.Any(n => n.Name == request.entity.Name))
                {
                    return new FolderInsertResponse
                    {
                        IsSuccess = false,
                        Message = String.Format(MessageResource.Exist, ProjectResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                request.entity.CreateDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                request.entity.LastUpdateDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                request.entity.EditLockDate = request.entity.EditLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                request.entity.RemoveLockDate = request.entity.RemoveLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                request.entity.EditCommentLockDate = request.entity.EditCommentLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                request.entity.RemoveCommentLockDate = request.entity.RemoveCommentLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                request.entity.EditFileLockDate = request.entity.EditFileLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                request.entity.RemoveFileLockDate = request.entity.RemoveFileLockDate ??
                    fpub.convert2shamsiS(DateTime.Now);
                var responseDirectory =
                       _directoryService.FindAll(new DirectoryVM());
                if (responseDirectory == null || responseDirectory.Count == 0)
                {
                    return new FolderInsertResponse
                    {
                        Message = String.Format(FileMessageResource.NotPathExist),
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }
                var d = request.entity.ToModel();
                d.Path = responseDirectory.OrderByDescending(dp => dp.Id).FirstOrDefault().Name;
                request.entity.TypesOfFolders.ToList();

                d.TypesOfFolders = request.entity.TypesOfFolders.ToModelListTypesOfFolder().ToList();

                d.Code = GenerateProjectCode();
                if (_db.Folder.Any(n => n.Code == d.Code))
                {
                    d.Code = GenerateProjectCode();
                }

                var r = _db.Folder.Add(d);
                var rr = _db.SaveChanges();
                var bdirectory = _directoryService.CreateDirectory(r.Code.Substring(0, 2), r.Code);
                if (!bdirectory)
                {
                    _db.Folder.Remove(r);
                    return new FolderInsertResponse
                    {
                        Message = String.Format(MessageResource.InsertField),
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }


                var users = _roleService.FindUsersInRoleName(
                    new RoleFindUserRequest() { Id = RolesT.AdminClient });
                if (!users.Result.IsSuccess || users.Result.Entities == null || users.Result.Entities.Count() == 0)
                {
                    _db.Folder.Remove(r);
                    _directoryService.DeleteDirectory(r.Path, r.Code.Substring(0, 2), r.Code);
                    return new FolderInsertResponse
                    {
                        Message = String.Format(MessageResource.InsertField),
                        ResponseType = ResponseType.Error,
                        entity = request.entity
                    };
                }
                foreach (var u in users.Result.Entities.Where(x => x.ClientCenterId == request.entity.ClientCenterId))
                {
                    _permissionClaimsProjectService = new PermissionClaimsProjectService(_db);
                    _permissionClaimsProjectService.Insert(r.Id, u.Id, null);
                }
                var type = _db.Folder.FirstOrDefault(n => n.Id == r.Id).TypesOfFolders;

                var e = _db.Folder.OrderByDescending(a => a.Id).FirstOrDefault();
                var uu = r.ToViewModel();
                uu.TypesOfFolders = type.ToViewModelList().ToList();
                request.entity.Id = e.Id;
                foreach (var t in r.TypesOfFolders)
                {
                    var tt = _db.TypeofFolder.FirstOrDefault(x => x.Id == t.TypeofFolderId);
                    t.TypeofFolder = tt;
                }
                uu = r.ToViewModel();
                var cu = _db.Users.FirstOrDefault(x => x.Id == r.CreateUserId);
                var lu = _db.Users.FirstOrDefault(x => x.Id == r.LastUpdateUserId);
                uu.CreateUser = new UserVM() { UserName = cu.UserName, Id = cu.Id };
                uu.LastUpdateUser = new UserVM() { UserName = lu.UserName, Id = lu.Id };
                var entity = ApplyAccess.ApplyFolderAccess(uu, request.RequsetCurrentUserId);
                return new FolderInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity,// request.entity
                };

            }
            catch (Exception ex)
            {
                string mm = ex.InnerException.Message;
                mm = mm + "...................." + ex.InnerException.InnerException.Message;
                mm = mm + "...................." + ex.Message;
                return new FolderInsertResponse
                {
                    Message = mm,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public FolderUpdateResponse Update(FolderUpdateRequest request)
        {
            try
            {
                var c = _db.Folder.FirstOrDefault(n => n.Id == request.entity.Id);
                if (c.EditLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId
                      )
                    return new FolderUpdateResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };


                if (c != null)
                {
                    var cl = ChooseClientCenter.ClientCenterId(request.RequsetCurrentUserId);
                    request.entity.ClientCenterId = cl.ClientCenterType == ClientCenterType.Main
                        ? request.entity.ClientCenterId
                        : cl.ClientCenterId;
                    if (_db.Folder.Any(n => n.Name == request.entity.Name && n.Id != request.entity.Id))
                    {
                        return new FolderUpdateResponse
                        {
                            IsSuccess = false,
                            Message = String.Format(MessageResource.Exist, ProjectResource.Name),
                            ResponseType = ResponseType.Warning,
                            entity = request.entity
                        };
                    }

                    bool tt = false;

                    foreach (var t in c.TypesOfFolders)
                    {
                        if (request.entity.TypesOfFolders.Exists(v => v.TypeofFolderId == t.TypeofFolderId &&
                                                                      v.FolderId == t.FolderId) == false)
                        {
                            tt = true;
                            break;
                        }
                    }
                    if (tt == false)
                    {
                        foreach (var t in request.entity.TypesOfFolders)
                        {
                            if (c.TypesOfFolders.Exists(v => v.TypeofFolderId == t.TypeofFolderId &&
                                                             v.FolderId == t.FolderId) == false)
                            {
                                tt = true;
                                break;
                            }
                        }
                    }
                    if (c.ClientCenterId != request.entity.ClientCenterId ||
                        (request.entity.Description != null && c.Description != request.entity.Description) ||
                        (request.entity.KeyWord != null && c.KeyWord != request.entity.KeyWord) ||
                        (request.entity.Name != null && c.Name != request.entity.Name) ||
                        (request.entity.TypesOfFolders != null &&
                         c.TypesOfFolders != request.entity.TypesOfFolders.ToModelList())

                        || c.EditLockDate == fpub.convert2miladi(request.entity.EditLockDate)
                        || c.RemoveLockDate == fpub.convert2miladi(request.entity.RemoveLockDate)
                        || c.EditFileLockDate == fpub.convert2miladi(request.entity.EditFileLockDate)
                        || c.RemoveFileLockDate == fpub.convert2miladi(request.entity.RemoveFileLockDate)
                        || c.EditCommentLockDate == fpub.convert2miladi(request.entity.EditCommentLockDate)
                        || c.RemoveCommentLockDate == fpub.convert2miladi(request.entity.RemoveCommentLockDate)
                        )
                    {


                        c.Name = request.entity.Name;
                        c.ClientCenterId = request.entity.ClientCenterId;
                        c.Description = request.entity.Description;
                        c.KeyWord = request.entity.KeyWord;
                        c.TypesOfFolders.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                        c.TypesOfFolders = request.entity.TypesOfFolders.ToModelList().ToList();
                        c.LastUpdateDate = DateTime.Now;
                        c.LastUpdateUserId = request.entity.LastUpdateUserId;
                        c.CreateUserId = c.CreateUserId;
                        c.CreateDate = c.CreateDate;

                        c.EditLockDate = fpub.convert2miladi(request.entity.EditLockDate);

                        c.RemoveLockDate = fpub.convert2miladi(request.entity.RemoveLockDate);

                        c.EditFileLockDate = fpub.convert2miladi(request.entity.EditFileLockDate);

                        c.RemoveFileLockDate = fpub.convert2miladi(request.entity.RemoveFileLockDate);

                        c.EditCommentLockDate = fpub.convert2miladi(request.entity.EditCommentLockDate);

                        c.RemoveCommentLockDate = fpub.convert2miladi(request.entity.RemoveCommentLockDate);

                    }

                    _db.Entry(c).State = EntityState.Detached;
                    _db.Entry(c).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                var entity = ApplyAccess.ApplyFolderAccess(c.ToViewModel(), request.RequsetCurrentUserId);
                return new FolderUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity
                };

            }
            catch (Exception ex)
            {

                return new FolderUpdateResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public FolderDeleteResponse DeleteById(FolderDeleteByIdRequest request)
        {
            try
            {
                var c = _db.Folder.Find(request.Id);
                if (c.RemoveLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId

                   )
                    return new FolderDeleteResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,

                    };

                if (c == null)
                {
                    return new FolderDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ProjectResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.File.Any(b => b.FolderId == request.Id) ||
                    _db.Folder.Any(b => b.Comments.Any(cc => cc.FolderId == request.Id)))
                {
                    return new FolderDeleteResponse
                    {
                        Message = String.Format(ProjectResource.UsedProject, ProjectResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }

                c.Comments.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                c.Files.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                c.TypesOfFolders.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);

                _permissionClaimsService.DeleteByClaimType(c.Id.ToString());
                _directoryService.DeleteDirectory(c.Path, c.Code.Substring(0, 2), c.Code);
                _db.Folder.Remove(c);
                _db.SaveChanges();
                return new FolderDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new FolderDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public FolderDeleteResponse Delete(FolderDeleteRequest request)
        {
            try
            {
                var c = _db.Folder.Find(request.entity.Id);
                if (c.RemoveLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId

                    )
                    return new FolderDeleteResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,

                    };


                if (c == null)
                {
                    return new FolderDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, ProjectResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.File.Any(b => b.FolderId == request.entity.Id) ||
                    _db.Folder.Any(b => b.Comments.Any(cc => cc.FolderId == request.entity.Id)))
                {
                    return new FolderDeleteResponse
                    {
                        Message = String.Format(ProjectResource.UsedProject, ProjectResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
                c.Comments.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                c.TypesOfFolders.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                _db.Folder.Remove(c);
                _db.SaveChanges();
                return new FolderDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new FolderDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }

        public FolderFindAllResponse FindAll(FolderFindAllRequest request)
        {

            return new FolderFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,

                Entities = ChooseSterategy.FolderFindAll(request).Entities// c.Entities,
            };

        }

        public FolderFindAllResponse FindAllPaging(FolderFindAllRequest request)
        {

            var res = ChooseSterategy.FolderFindAllPaging(request);
            return new FolderFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,

                Entities = res.Entities,
                TotalRecords = res.TotalRecords,
                Count = res.Count,
                Index = res.Index
            };


        }
        public FolderFindByIdResponse FindById(FolderFindByIdRequest request)
        {
            try
            {

                return new FolderFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = _db.Folder.Find(request.Id).ToViewModel()
                };

            }
            catch (Exception ex)
            {

                return new FolderFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public FolderFindByExpressionResponse FindByExpression(FolderFindByExpressionRequest request)
        {
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FolderFindByExpression(
                    new FolderFindByExpressionExpRequest()
                    {
                        RequsetCurrentUserId = request.RequsetCurrentUserId,
                        RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,

                    }).Entities


            };


        }
        public FolderFindByExpressionResponse FindByExpression(FolderFindByExpressionExpRequest request)
        {

            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FolderFindByExpression(request).Entities// c.Entities

            };


        }

        public FolderFindByExpressionResponse FindByExpressionPaging(FolderFindByExpressionExpRequest request)
        {

            var res = ChooseSterategy.FolderFindByExpressionPaging(request);
            return new FolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = res.Entities,// c.Entities
                TotalRecords = res.TotalRecords,
                Count = res.Count,
                Index = res.Index

            };

        }
        public Expression<Func<FolderModel, bool>> ToViewModel
            (Expression<Func<FolderVM, bool>> Folder)
        {
            return Mapper.Map<Expression<Func<FolderVM, bool>>,
                          Expression<Func<FolderModel, bool>>>(Folder);
        }


        public FolderFindByExpressionExpResponse FindByExpressionExp(FolderFindByExpressionExpRequest request)
        {

            return new FolderFindByExpressionExpResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FolderFindByExpression(request).Entities.ToModelEVMList()//c.Entities.ToModelEVMList()
                                                                                                    //  entity = _db.ClientCenter.Find(request.Id).ToViewModel()

            };


        }

        public Expression<Func<FolderModel, bool>> ToViewModel
            (Expression<Func<FolderExpVM, bool>> Folder)
        {

            return Mapper.Map<Expression<Func<FolderExpVM, bool>>,
               Expression<Func<FolderModel, bool>>>(Folder);
        }






        public FolderInsertResponse InsertComment(FolderInsertRequest request)
        {
            try
            {
                var c = _db.Folder.FirstOrDefault(n => n.Id == request.entity.Id);

                if (request.entity.Comments != null)
                {
                    foreach (var cm in request.entity.Comments)
                    {
                        if (_db.Folder.Any(n => n.Comments.Any(b => b.Description == cm.Description)))
                        {
                            return new FolderInsertResponse
                            {
                                IsSuccess = false,
                                Message = String.Format(MessageResource.Exist, CommentResource.Description),
                                ResponseType = ResponseType.Warning,
                                entity = request.entity
                            };
                        }
                        var mm = cm.ToModel();
                        Random r = new Random();
                        var gCode = "";
                        var folder = FindById(new FolderFindByIdRequest() { Id = request.entity.Id });

                        if (folder.entity.Comments == null || folder.entity.Comments.Count == 0)
                        {
                            gCode = folder.entity.Code.Substring(0, 2) + "0001";
                        }
                        else
                        {
                            gCode = folder.entity.Comments.OrderByDescending(x => x.Id).FirstOrDefault().Code;
                            gCode = (int.Parse(gCode) + 1).ToString();
                        }
                        mm.Code = gCode;
                        mm.CreateDate = DateTime.Now;
                        mm.LastUpdateDate = DateTime.Now;
                        mm.CreateUserId = cm.CreateUserId;
                        mm.LastUpdateUserId = cm.CreateUserId;
                        c.Comments.Add(mm);


                    }
                    _db.SaveChanges();

                }
                var entity = ApplyAccess.ApplyFolderAccess(c.ToViewModel(), request.RequsetCurrentUserId);
                return new FolderInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity
                };

            }
            catch (Exception ex)
            {

                return new FolderInsertResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }
        public FolderUpdateResponse UpdateComment2(FolderUpdateRequest request)
        {
            try
            {
                var CommentId = request.entity.Comments.FirstOrDefault().Id;
                var c = _db.Folder.FirstOrDefault(n => n.Comments.Any() && n.Comments.Any(x => x.Id ==
                CommentId));
                if (c.EditCommentLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId

                    )
                    return new FolderUpdateResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                if (request.entity.Comments != null)
                {
                    foreach (var cm in request.entity.Comments)
                    {
                        if (_db.Folder.Any(n => n.Comments.Any(b => b.Description == cm.Description)))
                        {
                            return new FolderUpdateResponse
                            {
                                IsSuccess = false,
                                Message = String.Format(MessageResource.Exist, CommentResource.Description),
                                ResponseType = ResponseType.Warning,
                                entity = request.entity
                            };
                        }
                        var comment = c.Comments.FirstOrDefault(x => x.Id == cm.Id);

                        comment.Description = cm.Description;
                        comment.LastUpdateDate = DateTime.Now;
                        comment.LastUpdateUserId = cm.LastUpdateUserId;
                        comment.FolderId = cm.FolderId;


                    }
                    _db.SaveChanges();
                }
                var entity = ApplyAccess.ApplyFolderAccess(c.ToViewModel(), request.RequsetCurrentUserId);
                return new FolderUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity
                };

            }
            catch (Exception ex)
            {

                return new FolderUpdateResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public FolderUpdateResponse UpdateComment(FolderUpdateRequest request)
        {
            try
            {
                var c = _db.Folder.FirstOrDefault(n => n.Id == request.entity.Id);
                if (c.EditCommentLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId

                    )
                    return new FolderUpdateResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                if (request.entity.Comments != null)
                {
                    foreach (var cm in request.entity.Comments)
                    {
                        var comment = c.Comments.FirstOrDefault(x => x.Id == cm.Id);

                        comment.Description = cm.Description;
                        comment.LastUpdateDate = DateTime.Now;
                        comment.LastUpdateUserId = cm.LastUpdateUserId;


                    }
                    _db.SaveChanges();
                }
                var entity = ApplyAccess.ApplyFolderAccess(c.ToViewModel(), request.RequsetCurrentUserId);
                return new FolderUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = entity
                };

            }
            catch (Exception ex)
            {

                return new FolderUpdateResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public FolderDeleteResponse DeleteComment(FolderDeleteRequest request)
        {
            try
            {
                var c = _db.Folder.Find(request.entity.Id);
                if (c.EditCommentLockDate < DateTime.Now && request.RequsetCurrentUserId != c.CreateUserId

                    )
                    return new FolderDeleteResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        //entity = request.entity
                    };
                c.Comments.Where(x => x.FolderId == request.entity.Id &&
                x.Id == request.entity.Comments.FirstOrDefault().Id).ToList()
                .ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);

                _db.SaveChanges();
                return new FolderDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new FolderDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }


    }
}

