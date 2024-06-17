using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Localization.TypeOfFileLocalization;
using YA.Archive.Model;
using YA.Archive.Model.TypeofFile;
using YA.Archive.Service.TypeofFileMdl.Mapping;
using YA.Archive.Service.TypeofFileMdl.Messaging;
using YA.Archive.Service.TypeofFileMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.TypeofFileMdl.Imps
{
    public class TypeofFileService
    {
        private ArchiveDataContext _db;

        public TypeofFileService()
        {
            _db = new ArchiveDataContext();
        }

        public TypeofFileService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public TypeofFileInsertResponse Insert(TypeofFileInsertRequest request)
        {
            try
            {
                if (_db.TypeofFile.Any(n => n.Name == request.entity.Name))
                {
                    return new TypeofFileInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, TypeOfFileResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var d = request.entity.ToModel();
                _db.TypeofFile.Add(d);
                _db.SaveChanges();
                var e = _db.TypeofFile.OrderByDescending(a => a.Id).FirstOrDefault();

                request.entity.Id = e.Id;
                return new TypeofFileInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new TypeofFileInsertResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public TypeofFileUpdateResponse Update(TypeofFileUpdateRequest request)
        {
            try
            {
                if (_db.TypeofFile.Any(n => n.Name == request.entity.Name))
                {
                    return new TypeofFileUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, TypeOfFileResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var c = _db.TypeofFile.FirstOrDefault(n => n.Id == request.entity.Id);
                Mapper.Map(request.entity, c);
                _db.SaveChanges();
                return new TypeofFileUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new TypeofFileUpdateResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public TypeofFileDeleteResponse DeleteById(TypeofFileDeleteByIdRequest request)
        {
            try
            {

                var c = _db.TypeofFile.Find(request.Id);
                if (c == null)
                {
                    return new TypeofFileDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, TypeOfFileResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.File.Any(n => n.TypesOfFiles.Any(b => b.TypeofFileId == request.Id)))
                {
                    return new TypeofFileDeleteResponse
                    {
                        Message = String.Format(TypeOfFileResource.UsedTypeOfFile, TypeOfFileResource.Name),
                        ResponseType = ResponseType.Warning

                    };
                }
                _db.TypeofFile.Remove(c);
                _db.SaveChanges();
                return new TypeofFileDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new TypeofFileDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFileDeleteResponse Delete(TypeofFileDeleteRequest request)
        {
            try
            {

                var c = _db.TypeofFile.Find(request.entity.Id);
                if (c == null)
                {
                    return new TypeofFileDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, TypeOfFileResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.File.Any(n => n.TypesOfFiles.Any(b => b.TypeofFileId == request.entity.Id)))
                {
                    return new TypeofFileDeleteResponse
                    {
                        Message = String.Format("The attachment type is used."),
                        ResponseType = ResponseType.Warning,

                    };
                }
                _db.TypeofFile.Remove(c);
                _db.SaveChanges();
                return new TypeofFileDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new TypeofFileDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFileFindAllResponse FindAll(TypeofFileFindAllRequest request)
        {
            //try
            //{
            var ents = _db.TypeofFile.ToList().ToViewModelList();
            return new TypeofFileFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };


        }


        public TypeofFileFindByIdResponse FindById(TypeofFileFindByIdRequest request)
        {
            try
            {
                return new TypeofFileFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = _db.TypeofFile.Find(request.Id).ToViewModel()
                };

            }
            catch (Exception ex)
            {

                return new TypeofFileFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFileFindByExpressionResponse FindByExpression(TypeofFileFindByExpressionRequest request)
        {

            var expression = ToViewModel(request.expression);
            return new TypeofFileFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = _db.TypeofFile.Where(expression).ToList().ToViewModelList()

            };


        }

        public Expression<Func<TypeofFileModel, bool>> ToViewModel
            (Expression<Func<TypeofFileVM, bool>> TypeofFile)
        {

            return Mapper.Map<Expression<Func<TypeofFileVM, bool>>,
               Expression<Func<TypeofFileModel, bool>>>(TypeofFile);
        }
    }
}

