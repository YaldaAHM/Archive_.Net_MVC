using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.FileMdl.VMs
{
    public class FileInfoVM<TFileType>
    {
        public TFileType File { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string EncodeFile { get; set; }
        public string OrigName { get; set; }
    }
}
