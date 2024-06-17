using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Model;
using YA.Archive.Service.ReportTitleMdl.Messaging;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;
using AutoMapper;
using YA.Archive.Service.ReportTitleMdl.Mapping;

namespace YA.Archive.Service.ReportTitleMdl.Imps
{
    public class ReportTitleService
    {
        private ArchiveDataContext _db;

        public ReportTitleService()
        {
            _db = new ArchiveDataContext();
        }


        public ReportTitleInsertResponse Insert(ReportTitleInsertRequest request)
        {
            try
            {
                var d = request.entity.ToModel();
                _db.ReportTitle.Add(d);
                _db.SaveChanges();
                var e = _db.ReportTitle.OrderByDescending(a => a.Id).FirstOrDefault();

                request.entity.Id = e.Id;
                return new ReportTitleInsertResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.InsertSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new ReportTitleInsertResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }


        public ReportTitleUpdateResponse Update(ReportTitleUpdateRequest request)
        {
            try
            {

                var c = _db.ReportTitle.FirstOrDefault(n => n.Id == request.entity.Id);
                Mapper.Map(request.entity, c);
                _db.SaveChanges();
                return new ReportTitleUpdateResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.UpdateSuccess,
                    ResponseType = ResponseType.Success,
                    entity = request.entity
                };

            }
            catch (Exception ex)
            {

                return new ReportTitleUpdateResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,
                    entity = request.entity

                };
            }
        }




        public ReportTitleFindAllResponse FindAll(ReportTitleFindAllRequest request)
        {

            return new ReportTitleFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = _db.ReportTitle.ToViewModelList()
            };


        }


        public ReportTitleFindByIdResponse FindById(ReportTitleFindByIdRequest request)
        {
            try
            {
                return new ReportTitleFindByIdResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    entity = _db.ReportTitle.Find(request.Id).ToViewModel()
                };

            }
            catch (Exception ex)
            {

                return new ReportTitleFindByIdResponse
                {
                    Message = ex.Message,
                    ResponseType = ResponseType.Error,

                };
            }
        }
    }
}
