using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using RealHomes.Models;
using System.Net.Mail;
using System.Configuration;

namespace RealHomes.Controllers
{
    public class ContactSurfaceController:SurfaceController
    {
        
        private const string PARTIAL_VIEW_NAME = "_ContactUs.cshtml";

        public ActionResult RenderForm()
        {
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_NAME);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ContactModel objContactModel)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            SendEmail(objContactModel);
            TempData["ContactSuccess"] = true;

            return RedirectToCurrentUmbracoPage();
            
        }

        private void SendEmail(ContactModel objContactModel)
        {
            try
            {
                //Retrieve the mail server host setting from the web.config file.
                var credential = new System.Net.Configuration.SmtpSection().Network;

                MailMessage newMailMessage = new MailMessage(objContactModel.txtEmail, StringConstants.COMPANY_EMAIL);

                newMailMessage.Subject = string.Format("Enquiry From {0} - {1}", objContactModel.txtName, objContactModel.txtSubject);
                newMailMessage.Body = objContactModel.txtMessage;
                
                SmtpClient mailClient = new SmtpClient(credential.Host);

                mailClient.Send(newMailMessage);
                return;
            }
            catch (Exception ex)
            {
                //we might want to log the error here 
                return;
            }
        }
    }
}