using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YA.Archive.Service.UserManagementMdl.VMs;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace YA.Archive.MVC2.Models
{
    public  class CurrentUser:Controller
    {
        private UserManagementService _UserService;

        public  CurrentUser()
        {
            _UserService = new UserManagementService();
        }
       
        public UserVM User()
        {

            var currentUser = System.Web.HttpContext.Current.User.Identity.GetUserId();
             var result =  _UserService.FindById(
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