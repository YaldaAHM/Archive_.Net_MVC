using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.Reporting.RCommentMdl.VMs;
using Microsoft.AspNet.Identity;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;
using YA.Archive.Service.Reporting.RCommentMdl.Messaging;
using YA.Archive.Service.Reporting.RCommentMdl.Imps;
using YA.Archive.Service.FolderMdl.Imps;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;

namespace YA.Archive.Service.Reporting.RCommentMdl.Imps
{
    public class ReportingCommentService
    {
      
        private FolderService _folderService;
        public ReportingCommentService()
        {
            _folderService = new FolderService();
           
        }

        public ReportCFCommentRResponse ReportComment(ReportCFCommentRRequest request)
        {
            if(request==null)
            return new ReportCFCommentRResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = null
            };
            List<ReportClientFolderCommentRVM> reportClientFolderCommentRList = null;
            var fromDate = request.FromDate == null ? DateTime.Now : fpub.convert2miladi(request.FromDate);
            var toDate = request.ToDate == null ? DateTime.Now : fpub.convert2miladi(request.ToDate);
            var responseFolder = new FolderFindByExpressionResponse();

            responseFolder = _folderService.FindByExpression(
                new FolderFindByExpressionExpRequest()
                { 
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    expression = x =>

                     x.Comments.Any()

                                      && x.Comments.Count() != 0 &&
                                     (request.entity.FolderId == null || x.Comments.Select(m => m.FolderId).Contains(request.entity.FolderId)) &&
                                       (request.entity.Description == "" ||
                                        x.Comments.Any(m => m.Description.Contains(request.entity.Description))) &&
                                      (request.entity.Code == "" || x.Comments.Any(m => m.Code.Contains(request.entity.Code))) &&
                                      (request.FromDate == "" || x.Comments.Any(m => m.CreateDate >= fromDate))
                                      &&
                                      (request.FromDate == "" || x.Comments.Any(m => m.CreateDate <= toDate))
                                     
                       
                });
            var cList = new FolderFindByExpressionResponse();
            cList.Entities = new List<FolderVM>();
            var cList1 = cList.Entities.ToList();
            foreach (var r in responseFolder.Entities)
            {
                cList1.Add(new FolderVM()
                {
                    Code = r.Code,
                    CreateUser = r.CreateUser,
                    Description = r.Description,
                    Id = r.Id,
                    Name=r.Name,
                ClientCenter=r.ClientCenter,
                    LastUpdateUser = r.LastUpdateUser,
                    CreateDate = r.CreateDate,
                    LastUpdateDate = r.LastUpdateDate,
                    Comments = new List<CommentVM>()
                    {

                    },
                    ClientCenterId = r.ClientCenterId
                }
                              );
                foreach (var c in r.Comments)
                {
                    if (
                                   (request.entity.Description == "" ||
                                     c.Description.Contains(request.entity.Description)) &&
                                  (request.entity.Code == "" || c.Code.Contains(request.entity.Code)) &&
                                  (request.FromDate == "" ||
                                  fpub.convert2miladi(c.CreateDate) >= fromDate)
                                  &&
                                  (request.FromDate == "" ||
                                  fpub.convert2miladi(c.CreateDate) <= toDate)) {

                        cList1.FirstOrDefault(x=>x.Id==r.Id).Comments.Add(c);

                       }
                }
            }
            if (!responseFolder.IsSuccess)
                {

                }



