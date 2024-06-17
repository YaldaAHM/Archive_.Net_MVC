using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YA.Archive.Service.FileMdl.VMs;


namespace YA.Archive.Service.FileMdl.FileStorage
{
    public class HttpFileCollectionBaseAdapter:IFileStorage
    {
        public FileInfoVM<TFile> Store<TRequest, TRootPath, TFile>
            (TRequest request, string rootPath, string fileCode, bool isPartialFile) // where TRequest :HttpRequestBase
        {
            var fileInfo = new FileInfoVM<TFile>();
        
                var thf = default(TRequest);
                thf = request  ;
                var t = thf as HttpRequestBase;
               var file= StoreFile2<HttpRequestBase,TRootPath,TFile>(t, rootPath, fileCode, isPartialFile);
            return file;
        }

        public bool Remove(string rootPath,string fileName )
        {
            var filePath = Path.Combine(rootPath, fileName);
               if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }

        public T Retrieve<T>()
        {
            throw new NotImplementedException();
        }
       private FileInfoVM<TFile> StoreFile<TRequest, TRootPath, TFile>
            (TRequest request, TRootPath rootPath, bool isPartialFile) where TRequest :HttpRequestBase
        {
 Type t = typeof(TRequest); 

            var tf = Convert.ChangeType(t, typeof(HttpRequestBase));
            var httpRequest = (HttpRequestBase)tf;
            var file = httpRequest.Files[0];

           
            var r = new List<ViewDataUploadFilesResult>();

            var statuses = new List<ViewDataUploadFilesResult>();
          
            var origFileName = file.FileName;
            var fileName = file.FileName;
      
            var tr = default(TRootPath);
            tr = rootPath;
            var ttr = tr as string;
            if (!isPartialFile)
            {
             
                UploadWholeFile(ttr,fileName, file, statuses);
            }
            else
            {
               
                UploadPartialFile(ttr,fileName, file, statuses);
            }

            
            var fileInfo = new FileInfoVM<TFile>()
            {
                FileName = file.FileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                EncodeFile = EncodeFile(file.FileName)
            };
            return fileInfo;
        }

        private FileInfoVM<TFile> StoreFile2<TRequest, TRootPath, TFile>
            (TRequest request, string rootPath, string fileCode, bool isPartialFile) where TRequest :HttpRequestBase
        {
            var fileInfo=new FileInfoVM<TFile>();
           
            var file = request.Files[0];
           
            var r = new List<ViewDataUploadFilesResult>();

            
            var statuses = new List<ViewDataUploadFilesResult>();
          
            var origFileName = file.FileName;
            var fileName = fileCode+"-" +file.FileName;
           
            if (!isPartialFile)
            {
                 UploadWholeFile(rootPath, fileName, file, statuses);
            }
            else
            {
                 UploadPartialFile(rootPath,fileName, file, statuses);
            }

         
            fileInfo = new FileInfoVM<TFile>()
            {
             
                FileName = fileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                EncodeFile =statuses[0].thumbnail_url,
                OrigName=origFileName,

            };
            return fileInfo;
        } 


        private string StorageRoot(string rootPath)
        {
            return rootPath;
        }
        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        //private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        private void UploadPartialFile(string rootPath, string fileName, HttpPostedFileBase file, List<ViewDataUploadFilesResult> statuses)

        {
            if (file != null) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
             var inputStream = file.InputStream;
            var fullName = Path.Combine(StorageRoot(rootPath), Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/FileUpload/Download/" + fileName,
                delete_url = "/FileUpload/Delete/" + fileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
                message = "registrated"
            });
            //return statuses;
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        //private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        private void UploadWholeFile(string rootPath, string fileName,HttpPostedFileBase file, List<ViewDataUploadFilesResult> statuses)
        {
              var fullPath = Path.Combine(StorageRoot(rootPath), Path.GetFileName(fileName));

            file.SaveAs(fullPath);

            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = file.FileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/FileUpload/Download/" + file.FileName,
                delete_url = "/FileUpload/Delete/" + file.FileName,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                delete_type = "GET",
                message = "registrated",
            });
            
        }

        
    }


    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
        public string message { get; set; }
    }
}
