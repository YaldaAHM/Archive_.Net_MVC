using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.FileStorage;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileInsertRequest<TRequest, TRootPath>  :BaseEntityRequest<FileVM> //where TFileType: IConvertible 

    {
        public string RequsetCurrentUserId { get; set; }
        public TRequest Request { get; set; }
        public TRootPath RootPath { get; set; }
        public bool IsPartialFile { get; set; }
    }
}
