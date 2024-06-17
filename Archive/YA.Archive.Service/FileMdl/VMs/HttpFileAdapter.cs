using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YA.Archive.Service.FileMdl.VMs
{
    public class HttpFileAdapter : ITFile
    {
        HttpFileCollectionBase ITFile.File<HttpFileCollectionBase>()
        {
            throw new NotImplementedException();
        }
    }
}
