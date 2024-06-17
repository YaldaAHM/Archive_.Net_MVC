using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YA.Archive.Service.UserManagementMdl.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
//using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        
       
        private UserManagementService _UserService;

        public AdminController()
        {
            _UserService = new UserManagementService();
        }
       
        public ActionResult Index()
        {
           
            var user = _UserService.FindAll(
               new UserFindAllRequest { });
            if (user.IsSuccess)
                return View("Index", user.Entities);
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RegistrationVM model)
        {
            if (ModelState.IsValid)
            {
                
                var result = new UserInsertResponse();
                result =  _UserService.Insert(
                     new UserInsertRequest { entity = model });
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //AddErrorsFromResult(result.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var result = _UserService.DeleteById(
                new UserDeleteByIdRequest { Id = id });
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }

            else
            {
                return View("Error", new string[] { "User Not Found" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            var result = _UserService.FindById(
                 new UserFindByIdRequest { Id = id });
            if (result.entity != null)
            {
                var user = new EditVM()
                {
                    Id = result.entity.Id,
                    Email = result.entity.Email,
                    UserName = result.entity.UserName,
                    Password = result.entity.PasswordHash
                };
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
          public async Task<ActionResult> Edit(EditVM model)
        {
            var result = new UserUpdateResponse();
            result = _UserService.Update(
                 new UserUpdateRequest { entity = model });
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(result.entity);
        }


        
    }
}