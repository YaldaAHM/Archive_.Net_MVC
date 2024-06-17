using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.FileStorage
{
    public interface IFileStorage
    {
        FileInfoVM<TFile> Store<TRequest, TRootPath, TFile>(TRequest request, string rootPath,string fileCode, bool isPartialFile);
        bool Remove(string fileName,string filePath);
        T Retrieve<T>();
      

    }
}
