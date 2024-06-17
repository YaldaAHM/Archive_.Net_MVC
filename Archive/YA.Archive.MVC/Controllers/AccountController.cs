using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Imps;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.Messaging;
using YA.Archive.Service.UserActivity.UserLogTimeMdl.VMs;
using YA.Archive.Service.UserManagementMdl.VMs;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using YA.Archive.Service.UserManagementMdl.Configuration;
using YA.Archive.Service.UserManagementMdl.Imps;
using YA.Archive.Service.UserManagementMdl.Messaging;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity.Owin;

namespace YA.Archive.MVC2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        private UserManagementService _UserService;
        private UserLogTimeService _userLogTimeService;
        public AccountController()
        {
            _UserService = new UserManagementService();
        }

        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            if (ident == null)
            {
                return View("Error", new string[] { "No claims available" });
            }
            else
            {
                return View(ident.Claims);
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] {"Access Denied"});
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM model, string returnUrl)
        {
           
            if(model==null || model.Password==null || model.UserName==null || model.Password=="" || model.UserName=="")
            {
                ViewBag.returnUrl = returnUrl;
                return View("Login", model);
            }
            var result = new UserLoginResponse();
            result = _UserService.Login(
                new UserLoginRequest {entity = model});
            if (result.IsSuccess)
            {
                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                                   {
                                       IsPersistent = false
                                   }, result.ident);
             
                
                string strSessionID;
                strSessionID = System.Web.HttpContext.Current.Session.SessionID;
         
                _userLogTimeService = new UserLogTimeService();
                  if (strSessionID != null)
                {
                    Session[result.entity.Id] = Guid.NewGuid();
                    var response = _userLogTimeService.Insert(
                        new UserLogTimeInsertRequest()
                        {
                            entity = new UserLogTimeExpVM()
                            {
                                LoginTime = DateTime.Now,
                                LogoutTime = DateTime.Now.AddMinutes(30),
                                UserId = result.entity.Id,
                                ProviderKey = Session[result.entity.Id].ToString() // strSessionID,
                            }
                        });
                }


                if (returnUrl != "")
                {
                    ViewBag.MessageLogin = result.Message;
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.returnUrl = returnUrl;
                ViewBag.MessageLogin = result.Message;
                return View("Login", model);
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            SessionIDManager manager = new SessionIDManager();
            string newSessionId = manager.GetSessionID(System.Web.HttpContext.Current);
            _userLogTimeService = new UserLogTimeService();
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
      
            string strSessionID;
            strSessionID = System.Web.HttpContext.Current.Session.SessionID;
            if (strSessionID != null)
            {
                if (Session[userId]!=null)
                {
                    
              
                var provideKey = Session[userId].ToString();// strSessionID;
            var response = _userLogTimeService.FindByExpression(
                new UserLogTimeFindByExpressionRequest()
                {
                    expression = x => x.ProviderKey == provideKey &&
                      x.UserId == userId
                });

            if (response != null && response.Entities.Count() != 0)
            {
                var l = response.Entities.FirstOrDefault();
                l.LogoutTime =fpub.convert2shamsiS( DateTime.Now);
                _userLogTimeService.Update(
                    new UserLogTimeUpdateRequest()
                    {
                        entity = new UserLogTimeExpVM()
                                 {
                                     Id=l.Id,
                                     LoginTime= fpub.convert2miladi(l.LoginTime),
                                     LogoutTime=DateTime.Now,
                                     UserId=l.UserId,
                                     ProviderKey=l.ProviderKey
                                 }
                    });
            }   }}
            AuthManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
        
            }
        }

    }
}