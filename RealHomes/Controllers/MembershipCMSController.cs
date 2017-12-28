using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using RealHomes;
using RealHomes.Helper;
using RealHomes.Models;
using RealHomes.Models.UmbracoIdentity;


using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core;
using Umbraco.Web.Models;
using UmbracoIdentity;
using Umbraco.Core.Persistence.Mappers;
using UmbracoIdentity.Models;
using IdentityExtensions = UmbracoIdentity.IdentityExtensions;

namespace RealHomes.Controllers
{
    public class MembershipCMSController : SurfaceController
    {

        private UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;
        private UmbracoMembersRoleManager<UmbracoApplicationRole> _roleManager;

        private const string REGISTER_VIEW_NAME = "Membership/_RegisterMember";
        private const string LOGIN_VIEW_NAME = "Membership/_LoginMember";
        private const string LOGOUT_VIEW_NAME = "Membership/_LogoutMember";


        public ActionResult RenderLogin()
        {
            return PartialView(LOGIN_VIEW_NAME, new MembershipLoginCMSModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(MembershipLoginCMSModel model,string returnURL)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, true);

                    var LoggedInMember = Services.MemberService.GetByUsername(model.UserName);

                    Session[StringConstants.CURRENT_USER_OBJECT] = LoggedInMember;
                    Session[StringConstants.CURRENT_USER_NAME] = LoggedInMember.Name;


                    /*UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
                    if (returnURL!="" && myHelper.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        return Redirect(StringConstants.HOME_ADDRESS_UAE);
                    }*/
                    return RedirectToCurrentUmbracoPage();
                }
            }
                TempData["LoginResult"] = StringConstants.LOGIN_FAILURE_MSG;
                return CurrentUmbracoPage();
           
        }



        public ActionResult RenderLogout()
        {
            return PartialView(LOGOUT_VIEW_NAME, new MembershipLoginCMSModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            if (Members.IsLoggedIn())
            {
                //ensure to only clear the default cookies
                OwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
                //redirect to current page by default
                TempData["LogoutSuccess"] = true;
                return RedirectToCurrentUmbracoPage();
            }

            //if there is a specified path to redirect to then use it
            //if (model.RedirectUrl.IsNullOrWhiteSpace() == false)
            //{
            //    return Redirect(model.RedirectUrl);
            //}
            //Members.Logout();
            //FormsAuthentication.SignOut();

            TempData["LogoutSuccess"] = true;
            return RedirectToCurrentUmbracoPage();
            
            
        }

        public ActionResult RenderRegisterForm()
        {
            return PartialView(REGISTER_VIEW_NAME, new MembershipCMSModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterMember(MembershipCMSModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            if (model != null)
            {
                MembershipCreateStatus result;
                RegisterModel newModel = Members.CreateRegistrationModel();

                //Save the record
                //By default alias will be 'Member'
                newModel.Username = model.UserName;
                newModel.UsernameIsEmail = false;
                newModel.Name = model.UserName;
                newModel.Password = model.Password;
                newModel.Email = model.UserEmail;

                Members.RegisterMember(newModel, out result);

                var memberGroup = Services.MemberGroupService.GetByName(model.GROUP_ALIAS);
                var member = Services.MemberService.GetByUsername(newModel.Username);

                member.SetValue(model.TYPE_PROPERTY_NAME, 360);
                /*member.SetValue("", model.ContactNo);
                member.SetValue("", model.countryValue);
                member.SetValue("", model.cityValue);*/
                Services.MemberService.Save(member); 

                TempData["RegisterResult"] = result.ToString();

                //if (result == MembershipCreateStatus.Success)
                //    return RedirectToCurrentUmbracoPage();

            }
            else
            {
                TempData["RegisterResult"] = "Registration failed.";
                //Error should be logged properly
            }

                return RedirectToCurrentUmbracoPage();
           
        }



        [HttpGet]
        public JsonResult GetUserByEmail(string sEmail)
        {
            try
            {
                IPublishedContent user = Members.GetByEmail(sEmail);

                if (user != null)
                {
                    return Json(true,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            } 
        }

        //GetUserByName
        [HttpGet]
        public JsonResult GetUserByName(string sName)
        {
            try
            {
                IPublishedContent user = Members.GetByUsername(sName);

                if (user != null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 
        /// </summary 
        /// <param name="sName"></param>
        /// <returns></returns>
        public MembershipLoginCMSModel GetMember(string sName)
        {
            try
            {
               var user = Services.MemberService.GetByUsername(sName);

                if (user != null)
                {
                    
                    //Fill the local model
                    MembershipLoginCMSModel loginModel = new MembershipLoginCMSModel();
                    
                    
                    
                    loginModel.DisplayName = user.Name;
                    loginModel.UserName = user.Username;
                    loginModel.Email = user.Email;
                                                 
                    return loginModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region External login and registration

        public UmbracoMembersUserManager<UmbracoApplicationMember> UserManager
        {
            get
            {
                return _userManager ?? (_userManager = OwinContext.GetUserManager<UmbracoMembersUserManager<UmbracoApplicationMember>>());
            }
        }

        public UmbracoMembersRoleManager<UmbracoApplicationRole> RoleManager
        {
            get
            {
                return _roleManager ?? (_roleManager = OwinContext
                    .Get<UmbracoMembersRoleManager<UmbracoApplicationRole>>());
            }
        }
        protected IOwinContext OwinContext
        {
            get { return Request.GetOwinContext(); }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            if (returnUrl.IsNullOrWhiteSpace())
            {
                //returnUrl = Request.RawUrl;
                returnUrl = StringConstants.HOME_ADDRESS_UAE;
           }

            // Request a redirect to the external login provider
            return new ChallengeResult(provider,
                 Url.Action("ExternalLoginCallback", "MembershipCMS", new { ReturnUrl = returnUrl }));
                //Url.SurfaceAction<MembershipCMSController>("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await OwinContext.Authentication.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                //go home, invalid callback
                return RedirectToLocal(returnUrl);
                
            }

                  
            var user = await UserManager.FindByEmailAsync(loginInfo.Email);
                        
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                //return PartialView("Membership/_ExternalLoginConfirmationMember", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                return PartialView("Membership/_ExternalLoginConfirmationMember", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                //go home, already authenticated
                return RedirectToLocal(returnUrl);
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await OwinContext.Authentication.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return PartialView("Membership/_ExternalLoginFailureMember");
                }

                var user = new UmbracoApplicationMember()
                {
                    Name = info.ExternalIdentity.Name,
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddModelErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider, string returnUrl = null)
        {
            if (returnUrl.IsNullOrWhiteSpace())
            {
                returnUrl = Request.RawUrl;
            }

            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider,
                Url.Action("LinkLoginCallback", "MembershipCMS", new { ReturnUrl = returnUrl }),
                User.Identity.GetUserId());
            //Url.SurfaceAction<MembershipCMSController>("LinkLoginCallback", new { ReturnUrl = returnUrl })
        }

        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                TempData["LinkLoginError"] = new[] { "An error occurred, could not get external login info" };
                return RedirectToLocal(returnUrl);
            }
            var result = await UserManager.AddLoginAsync(UmbracoIdentity.IdentityExtensions.GetUserId<int>(User.Identity), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            TempData["LinkLoginError"] = result.Errors.ToArray();
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(
                UmbracoIdentity.IdentityExtensions.GetUserId<int>(User.Identity),
                new UserLoginInfo(loginProvider, providerKey));

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(UmbracoIdentity.IdentityExtensions.GetUserId<int>(User.Identity));
                await SignInAsync(user, isPersistent: false);
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                AddModelErrors(result);
                return CurrentUmbracoPage();
            }
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(UmbracoIdentity.IdentityExtensions.GetUserId<int>(User.Identity));
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView(linkedAccounts);
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

        private async System.Threading.Tasks.Task SignInAsync(UmbracoApplicationMember member, bool isPersistent)
        {
            OwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            OwinContext.Authentication.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent },
                await member.GenerateUserIdentityAsync(UserManager));
                     
        }

       

        private void AddModelErrors(IdentityResult result, string prefix = "")
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(prefix, error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(UmbracoIdentity.IdentityExtensions.GetUserId<int>(User.Identity));
            if (user != null)
            {
                return !user.PasswordHash.IsNullOrWhiteSpace();
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect(StringConstants.HOME_ADDRESS_UAE);
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri, string userId = null)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            private string LoginProvider { get; set; }
            private string RedirectUri { get; set; }
            private string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
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