using YA.Archive.Service;
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
    public class MainClientCenterStrategy : IClientCenterStrategy
    {
        private  MainPermissionClaimsInitialize _mainPermissionClaimsInitialize;

        public MainClientCenterStrategy()
        {
            _mainPermissionClaimsInitialize = new MainPermissionClaimsInitialize();
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
            return _mainPermissionClaimsInitialize.PermissionClaimsInitializer();
        }
    }
}
