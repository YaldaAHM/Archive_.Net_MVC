using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.Reporting.RFileMdl.VMs;
using Microsoft.AspNet.Identity;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;
using YA.Archive.Service.Reporting.RFileMdl.Messaging;
using YA.Archive.Service.FileMdl.Messaging;
using YA.Archive.Service.FileMdl.Imps;
using Kendo.Mvc.Extensions;

namespace YA.Archive.Service.Reporting.RFileMdl.Imps
{
    public class ReportingFileService
    {

        private FileService _fileService;

        public ReportingFileService()
        {
            _fileService = new FileService();

        }

        public ReportCFFileRResponse ReportFile(ReportCFFileRRequest request)
        {
            if (request == null)
                return new ReportCFFileRResponse
                {
                    IsSuccess = true,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    Entities = null
                };
            var fromDate = request.FromDate == null ? DateTime.Now : fpub.convert2miladi(request.FromDate);
            var toDate = request.ToDate == null ? DateTime.Now : fpub.convert2miladi(request.ToDate);
            List<ReportClientFolderFileRVM> reportClientFolderRList = null; //= new List<ReportMealRVM>();
            var responseFile = new FileFindByExpressionResponse();

            var typeofFileId = request.entity.TypesOfFiles.Select(c => c.TypeofFileId).ToArray();
            var tp = (typeofFileId.Any()) ? typeofFileId.ToList().Count() : 0;

            responseFile = _fileService.FindByExpression(
                new FileFindByExpressionExpRequest()
                {

                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x =>
                                       (request.entity.Code == "" || x.Code.Contains(request.entity.Code)) &&
                                      (request.entity.FileName == "" ||
                                       x.FileName.Contains(request.entity.FileName)) &&
                                      (request.entity.FolderId == 0 || x.FolderId == request.entity.FolderId) &&
                                      (request.entity.KeyWord == "" ||
                                       x.KeyWord.Contains(request.entity.KeyWord)) &&
                                      (request.FromDate == "" || x.CreateDate >= fromDate) &&
                                      (request.ToDate == "" || x.CreateDate <= toDate) &&
                                       (tp == 0 ||
                                      (x.TypesOfFiles.Any()

                                      && x.TypesOfFiles.Count() != 0 &&
                                      x.TypesOfFiles.Any(s => typeofFileId.Contains(s.TypeofFileId))))

                });
            if (!responseFile.IsSuccess)
            {

            }

            if (responseFile.Entities != null)
            {
                ReportClientFolderFileRVM reportClientFolderR = new ReportClientFolderFileRVM();
                foreach (var r in responseFile.Entities)
                {
                    string typeOfFile = "";
                    foreach (var type in r.TypesOfFiles)
                    {
                        typeOfFile = type.TypeofFile.Name + ",";
                    }
                    if (reportClientFolderRList == null)
                    {
                        reportClientFolderRList = new List<ReportClientFolderFileRVM>();
                        reportClientFolderR = new ReportClientFolderFileRVM();
                        reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                        reportClientFolderR.ClientName = r.Folder.ClientCenter.Name;
                        reportClientFolderR.ClientId = r.Folder.ClientCenter.Id;
                        reportClientFolderR.TodayDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                        var reportCFolderRs = new List<ReportCFFileRVM>();
                        var reportCFolderR = new ReportCFFileRVM()
                        {
                            FolderId = r.Folder.Id,
                            FolderName = r.Folder.Name,
                            ClientId = r.Folder.ClientCenterId,
                            ClientName = r.Folder.ClientCenter.Name,
                            Code = r.Code,
                            TypesOfFiles = typeOfFile,
                            KeyWord = r.KeyWord,
                            Description = r.Description,
                            CreateDate = r.CreateDate,
                            CreateUserName = r.CreateUser.UserName,
                            LastUpdateDate = r.LastUpdateDate,
                            LastUpdateUserName = r.LastUpdateUser.UserName,
                            FileName = r.OrginalName

                        };
                        reportCFolderRs.Add(reportCFolderR);
                        var reportFolderR = new ReportFolderRVM()
                        {
                            FolderId = r.Folder.Id,
                            FolderName = r.Folder.Name,
                            ReportClientFolderRVM = reportClientFolderR,
                            ReportCFFileRVMs = reportCFolderRs
                        };
                        reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);
                        reportClientFolderRList.Add(reportClientFolderR);

                    }
                    else

                    {
                        bool isNewClient = false;
                        bool isNewSubClient = false;
                        if (reportClientFolderRList.Find(b => b.ClientId == r.Folder.ClientCenterId) == null)
                        {
                            isNewClient = true;
                            reportClientFolderR = new ReportClientFolderFileRVM();
                            reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                            reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                            reportClientFolderR.ClientName = r.Folder.ClientCenter.Name;
                            reportClientFolderR.ClientId = r.Folder.ClientCenter.Id;
                        }


                        var reportCFolderR = new ReportCFFileRVM()
                        {
                            FolderId = r.Folder.Id,
                            FolderName = r.Folder.Name,
                            ClientId = r.Folder.ClientCenterId,
                            ClientName = r.Folder.ClientCenter.Name,
                            Code = r.Code,
                            TypesOfFiles = typeOfFile,
                            KeyWord = r.KeyWord,
                            Description = r.Description,
                            CreateDate = r.CreateDate,
                            CreateUserName = r.CreateUser.UserName,
                            LastUpdateDate = r.LastUpdateDate,
                            LastUpdateUserName = r.LastUpdateUser.UserName,
                            FileName = r.OrginalName
                        };

                        if (isNewClient == false && reportClientFolderRList.FirstOrDefault
                            (c => c.ClientId == r.Folder.ClientCenterId) != null
                            && reportClientFolderRList.FirstOrDefault
                                (c => c.ClientId == r.Folder.ClientCenterId)
                                .ReportFolderRVMs.Find(b =>
                                    b.ReportClientFolderRVM.ClientId == r.Folder.ClientCenterId &&
                                    b.FolderId == r.Folder.Id) == null)
                        {
                            var reportCFolderRs = new List<ReportCFFileRVM>();
                            reportCFolderRs.Add(reportCFolderR);
                            var reportFolderR = new ReportFolderRVM()
                            {
                                FolderId = r.Folder.Id,
                                FolderName = r.Folder.Name,
                                ReportClientFolderRVM = reportClientFolderR,
                                ReportCFFileRVMs = reportCFolderRs
                            };
                            reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);
                        }
                        else if (reportClientFolderRList.FirstOrDefault
                            (c => c.ClientId == r.Folder.ClientCenterId) != null)
                        {
                            reportClientFolderR.ReportFolderRVMs
                                .FirstOrDefault(f => f.FolderId == r.Folder.Id
                                                     && f.ReportClientFolderRVM.ClientId == r.Folder.ClientCenterId)
                                .ReportCFFileRVMs.Add(reportCFolderR);
                        }
                        if (isNewClient == true)
                        {
                            var reportCFolderRs = new List<ReportCFFileRVM>();
                            reportCFolderRs.Add(reportCFolderR);
                            var reportFolderR = new ReportFolderRVM()
                            {
                                FolderId = r.Folder.Id,
                                FolderName = r.Folder.Name,
                                ReportClientFolderRVM = reportClientFolderR,
                                ReportCFFileRVMs = reportCFolderRs
                            };
                            reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);

                            reportClientFolderRList.Add(reportClientFolderR);
                        }
                        //  break;
                    }
                }
            }

            var reportClientFolderrR = new ReportClientFolderFileRVM();
            if (reportClientFolderRList != null)
                reportClientFolderRList.Add(reportClientFolderrR);
            return new ReportCFFileRResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = reportClientFolderRList
            };


        }
    }
}