using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RealHomes.Models;

using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Web.Mvc;

namespace RealHomes.Controllers
{
   
    public class LocationController : SurfaceController
    {
        private const string TABLE_NAME = "RHLocation";
                

        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

       
        public IEnumerable<LocationModel> GetAllLocations()
        {
            IEnumerable<LocationModel> locationModels = db.Fetch<LocationModel>(new Sql().Select("*").From(TABLE_NAME));

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
            return false;
        }

        /// <summary>
        /// Get Loction Model of   particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LocationModel GetLocation(long id)
        {
            try
            {
                LocationModel locationmodel = db.Fetch<LocationModel>(new Sql().Select("*").From(TABLE_NAME).Where<LocationModel>(x => x.LocationID ==  id, DatabaseContext.SqlSyntax)).FirstOrDefault();
                //LocationModel locationmodel = db.Fetch<LocationModel>(new Sql("*").Select().From(TABLE_NAME).Where("LocationID=" + id)).FirstOrDefault();

                return locationmodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}