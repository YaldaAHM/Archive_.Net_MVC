using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.ClientCenterMdl.Imps;
using YA.Archive.Service.GroupMdl.VMs;
using YA.Archive.Service.GroupUserMdl.VMs;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.Messaging;
using Microsoft.AspNet.Identity;
using YA.Archive.Service.Reporting.RUserLogTimeMdl.VMs;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Localization.MessageLocalize;
using YA.Infrastructure.Service;
using Kendo.Mvc.Extensions;

namespace YA.Archive.Service.Reporting.RUserLogTimeMdl.Imps
{
    public class ReportingUserLogTimeService
    {
        private UserManagementService _userService;
        private ClientCenterService _clientCenterService;
        private UserLogTimeService _userLogTimeService;

        public ReportingUserLogTimeService()
        {
            _userService = new UserManagementService();
            _userLogTimeService = new UserLogTimeService();
            _clientCenterService = new ClientCenterService();
        }

        public ReportClientUserLogTimeRResponse ReportUserLogTime(ReportClientUserLogTimeRRequest request)
        {
            if (request == null)
            {
                return new ReportClientUserLogTimeRResponse
                {
                    IsSuccess = false,
                    Message = MessageResource.FindSuccess,
                    ResponseType = ResponseType.Success,
                    Entities = null
                };
            }
            var fromDate = request.FromDate == null ? DateTime.Now : fpub.convert2miladi(request.FromDate);
            var toDate = request.ToDate == null ? DateTime.Now : fpub.convert2miladi(request.ToDate);
            List<ReportClientUserLogTimeRVM> reportClientUserLogTimeRList = null;
            var responseUser = new UserLogTimeFindByExpressionResponse();

            var GroupUsersId = request.entity.GroupUsers.Select(c => c.GroupId).ToArray();
            var tp1 = (GroupUsersId.Any()) ? GroupUsersId.ToList().Count() : 0;
            responseUser = _userLogTimeService.FindByExpression(new UserLogTimeFindByExpressionExpRequest()
            {

                RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                expression = x =>
                (request.entity.UserName == "" || x.User.UserName.Contains(request.entity.UserName)) &&
    (request.entity.ClientCenterId == 0 || x.User.ClientCenterId == request.entity.ClientCenterId) &&
    (tp1 == 0 ||
     (x.User.GroupUsers.Any()

      && x.User.GroupUsers.Count() != 0 &&
      x.User.GroupUsers.Any(s => GroupUsersId.Contains(s.GroupId))))
    && (request.FromDate == "" || x.LoginTime >= fromDate) &&
    (request.ToDate == "" || x.LogoutTime <= toDate)


            });


            users = ToUserLog(responseUser.Entities.ToList());


            if (users != null)
            {
                ReportClientUserLogTimeRVM reportClientUserLogTimeR = new ReportClientUserLogTimeRVM();
                foreach (var r in users)
                {
                    string groupUsers = "";
                    foreach (var type in r.Groups)
                    {
                        groupUsers = groupUsers + "," + type.Name;
                    }
                    if (reportClientUserLogTimeRList == null)
                    {
                        reportClientUserLogTimeRList = new List<ReportClientUserLogTimeRVM>();
                        reportClientUserLogTimeR = new ReportClientUserLogTimeRVM();
                        reportClientUserLogTimeR.ReportCUserLogTimeVMs = new List<ReportCUserLogTimeRVM>();
                        reportClientUserLogTimeR.ClientName = r.ClientCenter.Name;
                        reportClientUserLogTimeR.ClientId = r.ClientCenter.Id;
                        reportClientUserLogTimeR.TodayDate = fpub.convert2shamsi(DateTime.Now).ToString("yyyy/MM/dd");
                        var reportCUserLogTimeR = new ReportCUserLogTimeRVM()
                        {
                            UserLogTimeId = r.Id,
                            ClientId = r.ClientCenterId,
                            ClientName = r.ClientCenter.Name,
                            UserName = r.UserName,
                            UserId = r.UserName,
                            GroupUser = groupUsers,
                            LoginTime = r.LoginTimeStr,
                            LogoutTime = r.LogoutTimeStr,

                        };
                        reportClientUserLogTimeR.ReportCUserLogTimeVMs.Add(reportCUserLogTimeR);
                        reportClientUserLogTimeRList.Add(reportClientUserLogTimeR);

                    }
                    else

                    {
                        bool isNewClient = false;
                        bool isNewSubClient = false;
                        if (reportClientUserLogTimeRList.Find(b => b.ClientId == r.ClientCenterId) == null)
                        {
                            isNewClient = true;
                            reportClientUserLogTimeR = new ReportClientUserLogTimeRVM();
                            reportClientUserLogTimeR.ReportCUserLogTimeVMs = new List<ReportCUserLogTimeRVM>();
                            reportClientUserLogTimeR.ClientName = r.ClientCenter.Name;
                            reportClientUserLogTimeR.ClientId = r.ClientCenter.Id;
                        }


                        var reportCUserLogTimeR = new ReportCUserLogTimeRVM()
                        {

                            UserLogTimeId = r.Id,
                            ClientId = r.ClientCenterId,
                            ClientName = r.ClientCenter.Name,
                            UserName = r.UserName,
                            UserId = r.UserName,
                            GroupUser = groupUsers,
                            LoginTime = r.LoginTimeStr,
                            LogoutTime = r.LogoutTimeStr,
                        };

                        if (isNewClient == false && reportClientUserLogTimeRList.FirstOrDefault
                            (c => c.ClientId == r.ClientCenterId) != null

                            )
                        {

                            reportClientUserLogTimeR.ReportCUserLogTimeVMs.Add(reportCUserLogTimeR);
                        }
                        else if (reportClientUserLogTimeRList.FirstOrDefault
                            (c => c.ClientId == r.ClientCenterId) != null)
                        {

                            reportClientUserLogTimeR.ReportCUserLogTimeVMs.Add(reportCUserLogTimeR);
                        }
                        if (isNewClient == true)
                        {
                            reportClientUserLogTimeR.ReportCUserLogTimeVMs.Add(reportCUserLogTimeR);
                            reportClientUserLogTimeRList.Add(reportClientUserLogTimeR);
                        }

                    }
                }
            }

            var reportClientUserLogTimerR = new ReportClientUserLogTimeRVM();
            if (reportClientUserLogTimeRList != null)
                reportClientUserLogTimeRList.Add(reportClientUserLogTimerR);
            return new ReportClientUserLogTimeRResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = reportClientUserLogTimeRList
            };
        }



