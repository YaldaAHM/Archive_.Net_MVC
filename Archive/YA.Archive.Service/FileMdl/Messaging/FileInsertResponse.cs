using YA.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;
using YA.Archive.Service.FileMdl.FileStorage;

namespace YA.Archive.Service.FileMdl.Messaging
{
    public class FileInsertResponse<T> : BaseEntityResponse<FileVM>
    {
        public FileInfoVM<T> FileInfo { get; set; }
      //  public TFileType File { get; set; }
        //public string FileName { get; set; }
        //public int FileSize { get; set; }
        //public string FileType { get; set; }
        //public string EncodeFile { get; set; }

    }
}
