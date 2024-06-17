using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.Reporting.RCommentMdl.VMs
{
    public class ReportCFCommentRVM
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int CommentId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateUserName { get; set; }
        public string LastUpdateDate { get; set; }
        public string LastUpdateUserName { get; set; }
        public ReportFolderRVM ReportFolderRVM { get; set; }
    }
}
