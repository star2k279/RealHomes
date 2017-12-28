using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace SmartBlog
{
    public class SmartSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult SubmitComment(SmartBlog.Comment Model)
        {
            // Model not valid, do not save, but return current Umbraco page.
            if (!ModelState.IsValid || Model.intId == 0)
            {
                return CurrentUmbracoPage();
            }

            String strModeratorEmail = SmartBlogLibraries.Global.GetConfig().GetElementsByTagName("moderatorCommentEmail")[0].InnerText;
            String strRobotEmail = SmartBlogLibraries.Global.GetConfig().GetElementsByTagName("robotEmail")[0].InnerText;
            Boolean blnAutoApproveComments = Boolean.Parse(SmartBlogLibraries.Global.GetConfig().GetElementsByTagName("autoApproveComments")[0].InnerText);

            Dictionary<String, Object> colProperties = new Dictionary<String, Object>() { 
                    {"smartBlogName", HttpUtility.UrlDecode(Model.strName)},
                    {"smartBlogEmail", HttpUtility.UrlDecode(Model.strEmail)},
                    {"smartBlogWebsite", HttpUtility.UrlDecode(Model.strWebsite)},
                    {"smartBlogComment", HttpUtility.UrlDecode(Model.strComment)}
                };

            SmartBlogLibraries.Helpers.Cms.CreateContent(Model.strName, "SmartBlogComment", Model.intId, colProperties, blnAutoApproveComments);

            IPublishedContent objNode = SmartBlogLibraries.Global.objUmbHelper.TypedContent(Model.intId);

            if (!String.IsNullOrEmpty(strModeratorEmail))
            {
                StringBuilder objEmailBody = new StringBuilder();
                objEmailBody.AppendLine("This is an automated message; please do not reply.");
                objEmailBody.AppendLine("--------------------------------------------------");
                objEmailBody.AppendLine();
                objEmailBody.AppendLine(Model.strName + " posted a comment on '" + objNode.Name + "' at " + DateTime.Now.ToString() + " with the following text:");
                objEmailBody.AppendLine(Model.strComment);
                objEmailBody.AppendLine();
                objEmailBody.AppendLine("--------------------------------------------------");
                objEmailBody.AppendLine();
                objEmailBody.AppendLine("The comment was posted here: http://" + System.Web.HttpContext.Current.Request.Url.Host + SmartBlogLibraries.Global.objUmbHelper.NiceUrl(objNode.Id) + " please log into the content management system to approve it and make it visible on the website.");
                objEmailBody.AppendLine();
                objEmailBody.AppendLine("Regards,");
                objEmailBody.AppendLine("Support");

                try
                {
                    SmartBlogLibraries.Helpers.Mailing.SendEmail(strModeratorEmail, strRobotEmail, "Comment Added - Website Name", objEmailBody.ToString(), Model.strEmail);
                }
                catch (Exception) { }
            }

            // Redirect to current page.
            return RedirectToCurrentUmbracoPage();
        }
    }

    /// <summary>
    /// Container for comments that are posted back.
    /// </summary>
    public class Comment
    {
        [Required(ErrorMessage = "Error getting post ID")]
        public Int32 intId { get; set; }

        [Display(Name = "Name:")]
        [Required(ErrorMessage = "Name is required")]
        public String strName { get; set; }

        [Display(Name = "Website:")]
        public String strWebsite { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public String strEmail { get; set; }

        [Display(Name = "Date:")]
        public String strDate { get; set; }

        [Display(Name = "Comment:")]
        [Required(ErrorMessage = "Comment is required")]
        public String strComment { get; set; }

        [Display(Name = "Security Question: What is 2 + 2?")]
        [Required(ErrorMessage = "Security question is required")]
        [RegularExpression(@"^4$", ErrorMessage = "Invalid answer to security question")]
        public String strSecurity { get; set; }
    }
}