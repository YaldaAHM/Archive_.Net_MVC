using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Archive.Service.GroupMdl.VMs
{
    public class GroupModificationVM
    {
        public virtual GroupVM Group { get; set; }
        public int GroupId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
