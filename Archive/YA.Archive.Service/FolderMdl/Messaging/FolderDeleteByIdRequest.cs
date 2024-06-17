using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Infrastructure.Service;

namespace YA.Archive.Service.FolderMdl.Messaging
{
    public class FolderDeleteByIdRequest : BaseIdRequest<int>
    {
        public string RequsetCurrentUserId { get; set; }
    }
}
