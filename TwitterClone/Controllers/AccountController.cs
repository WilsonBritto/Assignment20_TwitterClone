using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TwitterClone.Models;
using BLL;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TModel;

namespace TwitterClone.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        Person BllEntity = new Person();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            int count = 0;
            string res = "";
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Initialization.    
             List<TwitterLogin_MResult> loginInfo = (List<TwitterLogin_MResult>)BllEntity.verifyLogin(model.UserName, model.Password);

            //// Verification.    
            if (loginInfo != null && loginInfo.Count() > 0)
            {
                // Initialization.    
                var logindetails = loginInfo.First();
                // Login In.    
                this.SignInUser(logindetails.User_Id, 1, false);
                // setting.    
                this.Session["role_id"] = 1;
                // Info.    
                return this.RedirectToLocal(returnUrl);
            }

            if (count > 0)
            {
                res = "Success";
            }
            else
            {
                res = "Failure";
            }
            switch (res)
            {
                case "Success":
                    return RedirectToLocal(returnUrl);
                case "Failure":
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
                

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                TRegisterModel m = new TRegisterModel();
                m.User_Id = model.UserName;
                m.FullName = model.FullName;
                m.Email = model.Email;
                m.Password = model.Password;
                //var user = new TRegisterModel { User_Id = model.User_Id, Email = model.Email, Password=model.Password, FullName = model.FullName };
                var result = BllEntity.AddbUser(m);
                if (result > 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public PartialViewResult UserProfile()
        {
            List<TwitterLogin_MResult> Persons = (List<TwitterLogin_MResult>)BllEntity.GetPersons();

            var data = (from p in Persons
                        where p.User_Id.ToLower() == User.Identity.Name.ToLower()
                        select p).First();

            ManageProfile mp = new ManageProfile();
            mp.UserName = data.User_Id;
            mp.Email = data.Email;
            mp.FullName = data.FullName;

            return PartialView(mp);
        }

        [HttpPost]
        public ActionResult UserProfile(ManageProfile mp)
        {
            BllEntity.UpdateUserbDetails(mp.UserName, mp.FullName, mp.Email);
            return RedirectToAction("Index", "Home");
        }

        public PartialViewResult PasswordManager()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PasswordManager(ChangePasswordViewModel cvw)
        {
            BllEntity.ChangebPwd(User.Identity.Name, cvw.NewPassword);
            return RedirectToAction("Index", "Home");
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
     
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        

        #region Sign In method.    
        /// <summary>    
        /// Sign In User method.    
        /// </summary>    
        /// <param name="username">Username parameter.</param>    
        /// <param name="role_id">Role ID parameter</param>    
        /// <param name="isPersistent">Is persistent parameter.</param>    
        private void SignInUser(string username, int role_id, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                // Setting    
                claims.Add(new Claim(ClaimTypes.Name, username));
                //claims.Add(new Claim(ClaimTypes.Role, role_id.ToString()));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}