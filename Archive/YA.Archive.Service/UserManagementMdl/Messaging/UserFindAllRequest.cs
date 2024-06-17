using YA.Archive.Infrastructure.ClientCenterStrategy;
using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.UserManagementMdl.Messaging
{
    public class UserFindAllRequest : BasePagingRequest
    {
        public string RequsetCurrentUserId { get; set; }
        public int RequsetCurrentClientCenterId { get; set; }
        //public ClientCenterType ClientCenterType { get; set; }
    }
}
