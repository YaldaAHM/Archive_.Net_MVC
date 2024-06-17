using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Infrastructure.Folder;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Infrastructure.Localization.FileLocalization;
using YA.Archive.Model;
using YA.Archive.Model.File;
using YA.Archive.Service.FileMdl.FileStorage;
using YA.Archive.Service.FileMdl.Mapping;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FolderMdl.Access;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Imps;
using YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Messaging;
using YA.Localization.AccessLocalize;
using YA.Archive.Service.FileMdl.Sterategy;

namespace YA.Archive.Service.FileMdl.Imps
{
    public class FileService
    {
        private ArchiveDataContext _db;
        private IFileStorage _fileStorage;
        private FolderService _folderService;
        private PermissionClaimsService _permissionClaimsService;

        public FileService()
        {
            _db = new ArchiveDataContext();
            _folderService=new FolderService();
            _permissionClaimsService=new PermissionClaimsService();
        }
        public FileService(IFileStorage fileStorage)
        {
            _fileStorage =  fileStorage;
        }
        public FileService(ArchiveDataContext db, IFileStorage fileStorage)
        {
            _db = new ArchiveDataContext();
            _fileStorage = fileStorage;
        }
        public string GenerateFileCode()
        {
            string code = "";
            var date = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd").Substring(2, 2);
            if (!_db.File.ToList().Any())
            {
                code = (int.Parse(date)).ToString() + "0010";
                return code;
            }


            var file = _db.File.OrderByDescending(f => f.Id).FirstOrDefault();

            var oldCode = file.Code;
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

        public FileInsertResponse<TF> Insert<TRequest, TRootPath,TF>(FileInsertRequest<TRequest,TRootPath> request
            , IFileStorage fileStorage1) //where TRequest : IConvertible //where TF : IConvertible
        {
            try
            {
                if(_permissionClaimsService.FindByExpression(
                    new PermissionClaimsFindByExpressionRequest()
                    {
                        expression =
                            x =>
                            x.ClaimType == request.entity.FolderId.ToString() && x.ClaimValue == ProjectPermissionClaimValue.CanCreate
                            && x.UserId == request.RequsetCurrentUserId
                    }).Entities == null)
                    return new FileInsertResponse<TF>
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                if (_db.File.Any(n => n.Code.Substring(0, 2) == request.entity.Code.Substring(0, 2)
                                       && n.FileName == request.entity.FileName))
            {
                return new FileInsertResponse<TF>
                {
                    Message = String.Format(MessageResource.Exist, FileResource.FileName),
                    ResponseType = ResponseType.Warning,
                    entity = request.entity
                };
            }
            _fileStorage = fileStorage1;
           var folder= _folderService.FindById(new FolderFindByIdRequest() {Id = request.entity.FolderId});
                 string rootPath = folder.entity.Path + FolderName.MainFolder +
                              folder.entity.Code.Substring(0, 2) + @"\" + folder.entity.Code;
                var gCode = "";
                if (folder.entity.Files == null || folder.entity.Files.Count==0)
                {
                     gCode= folder.entity.Code.Substring(0, 2) + "0001";
                }
                else {
            gCode=folder.entity.Files.OrderByDescending(x => x.Id).FirstOrDefault().Code;
                    gCode = (int.Parse(gCode) + 1).ToString();
                }
                
            var f = _fileStorage.Store< TRequest, TRootPath,TF>
                (request.Request, 
                rootPath, gCode.Substring(2)
                , request.IsPartialFile);
            request.entity.FileName = f.FileName;
            request.entity.OrginalName = f.OrigName;
            request.entity.FileSize = f.FileSize;
            request.entity.Path = "";
            request.entity.Folder = null;
            request.entity.CreateDate = fpub.convert2shamsiS(DateTime.Now);
            request.entity.LastUpdateDate = fpub.convert2shamsiS(DateTime.Now);
            var d = request.entity.ToModel();
            d.Code = gCode.ToString();
            d.CreateDate = DateTime.Now;
            d.LastUpdateDate = DateTime.Now;
            var file= _db.File.Add(d);
                _db.SaveChanges();
                foreach (var t in file.TypesOfFiles)
                {
                    var tt = _db.TypeofFile.FirstOrDefault(x => x.Id == t.TypeofFileId);
                    t.TypeofFile = tt;
                }
                var e = _db.File.OrderByDescending(a => a.Id).FirstOrDefault();
                var entity = ApplyAccess.ApplyFileAccess(file.ToViewModel(), request.RequsetCurrentUserId);
                request.entity.Id = e.Id;
                return new FileInsertResponse<TF>
                       {
                           IsSuccess = true,
                           Message = MessageResource.InsertSuccess,
                           ResponseType = ResponseType.Success,
                           entity = entity,
                           FileInfo = new FileInfoVM<TF>()
                           {
                               FileName = f.FileName,
                               EncodeFile = f.EncodeFile,
                               File = f.File,
                               FileSize = f.FileSize,
                               FileType = f.FileType,
                               OrigName = f.OrigName
                           },
                       };

        }
            catch (Exception ex)
            {

                return new FileInsertResponse<TF>
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                           entity = request.entity

    };
}
        }


