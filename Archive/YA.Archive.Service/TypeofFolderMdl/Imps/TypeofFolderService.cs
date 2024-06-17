using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Localization.TypeOfProjectLocalization;
using YA.Archive.Model;
using YA.Archive.Model.TypeofFolder;
using YA.Archive.Service.TypeofFolderMdl.Mapping;
using YA.Archive.Service.TypeofFolderMdl.Messaging;
using YA.Archive.Service.TypeofFolderMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.TypeofFolderMdl.Imps
{
    public class TypeofFolderService
    {
        private ArchiveDataContext _db;

        public TypeofFolderService()
        {
            _db = new ArchiveDataContext();
        }

        public TypeofFolderService(ArchiveDataContext db)
        {
            _db = new ArchiveDataContext();
        }

        public TypeofFolderInsertResponse Insert(TypeofFolderInsertRequest request)
        {
            try
            {
                if (_db.TypeofFolder.Any(n => n.Name == request.entity.Name))
                {
                    return new TypeofFolderInsertResponse
                    {
                        Message = String.Format(MessageResource.Exist, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var d = request.entity.ToModel();
                _db.TypeofFolder.Add(d);
                _db.SaveChanges();

                return new TypeofFolderInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new TypeofFolderInsertResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public TypeofFolderUpdateResponse Update(TypeofFolderUpdateRequest request)
        {
            try
            {
                if (_db.TypeofFolder.Any(n => n.Name == request.entity.Name))
                {
                    return new TypeofFolderUpdateResponse
                    {
                        Message = String.Format(MessageResource.Exist, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Warning,
                        entity = request.entity
                    };
                }
                var c = _db.TypeofFolder.FirstOrDefault(n => n.Id == request.entity.Id);
                Mapper.Map(request.entity, c);
                _db.SaveChanges();
                return new TypeofFolderUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new TypeofFolderUpdateResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }

        public TypeofFolderDeleteResponse DeleteById(TypeofFolderDeleteByIdRequest request)
        {
            try
            {

                var c = _db.TypeofFolder.Find(request.Id);
                if (c == null)
                {
                    return new TypeofFolderDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.Folder.Any(n => n.TypesOfFolders.Any(b => b.TypeofFolderId == request.Id)))
                {
                    return new TypeofFolderDeleteResponse
                    {
                        Message = String.Format(TypeOfProjectResource.UsedTypeOfFolder, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
                _db.TypeofFolder.Remove(c);
                _db.SaveChanges();
                return new TypeofFolderDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {

                return new TypeofFolderDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFolderDeleteResponse Delete(TypeofFolderDeleteRequest request)
        {
            try
            {

                var c = _db.TypeofFolder.Find(request.entity.Id);
                if (c == null)
                {
                    return new TypeofFolderDeleteResponse
                    {
                        Message = String.Format(MessageResource.NotExist, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Error,

                    };
                }
                if (_db.Folder.Any(n => n.TypesOfFolders.Any(b => b.TypeofFolderId == request.entity.Id)))
                {
                    return new TypeofFolderDeleteResponse
                    {
                        Message = String.Format(TypeOfProjectResource.UsedTypeOfFolder, TypeOfProjectResource.Name),
                        ResponseType = ResponseType.Warning,

                    };
                }
                _db.TypeofFolder.Remove(c);
                _db.SaveChanges();
                return new TypeofFolderDeleteResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.DeleteSuccess,
                    ResponseType = ResponseType.Success,
                };

            }
            catch (Exception ex)
            {
                return new TypeofFolderDeleteResponse
                {
                    Message = MessageResource.Field,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFolderFindAllResponse FindAll(TypeofFolderFindAllRequest request)
        {
            //try
            //{
            var ents = _db.TypeofFolder.ToList().ToViewModelList();
            return new TypeofFolderFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }


        public TypeofFolderFindByIdResponse FindById(TypeofFolderFindByIdRequest request)
        {
            try
            {
                return new TypeofFolderFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = _db.TypeofFolder.Find(request.Id).ToViewModel()
                };

            }
            catch (Exception ex)
            {

                return new TypeofFolderFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
        public TypeofFolderFindByExpressionResponse FindByExpression(TypeofFolderFindByExpressionRequest request)
        {

            var expression = ToViewModel(request.expression);
            return new TypeofFolderFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = _db.TypeofFolder.Where(expression).ToList().ToViewModelList()

            };


        }

        public Expression<Func<TypeofFolderModel, bool>> ToViewModel
            (Expression<Func<TypeofFolderVM, bool>> TypeofFolder)
        {

            return Mapper.Map<Expression<Func<TypeofFolderVM, bool>>,
                Expression<Func<TypeofFolderModel, bool>>>(TypeofFolder);
        }

    }
}

