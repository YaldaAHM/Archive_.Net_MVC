using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using YA.Archive.Infrastructure.Infrastructure.Folder;
using YA.Archive.Model;
using YA.Archive.Model.Directory;
using YA.Archive.Service.DirectoryMdl.VMs;

namespace YA.Archive.Service.DirectoryMdl.Imps
{
    public class DirectoryService
    {
        private ArchiveDataContext _db;

        public DirectoryService()
        {
            _db = new ArchiveDataContext();
        }

        public bool CreateDirectory(string year, string projectName)
        {
            var root = _db.Directory.ToList();
            string path;
            bool isDir = false;
            foreach (var r in root)
            {

                string pp = Path.Combine(r.Name + FolderName.MainFolder + year);
                if (Directory.Exists(Path.Combine(pp, projectName)))
                {
                    return true;
                }
            }
            if (!isDir)
            {
                var p = _db.Directory.ToList().OrderByDescending(m => m.Id).FirstOrDefault().Name;
                DirectoryInfo diproject = Directory.CreateDirectory(Path.Combine(p + FolderName.MainFolder + year, projectName));

                if (diproject.Exists)
                    return true;
                else
                {
                    return false;
                }
            }

            return false;
        }
        public bool DeleteDirectory(string path,string year, string projectName)
        {
            if (Directory.Exists(Path.Combine(path + FolderName.MainFolder + year, projectName)))
            {
                Directory.Delete(Path.Combine(path + FolderName.MainFolder + year, projectName));

                return true;
            }
                else
                {
                    return true;
                }
            
        }



        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                            Rights,
                                                            ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }

        public DirectoryVM Insert(DirectoryVM request)
    {
        try
        {
            if (_db.Directory.Any(n => n.Name==request.Name))
            {
                return new DirectoryVM
                {
                    
                };
            }
            var d = new DirectoryModel() {Name = request.Name};
            var c = _db.Directory.Add(d);
            var cl = _db.SaveChanges();
           
            return new DirectoryVM()
            {
                Id=c.Id,
                Name=c.Name
            };

        }
        catch (Exception ex)
        {
                return new DirectoryVM();

            }
    }


    public DirectoryVM Update(DirectoryVM request)
    {
        try
        {

            var c = _db.Directory.FirstOrDefault(n => n.Id == request.Id);
            var folders = _db.Folder.Where(x => x.Path == c.Name).ToList();
            foreach (var f in folders)
            {
              var ff=  _db.Folder.FirstOrDefault(x => x.Id == f.Id);
                ff.Path = request.Name;
                    _db.SaveChanges();
                }
            c.Name = request.Name;
            _db.SaveChanges();
            return new DirectoryVM
            {
                Id=request.Id,
                Name=request.Name
            };

        }
        catch (Exception ex)
        {

            return new DirectoryVM
            {
               Id=request.Id,Name=request.Name

            };
        }
    }

    public DirectoryVM DeleteById(DirectoryVM request)
    {
        try
        {

            var c = _db.Directory.Find(request.Id);
            _db.Directory.Remove(c);
                return new DirectoryVM();

        }
        catch (Exception ex)
        {

                return new DirectoryVM();
            }
    }
    public DirectoryVM Delete(DirectoryVM request)
    {
        try
        {

            var c = _db.Directory.Find(request.Id);
            _db.Directory.Remove(c);
            _db.SaveChanges();
            return new DirectoryVM
            {
                
            };

        }
        catch (Exception ex)
        {

            return new DirectoryVM
            {
               

            };
        }
    }
    public List<DirectoryVM> FindAll(DirectoryVM request)
    {

        var ents = _db.Directory.ToList();
            var l=new List<DirectoryVM>();
        foreach (var en in ents)
        {
            l.Add(new DirectoryVM()
                  {
                      Id=en.Id,
                      Name=en.Name
                  });
        }
        return l;

      
    }


    public DirectoryVM FindById(DirectoryVM request)
    {
        try
        {
            var entity = _db.ClientCenter.Find(request.Id);
            return new DirectoryVM
            {
               Id=entity.Id,
                Name=entity.Name
            };

        }
        catch (Exception ex)
        {

            return new DirectoryVM
            {
                

            };
        }
    }
}
}