        private List<GroupVM> GroupsList(List<GroupUserVM> gu)
        {
            List<GroupVM> groups = new List<GroupVM>();
            foreach (var g in gu)
            {
                groups.Add(new GroupVM() { Id = g.GroupId, Name = g.Group.Name });
            }
            return groups;
        }

        public List<UserLogVM> users = new List<UserLogVM>();
        public List<UserLogVM> ToUserLog(List<UserLogTimeVM> u)
        {
            users = new List<UserLogVM>();
            foreach (var ul in u)
            {
                users.Add(new UserLogVM
                {

                    Id = ul.Id,
                    UserId = ul.UserId,
                    UserName = ul.User.UserName,

                    ClientCenterId = ul.User.ClientCenterId,
                    ClientCenter = ul.User.ClientCenter,

                    Groups = GroupsList(ul.User.GroupUsers),

                    ProviderKey = ul.ProviderKey,
                    LoginTimeStr = ul.LoginTime,
                    LogoutTimeStr = ul.LogoutTime,

                });

            }
            return users;
        }
        public UserLogVM ToUser(UserVM u, UserLogTimeExpVM ul)
        {

            return new UserLogVM()
            {
                Id = ul.Id,
                UserId = ul.UserId,
                UserName = u.UserName,
                ClientCenterId = u.ClientCenterId,
                ClientCenter = u.ClientCenter,

                Groups = GroupsList(u.GroupUsers),

                ProviderKey = ul.ProviderKey,
                LoginTimeStr = fpub.convert2shamsiSTime(ul.LoginTime),
                LogoutTimeStr = fpub.convert2shamsiSTime(ul.LogoutTime),

            };



        }
    }
}