        public FileUpdateResponse Update(FileUpdateRequest request)
        {
            try
            {
                var c = _db.File.FirstOrDefault(n => n.Id == request.entity.Id);
                if (c.Folder.EditFileLockDate <  DateTime.Now && request.RequsetCurrentUserId != c.Folder.CreateUserId
                     )
                    return new FileUpdateResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                if (_db.File.Any(n =>  n.FileName == request.entity.FileName))
                {
                    return new FileUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, FileResource.FileName),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
               
                c.Description = request.entity.Description;
                c.FolderId = request.entity.FolderId;
                c.KeyWord = request.entity.KeyWord;
                c.LastUpdateDate = DateTime.Now;
                c.LastUpdateUserId = request.entity.LastUpdateUserId;
                c.TypesOfFiles.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
                c.TypesOfFiles = request.entity.TypesOfFiles.ToModelList().ToList();
                foreach (var t in c.TypesOfFiles)
                {
                    var tt = _db.TypeofFile.FirstOrDefault(x => x.Id == t.TypeofFileId);
                    t.TypeofFile = tt;
                }
         
                _db.SaveChanges();
                var entity = ApplyAccess.ApplyFileAccess(c.ToViewModel(), request.RequsetCurrentUserId);
                return new FileUpdateResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.UpdateSuccess,
                           ResponseType = ResponseType.Success,
                           entity = entity
                };

            }
            catch (Exception ex)
            {

                return new FileUpdateResponse
                {
                    Message = MessageResource.Field,
                 
                    ResponseType = ResponseType.Error,
                           entity = request.entity

                       };
            }
        }

