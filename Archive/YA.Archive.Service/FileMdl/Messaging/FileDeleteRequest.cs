using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileDeleteRequest : BaseEntityRequest<FileVM>
    {
        public string RequsetCurrentUserId { get; set; }
        public string FileName { get; set; }
        public string RootPath { get; set; }
    }
}
