using System;


using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;


using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace RealHomes.App_Code
{
    public class Global: ApplicationEventHandler
    {


        /*protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            base.OnApplicationStarted(sender, e);
            //AreaRegistration.RegisterAllAreas();
            //RegisterWebApiRoutes(GlobalConfiguration.Configuration);

            //RegisterRoutes(RouteTable.Routes);
           
        }*/

        public new void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.OnApplicationStarted(umbracoApplication, applicationContext);
        }

        public new void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.OnApplicationStarted(umbracoApplication, applicationContext);
        }

        public new void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.OnApplicationStarted(umbracoApplication, applicationContext);
            RouteTable.Routes.MapRoute(name: "Locations",
                                      url: umbraco.GlobalSettings.UmbracoMvcArea + "/backoffice/LocationApi/{action}/{id}",
                                      defaults: new
                                      {
                                          controller = "LocationApi",
                                          action = "GetAllLocations",
                                          id = UrlParameter.Optional
                                      });
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
                         

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SearchWidget",
                url: "SearchWidgetApi/GetLocationModels",
                defaults: new { controller = "SearchWidgetApi", action = "GetLocationModels" }
            );

        }

        public static void RegisterWebApiRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "SearchWidgetApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}