            if (cList1 != null)
            {
                ReportClientFolderCommentRVM reportClientFolderR = new ReportClientFolderCommentRVM();
                foreach (var rr in cList1)
                {
                    foreach (var r in rr.Comments)
                    {

                        if (reportClientFolderCommentRList == null)
                        {
                            reportClientFolderCommentRList = new List<ReportClientFolderCommentRVM>();
                            reportClientFolderR = new ReportClientFolderCommentRVM();
                            reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                            reportClientFolderR.ClientName = rr.ClientCenter.Name;
                            reportClientFolderR.ClientId = rr.ClientCenter.Id;
                            reportClientFolderR.TodayDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                            var reportCFolderRs = new List<ReportCFCommentRVM>();
                            var reportCFolderR = new ReportCFCommentRVM()
                                                 {
                                                     FolderId = rr.Id,
                                                     FolderName = rr.Name,
                                                     ClientId = rr.ClientCenterId,
                                                     ClientName = rr.ClientCenter.Name,
                                                     Code = r.Code,
                                                     Description = r.Description,
                                                     CreateDate = r.CreateDate,
                                                     CreateUserName = r.CreateUser.UserName,
                                                     LastUpdateDate = r.LastUpdateDate,
                                                     LastUpdateUserName = r.LastUpdateUser.UserName,

                                                 };
                            reportCFolderRs.Add(reportCFolderR);
                            var reportFolderR = new ReportFolderRVM()
                                                {
                                                    FolderId = rr.Id,
                                                    FolderName = rr.Name,
                                                    ReportClientFolderRVM = reportClientFolderR,
                                                    ReportCFCommentRVMs = reportCFolderRs
                                                };
                            reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);
                            reportClientFolderCommentRList.Add(reportClientFolderR);

                        }
                        else

                        {
                            bool isNewClient = false;
                            bool isNewSubClient = false;
                            if (reportClientFolderCommentRList.Find(b => b.ClientId == rr.ClientCenterId) == null)
                            {
                                isNewClient = true;
                                reportClientFolderR = new ReportClientFolderCommentRVM();
                                reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                                reportClientFolderR.ReportFolderRVMs = new List<ReportFolderRVM>();
                                reportClientFolderR.ClientName = rr.ClientCenter.Name;
                                reportClientFolderR.ClientId = rr.ClientCenter.Id;
                            }


                            var reportCFolderR = new ReportCFCommentRVM()
                                                 {
                                                     FolderId = rr.Id,
                                                     FolderName = rr.Name,
                                                     ClientId = rr.ClientCenterId,
                                                     ClientName = rr.ClientCenter.Name,
                                                     Code = r.Code,
                                                     Description = r.Description,
                                                     CreateDate = r.CreateDate,
                                                     CreateUserName = r.CreateUser.UserName,
                                                     LastUpdateDate = r.LastUpdateDate,
                                                     LastUpdateUserName = r.LastUpdateUser.UserName,
                                                 };

                            if (isNewClient == false && reportClientFolderCommentRList.FirstOrDefault
                                (c => c.ClientId == rr.ClientCenterId) != null
                                && reportClientFolderCommentRList.FirstOrDefault
                                    (c => c.ClientId == rr.ClientCenterId)
                                    .ReportFolderRVMs.Find(b =>
                                        b.ReportClientFolderRVM.ClientId == rr.ClientCenterId &&
                                        b.FolderId == rr.Id) == null)
                            {
                                var reportCFolderRs = new List<ReportCFCommentRVM>();
                                reportCFolderRs.Add(reportCFolderR);
                                var reportFolderR = new ReportFolderRVM()
                                                    {
                                                        FolderId = rr.Id,
                                                        FolderName = rr.Name,
                                                        ReportClientFolderRVM = reportClientFolderR,
                                                        ReportCFCommentRVMs = reportCFolderRs
                                                    };
                                reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);
                            }
                            else if (reportClientFolderCommentRList.FirstOrDefault
                                (c => c.ClientId == rr.ClientCenterId) != null)
                            {
                                reportClientFolderR.ReportFolderRVMs
                                    .FirstOrDefault(f => f.FolderId == rr.Id
                                                         && f.ReportClientFolderRVM.ClientId == rr.ClientCenterId)
                                    .ReportCFCommentRVMs.Add(reportCFolderR);
                            }
                            if (isNewClient == true)
                            {
                                var reportCFolderRs = new List<ReportCFCommentRVM>();
                                reportCFolderRs.Add(reportCFolderR);
                                var reportFolderR = new ReportFolderRVM()
                                                    {
                                                        FolderId = rr.Id,
                                                        FolderName = rr.Name,
                                                        ReportClientFolderRVM = reportClientFolderR,
                                                        ReportCFCommentRVMs = reportCFolderRs
                                                    };
                                reportClientFolderR.ReportFolderRVMs.Add(reportFolderR);

                                reportClientFolderCommentRList.Add(reportClientFolderR);
                            }
                    
                        }
                    }
                }
            }
            var reportClientFolderrR = new ReportClientFolderCommentRVM();
            if (reportClientFolderCommentRList != null)
                reportClientFolderCommentRList.Add(reportClientFolderrR);
            return new ReportCFCommentRResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = reportClientFolderCommentRList
            };
        }

    }  
}
