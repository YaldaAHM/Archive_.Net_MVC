using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YA.Archive.Service.FileMdl.FileStorage
{
    public class HttpFileTypeAdapter : IFileType<HttpFileCollectionBase>
    {
        public HttpFileCollectionBase FileType 
        {
            get;
            set;
        }
    }
}
   
