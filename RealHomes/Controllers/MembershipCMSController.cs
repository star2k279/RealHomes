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

namespace RealHomes.Controllers
{
    public class MembershipCMSController : SurfaceController
    {
        private const string REGISTER_VIEW_NAME = "Membership/_RegisterMember";
        private const string LOGIN_VIEW_NAME = "Membership/_LoginMember";


        [HttpGet]
        public ActionResult MemberLogin()
        {
            return PartialView(LOGIN_VIEW_NAME, new MembershipCMSModel());
        }

        [HttpPost]
        public ActionResult MemberLogin(MembershipCMSModel model,string returnURL)
        {
            if (ModelState.IsValid)
            {
                if (Members.Login(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.DisplayName, false);
                    //Session[StringConstants.CURRENT_USER_OBJECT] = userObject;
                    UrlHelper myHelper = new UrlHelper(HttpContext.Request.RequestContext);
                    if (myHelper.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        return Redirect("~/login/");
                    }
                }
                else
                {
                    TempData["LoginResult"] = "Login failed.";

                }
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                return CurrentUmbracoPage();
            }
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

    }
}