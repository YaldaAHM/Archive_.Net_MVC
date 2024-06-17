using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.FileMdl.FileStorage
{
    public interface IFileType<T>
    {
        T FileType { get; set; }

    }
}
