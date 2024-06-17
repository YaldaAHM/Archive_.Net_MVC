
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Infrastructure.Infrastructure.Permission;
using YA.Archive.Model;
using YA.Archive.Model.Folder;
using YA.Archive.Service.FolderMdl.Mapping;
using YA.Archive.Service.FolderMdl.Messaging;
using YA.Archive.Service.FolderMdl.VMs;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;

namespace YA.Archive.Service.UserManagementMdl.PermissionClaimsMdl.Sterategy
{
    public class OtherClientCenterStrategy : IClientCenterStrategy
    {
        private PermissionClaimsInitialize _permissionClaimsInitialize;

        public OtherClientCenterStrategy()
        {
            _permissionClaimsInitialize = new PermissionClaimsInitialize();
        }

        public List<PermissionClaimsModel> ApplyFindAllTo(string request)
        {
            throw new NotImplementedException();
        }

        public List<PermissionClaimsModel> ApplyFindByExpressionTo(string requestExp)
        {
            throw new NotImplementedException();
        }

        public List<PermissionClaimsModel> ApplyFindAllTo()
        {
            return _permissionClaimsInitialize.PermissionClaimsInitializer();
        }
    }
}
