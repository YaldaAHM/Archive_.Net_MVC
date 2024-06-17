using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.TypeofFolderMdl.Imps;
using YA.Archive.Service.Reporting.RFolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.Messaging;
using Microsoft.AspNet.Identity;
using YA.Archive.Service.Reporting.RFolderMdl.VMs;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;
using Kendo.Mvc.Extensions;

namespace YA.Archive.Service.Reporting.RFolderMdl.Imps
{
    public class ReportingFolderService
    {
        private TypeofFolderService _typeOfFolderService;
        private ClientCenterService _clientCenterService;
        private FolderService _folderService;

        public ReportingFolderService()
        {
            _folderService = new FolderService();
            _typeOfFolderService = new TypeofFolderService();
            _clientCenterService = new ClientCenterService();
        }

        public ReportClientFolderRResponse ReportFolder(ReportClientFolderRRequest request)
        {
            if (request == null)
            {
                return new ReportClientFolderRResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    Entities = null
                };
            }
            var fromDate = request.FromDate == null ? DateTime.Now : fpub.convert2miladi(request.FromDate);
            var toDate = request.ToDate == null ? DateTime.Now : fpub.convert2miladi(request.ToDate);
            List<ReportClientFolderRVM> reportClientFolderRList = null; //= new List<ReportMealRVM>();
            var responseFolder = new FolderFindByExpressionResponse();

            var typeofFolderId = request.entity.TypesOfFolders.Select(c => c.TypeofFolderId).ToArray();
            var tp = (typeofFolderId.Any()) ? typeofFolderId.ToList().Count() : 0;

            responseFolder = _folderService.FindByExpression(
                new FolderFindByExpressionExpRequest()
                {

                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x =>
                            (request.entity.Code == "" || x.Code.Contains(request.entity.Code)) &&
                        (request.entity.Name == "" || x.Name.Contains(request.entity.Name)) &&
                        (request.entity.ClientCenterId == 0 ||
                         x.ClientCenterId == request.entity.ClientCenterId) &&
                        (request.entity.KeyWord == "" ||
                         x.KeyWord.Contains(request.entity.KeyWord)) &&
                        (request.FromDate == "" || x.CreateDate >= fromDate) &&
                        (request.ToDate == "" || x.CreateDate <= toDate) &&
                        (tp == 0 ||
                         (x.TypesOfFolders.Any()

                          && x.TypesOfFolders.Count() != 0 &&
                          x.TypesOfFolders.Any(s => typeofFolderId.Contains(s.TypeofFolderId))))

                });
            if (!responseFolder.IsSuccess)
            {

            }

            if (responseFolder.Entities != null)
            {
                ReportClientFolderRVM reportClientFolderR = new ReportClientFolderRVM();
                foreach (var r in responseFolder.Entities)
                {
                    string typeOfFolder = "";
                    foreach (var type in r.TypesOfFolders)
                    {
                        typeOfFolder = type.TypeofFolder.Name + ",";
                    }
                    if (reportClientFolderRList == null)
                    {
                        reportClientFolderRList = new List<ReportClientFolderRVM>();
                        reportClientFolderR = new ReportClientFolderRVM();
                        reportClientFolderR.ReportCFolderVMs = new List<ReportCFolderRVM>();
                        reportClientFolderR.ClientName = r.ClientCenter.Name;
                        reportClientFolderR.ClientId = r.ClientCenter.Id;
                        reportClientFolderR.TodayDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                        var reportCFolderR = new ReportCFolderRVM()
                        {
                            FolderId = r.Id,
                            FolderName = r.Name,
                            ClientId = r.ClientCenterId,
                            ClientName = r.ClientCenter.Name,
                            Code = r.Code,
                            TypesOfFolders = typeOfFolder,
                            KeyWord = r.KeyWord,
                            Description = r.Description,
                            CreateDate = r.CreateDate,
                            CreateUserName = r.CreateUser.UserName,
                            LastUpdateDate = r.LastUpdateDate,
                            LastUpdateUserName = r.LastUpdateUser.UserName,

                        };

                        reportClientFolderR.ReportCFolderVMs.Add(reportCFolderR);
                        reportClientFolderRList.Add(reportClientFolderR);

                    }
                    else

                    {
                        bool isNewClient = false;
                        bool isNewSubClient = false;
                        if (reportClientFolderRList.Find(b => b.ClientId == r.ClientCenterId) == null)
                        {
                            isNewClient = true;
                            reportClientFolderR = new ReportClientFolderRVM();
                            reportClientFolderR.ReportCFolderVMs = new List<ReportCFolderRVM>();
                            reportClientFolderR.ClientName = r.ClientCenter.Name;
                            reportClientFolderR.ClientId = r.ClientCenter.Id;
                        }


                        var reportCFolderR = new ReportCFolderRVM()
                        {
                            FolderId = r.Id,
                            FolderName = r.Name,
                            ClientId = r.ClientCenterId,
                            ClientName = r.ClientCenter.Name,
                            Code = r.Code,
                            TypesOfFolders = typeOfFolder,
                            KeyWord = r.KeyWord,
                            Description = r.Description,
                            CreateDate = r.CreateDate,
                            CreateUserName = r.CreateUser.UserName,
                            LastUpdateDate = r.LastUpdateDate,
                            LastUpdateUserName = r.LastUpdateUser.UserName,
                        };

                        if (isNewClient == false && reportClientFolderRList.FirstOrDefault
                            (c => c.ClientId == r.ClientCenterId) != null

                            )
                        {

                            reportClientFolderR.ReportCFolderVMs.Add(reportCFolderR);
                        }
                        else if (reportClientFolderRList.FirstOrDefault
                            (c => c.ClientId == r.ClientCenterId) != null)
                        {
                            reportClientFolderR.ReportCFolderVMs.Add(reportCFolderR);
                        }
                        if (isNewClient == true)
                        {
                            reportClientFolderR.ReportCFolderVMs.Add(reportCFolderR);
                            reportClientFolderRList.Add(reportClientFolderR);
                        }
                    }
                }
            }

            var reportClientFolderrR = new ReportClientFolderRVM();
            if (reportClientFolderRList != null)
                reportClientFolderRList.Add(reportClientFolderrR);
            return new ReportClientFolderRResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = reportClientFolderRList
            };
        }

    }
}