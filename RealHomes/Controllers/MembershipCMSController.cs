using System;
using System.Web.Mvc;
using System.Collections.Generic;

using System.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;
using RealHomes.Helper;
using umbraco.cms.businesslogic.web;
using System.Web.Security;

using System.Threading.Tasks;
using System.Web.Configuration;
using RealHomes.Models;
using Umbraco.Web.Models;
using Umbraco.Core.Persistence.Mappers;

namespace RealHomes.Controllers
{
    public class MembershipCMSController : SurfaceController
    {
        private const string REGISTER_VIEW_NAME = "Membership/_RegisterMember";
        private const string LOGIN_VIEW_NAME = "Membership/_LoginMember";
        private const string LOGOUT_VIEW_NAME = "Membership/_LogoutMember";


        public ActionResult RenderLogin()
        {
            return PartialView(LOGIN_VIEW_NAME, new MembershipLoginCMSModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MembershipLoginCMSModel model,string returnURL)
        {
            if (ModelState.IsValid)
            {
                if (Members.Login(model.UserName, model.Password))
                {
                   var LoggedInMember = Services.MemberService.GetByUsername(model.UserName);

                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    Session[StringConstants.CURRENT_USER_OBJECT] = LoggedInMember;
                    Session[StringConstants.CURRENT_USER_NAME] = LoggedInMember.Name;

                    UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
                    if (returnURL!="" && myHelper.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        return Redirect(StringConstants.HOME_ADDRESS_UAE);
                    }
                }
                else
                {
                    TempData["LoginResult"] = StringConstants.LOGIN_FAILURE_MSG;

                }
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                TempData["LoginResult"] = StringConstants.LOGIN_FAILURE_MSG;
                return CurrentUmbracoPage();
            }
        }



        public ActionResult RenderLogout()
        {
            return PartialView(LOGOUT_VIEW_NAME, new MembershipLoginCMSModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Members.Logout();
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
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

                    newModel.Username = model.UserName;
                    newModel.UsernameIsEmail = false;
                    newModel.Name = model.UserName;
                    newModel.Password = model.Password;
                    newModel.Email = model.UserEmail;

                    Members.RegisterMember(newModel, out result);

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
                    MembershipCMSModel model = new MembershipCMSModel();
                    
                    
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
    }
}