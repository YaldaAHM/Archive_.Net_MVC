using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileUpdateRequest : BaseEntityResponse<FileVM>
    {
        public string RequsetCurrentUserId { get; set; }
    }
}
