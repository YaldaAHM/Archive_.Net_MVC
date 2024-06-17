using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Models
{
    public static class CUser
    {
        private static UserManagementService _UserService;

        
        public static UserVM User()
        {
            _UserService = new UserManagementService();
             var currentUser = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var result = _UserService.FindById(
                new UserFindByIdRequest
                {
                    Id = currentUser
                });
            if (result.entity != null)
            {
                return result.entity;
            }
            return null;

        }

    }
}