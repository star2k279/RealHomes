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

    public class CityController : SurfaceController
    {
        private const string TABLE_NAME = "RHCity";


        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;


        public IEnumerable<CityModel> GetAllCities()
        {
            IEnumerable<CityModel> cityModels = db.Fetch<CityModel>(new Sql().Select("*").From(TABLE_NAME));

            return cityModels;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(long id)
        {
            return false;
        }

        /// <summary>
        /// Get Loction Model of   particular Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CityModel GetCity(long id)
        {
            try
            {
                CityModel citymodel = db.Fetch<CityModel>(new Sql().Select("*").From(TABLE_NAME).Where<CityModel>(x => x.CityId == id, DatabaseContext.SqlSyntax)).FirstOrDefault();
                //LocationModel locationmodel = db.Fetch<LocationModel>(new Sql("*").Select().From(TABLE_NAME).Where("LocationID=" + id)).FirstOrDefault();

                return citymodel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}