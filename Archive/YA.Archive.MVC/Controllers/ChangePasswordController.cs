using YA.Archive.Service.UserManagementMdl.Imps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YA.Archive.MVC2.Models;
using YA.Archive.Service.UserManagementMdl.Messaging;
using YA.Archive.Service.UserManagementMdl.VMs;
using Microsoft.AspNet.Identity;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        private UserManagementService _userService;

        public ChangePasswordController()
        {
            _userService = new UserManagementService();
        }
        public ActionResult Index()
        {
            var mapSites = new List<SiteRouting>
                           {
                               new SiteRouting()

                               {
                                   Subject = "Home",
                                   Url="/Home/Index"
                                   
                               },
                               new SiteRouting()

                               {
                                   Subject ="Change Password ",
                                   Url="/ChangePassword/Index"
                                  
                               },
                              
                           };
            ViewData["ChangePasswordSiteMap"] = mapSites;
            var user=  _userService.FindById(new UserFindByIdRequest()
                                  {
                                      Id =
                                          System.Web.HttpContext.Current.User.Identity
                                          .GetUserId(),
                                  });
              if (user != null)
            {
                var u = new EditVM()
                        {
                            Id = user.entity.Id,
                            UserName = user.entity.UserName
                        };
                return View( u);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
       

        [Authorize]
        public ActionResult Edit(string id)
        {
            var user = _userService.FindById(new UserFindByIdRequest()
            {
                Id =
                                            System.Web.HttpContext.Current.User.Identity
                                            .GetUserId(),
            });
            
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [Authorize]
        [HttpPost]
        public  ActionResult Index(string id, string oldpassword, string password, string repeatpassword)
        {
            var mapSites = new List<SiteRouting>
                           {
                               new SiteRouting()

                               {
                                   Subject = "Home",
                                   Url="/Home/Index"
                                  
                               },
                               new SiteRouting()

                               {
                                   Subject ="Change Password ",
                                   Url="/ChangePassword/Index"
                              
                               },

                           };
            ViewData["ChangePasswordSiteMap"] = mapSites;
            if (oldpassword == "" || password == "" || repeatpassword == "")
            {
                ViewBag.MessageChangePass = "Please enter the information correctly.";
                return View("Index");
            }
            if ( password != repeatpassword )
            {
                ViewBag.MessageChangePass = "Please enter the correct password.";
                return View("Index");
            }
           var user = _userService.FindById(new UserFindByIdRequest()
            {
                Id = System.Web.HttpContext.Current.User.Identity
                                         .GetUserId(),
            });
            if (user != null)
            {
                var result = _userService.ChangePassword(new UserUpdateRequest()
                                                         {
                    RequsetCurrentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                    OldPassword = oldpassword,
                                                             entity =
                                                             {
                                                                 Id = System.Web.HttpContext.Current.User.Identity
                                                                     .GetUserId(),
                                                                 Password = password
                                                             }
                                                         });
                if (result.IsSuccess)
                {
                    ViewBag.MessageChangePass = "Your password has been successfully entered.";
                    return View("Index");
                }
                else
                {
                    ViewBag.MessageChangePass = "Please enter the information correctly.";
                }
               
            }
            else
            {
                ViewBag.MessageChangePass = "Please enter the information correctly.";
           
            }
            return View("Index");
        }
    }
}