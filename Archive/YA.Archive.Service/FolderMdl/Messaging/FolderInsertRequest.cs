using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FolderMdl.VMs;

namespace YA.Archive.Service.FolderMdl.Messaging
{
    public class FolderInsertRequest : BaseEntityRequest<FolderVM>
    {
        public string RequsetCurrentUserId { get; set; }
    }
}