        public FileDeleteResponse DeleteById(FileDeleteByIdRequest request, IFileStorage fileStorage1)
        {
            try
            {
                var file=_db.File.FirstOrDefault(f => f.Id == request.Id);
                if (file.Folder.RemoveFileLockDate < DateTime.Now && request.RequsetCurrentUserId != file.Folder.CreateUserId
                    )
                    return new FileDeleteResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                        
                    }; 
                 var c = _db.File.Find(request.Id);
                c.TypesOfFiles.ToList().ForEach(pp => _db.Entry(pp).State = EntityState.Deleted);
               string rootPath = c.Folder.Path + FolderName.MainFolder +
                          c.Folder.Code.Substring(0, 2) + @"\" + c.Folder.Code;
                _fileStorage = fileStorage1;
                var fileStorage = _fileStorage.Remove(rootPath, c.FileName);
                _db.File.Remove(c);
                _db.SaveChanges();
                return new FileDeleteResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.DeleteSuccess,
                           ResponseType = ResponseType.Success,
                       };

            }
            catch (Exception ex)
            {

                return new FileDeleteResponse
                {
                    Message = MessageResource.Field,
                     ResponseType = ResponseType.Error,

                       };
            }
        }

        public FileDeleteResponse Delete(FileDeleteRequest request, IFileStorage fileStorage1)    
        {
            try
            {var c = _db.File.Find(request.entity.Id);
                if (c.Folder.RemoveFileLockDate < DateTime.Now && request.RequsetCurrentUserId != c.Folder.CreateUserId
                      
                      )
                    return new FileDeleteResponse
                    {
                        Message = AccessResource.Limited,
                        ResponseType = ResponseType.Warning,
                       
                    };
                _fileStorage = fileStorage1;
                var fileStorage = _fileStorage.Remove(request.FileName,request.RootPath);

                
                _db.File.Remove(c);
                _db.SaveChanges();
                return new FileDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new FileDeleteResponse
                {
                    Message = MessageResource.Field,
                    
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public FileFindAllResponse FindAll(FileFindAllRequest request)
        {
            
            return new FileFindAllResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FileFindAll(request).Entities
                
            };
        }

        public FileFindAllResponse FindAllPaging(FileFindAllRequest request)
        {
            var c = ChooseSterategy.FileFindAllPaging(request);
              return new FileFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = c.Entities,
                TotalRecords = c.TotalRecords,
                Count = c.Count,
                Index = c.Index
               
            };
           
        }
        public FileFindByIdResponse FindById(FileFindByIdRequest request)
        {
            try
            {
                return new FileFindByIdResponse
                       {
                           IsSuccess = true,
                           Message = MessageResource.FindSuccess,
                           ResponseType = ResponseType.Success,
                           entity = _db.File.Find(request.Id).ToViewModel()
                       };

            }
            catch (Exception ex)
            {

                return new FileFindByIdResponse
                       {
                           Message = ex.Message,
                           ResponseType = ResponseType.Error,

                       };
            }
        }
        public FileFindByExpressionResponse FindByExpression(FileFindByExpressionRequest request)
        {
            
            var expression = ToViewModel(request.expression);
            return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FileFindByExpression(
                    new FileFindByExpressionExpRequest()
                    {
                        RequsetCurrentUserId = request.RequsetCurrentUserId,
                        RequsetCurrentClientCenterId = request.RequsetCurrentClientCenterId,

                    }).Entities
            };

            
        }

        public Expression<Func<FileModel, bool>> ToViewModel
            (Expression<Func<FileVM, bool>> File)
        {

             return Mapper.Map<Expression<Func<FileVM, bool>>,
                Expression<Func<FileModel, bool>>>(File);
        }

        public FileFindByExpressionResponse FindByExpression(FileFindByExpressionExpRequest request)
        {
                  return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FileFindByExpression(request).Entities

            };

        }
        public FileFindByExpressionResponse FindByExpressionPaging(FileFindByExpressionExpRequest request)
        {
            
            var c = ChooseSterategy.FileFindByExpressionPaging(request);
            return new FileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = c.Entities,
                TotalRecords = c.TotalRecords,
                Count = c.Count,
                Index = c.Index
                
            };

         
        }
        public FileFindByExpressionExpResponse FindByExpressionExp(FileFindByExpressionExpRequest request)
        {
            

            var expression = ToViewModel(request.expression);
            return new FileFindByExpressionExpResponse
                   {
                       IsSuccess = true,
                       Message = MessageResource.FindSuccess,
                       ResponseType = ResponseType.Success,
                Entities = ChooseSterategy.FileFindByExpression(request).Entities.ToModelListExp(),
               
            };

        }

        public Expression<Func<FileModel, bool>> ToViewModel
            (Expression<Func<FileExpVM, bool>> File)
        {

             return Mapper.Map<Expression<Func<FileExpVM, bool>>,
                Expression<Func<FileModel, bool>>>(File);
        }
    }
}

