using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using UmbracoIdentity;
using RealHomes.Models.UmbracoIdentity;
using RealHomes;
using Owin;
using Umbraco.Web;
using Umbraco.Web.Security.Identity;
using UmbracoIdentity.Models;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using System.Threading.Tasks;

[assembly: OwinStartup("UmbracoIdentityStartup", typeof(UmbracoIdentityStartup))]

namespace RealHomes
{
   
    /// <summary>
    /// OWIN Startup class for UmbracoIdentity 
    /// </summary>
    public class UmbracoIdentityStartup : UmbracoDefaultOwinStartup
    {
        /// <summary>
        /// Configures services to be created in the OWIN context (CreatePerOwinContext)
        /// </summary>
        /// <param name="app"/>
        protected override void ConfigureServices(IAppBuilder app)
        {
            base.ConfigureServices(app);

            //Single method to configure the Identity user manager for use with Umbraco
            app.ConfigureUserManagerForUmbracoMembers<UmbracoApplicationMember>();

            //Single method to configure the Identity user manager for use with Umbraco
            app.ConfigureRoleManagerForUmbracoMembers<UmbracoApplicationRole>();
        }

        /// <summary>
        /// Configures middleware to be used (i.e. app.Use...)
        /// </summary>
        /// <param name="app"/>
        protected override void ConfigureMiddleware(IAppBuilder app)
        {
            //Ensure owin is configured for Umbraco back office authentication. If you have any front-end OWIN
            // cookie configuration, this must be declared after it.
            var googleClientId = WebConfigurationManager.AppSettings["GoogleOAuth2ClientID"];
            var googleSecret = WebConfigurationManager.AppSettings["GoogleOAuth2Secret"];

            var fbClientId = WebConfigurationManager.AppSettings["FacebookOAuthClientID"];
            var fbSecret = WebConfigurationManager.AppSettings["FacebookOAuthSecret"];

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = fbClientId,
                AppSecret = fbSecret,
                Scope = { "email" },
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        // Add the access token to the identity context
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        return Task.FromResult(0);
                    }
                },
                Fields = {"name","email"}
            } );
            
            var gOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = googleClientId,
                ClientSecret = googleSecret,
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        var claims = context.Identity.Claims.ToList();
                        //context.Identity.AddClaim(new System.Security.Claims.Claim("GoogleAccessToken", context.AccessToken));
                        //context.Identity.AddClaim(new System.Security.Claims.Claim("email", context.User.GetValue("picture").ToString()));
                        //context.Identity.AddClaim(new System.Security.Claims.Claim("openid", context.User.GetValue("profile").ToString()));
                        return Task.FromResult(0);
                    }
                },

            };
            gOptions.Scope.Add("email");
            gOptions.Scope.Add("openid");
            
            app.UseGoogleAuthentication(gOptions);
            
            app
                .UseUmbracoBackOfficeCookieAuthentication(ApplicationContext, PipelineStage.Authenticate);
                //Open following LOC if you want external service login in Umbraco backoffice.
                //.UseUmbracoBackOfficeExternalCookieAuthentication(ApplicationContext, PipelineStage.Authenticate);
            
            // Enable the application to use a cookie to store information for the 
            // signed in user and to use a cookie to temporarily store information 
            // about a user logging in with a third party login provider 
            // Configure the sign in cookie
            app.UseCookieAuthentication(new FrontEndCookieAuthenticationOptions
            {
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user 
                    // logs in. This is a security feature which is used when you 
                    // change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator
                        .OnValidateIdentity<UmbracoMembersUserManager<UmbracoApplicationMember>, UmbracoApplicationMember, int>(
                            TimeSpan.FromMinutes(30),
                            (manager, user) => user.GenerateUserIdentityAsync(manager),
                            UmbracoIdentity.IdentityExtensions.GetUserId<int>)
                }
            }, PipelineStage.Authenticate);

            // Uncomment the following lines to enable logging in with third party login providers

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UseMicrosoftAccountAuthentication(
            //  clientId: "",
            //  clientSecret: "");

            //app.UseTwitterAuthentication(
            //  consumerKey: "",
            //  consumerSecret: "");

            

            //Below LOCs for Umbraco Back office externl login authentication.

            //UmbracoFacebookAuthExtensions.ConfigureBackOfficeFacebookAuth(app, fbClientId, fbSecret, caption: "Facebook");

           // UmbracoGoogleAuthExtensions.ConfigureBackOfficeGoogleAuth(app, googleClientId, googleSecret);



            //Lasty we need to ensure that the preview Middleware is registered, this must come after
            // all of the authentication middleware:
            app.UseUmbracoPreviewAuthentication(ApplicationContext, PipelineStage.Authorize);
        }
        
    }
}

