using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Service.FileMdl.VMs;

namespace YA.Archive.Service.FileMdl.FileStorage
{
    public class NullObjectFile:IFileStorage
    {
        public FileInfoVM<TFile> Store<T, TRootPath, TFile>(T request, string rootPath, string fileCode, bool isPartialFile) //where T : IConvertible
        {
            throw new NotImplementedException();
        }

        public FileInfoVM<TFile> Store<TRootPath, TFile>(IConvertible request, TRootPath rootPath, bool isPartialFile)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string fileName,string filePath)
        {
            throw new NotImplementedException();
            
        }
        
        public T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

       
    }
}
