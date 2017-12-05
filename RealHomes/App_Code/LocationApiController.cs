using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealHomes.Models;

using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Web.Editors;
using Umbraco.Web.WebApi;

namespace RealHomes.Controller
{

    [Umbraco.Web.Mvc.PluginController("MyArea")]
    public class LocationApiController : UmbracoAuthorizedJsonController
    {
        private const string LOCATION_TABLE_NAME = "RHLocation";


        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: api/LocationApi
        public IEnumerable<LocationModel> GetAllLocations()
        {
            IEnumerable<LocationModel> locationModels = db.Fetch<LocationModel>(new Sql().Select("*").From(LOCATION_TABLE_NAME));

            return locationModels;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationModelExists(long id)
        {
            return GetAllLocations().Count() > 0;
        }
    }
}
