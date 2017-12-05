using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RealHomes.Models;

using System.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using RealHomes.Helper;

namespace RealHomes.Controllers
{

    public class SearchWidgetController : SurfaceController
    {
        private const string FIXTURE_TABLE_NAME = "RHFixturesAndFeatures";
        private const string VIEWS_TABLE_NAME = "RHViews";
        private const string PARTIAL_VIEW_SEARCH = "SiteLayout/_SearchWidget.cshtml";




        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        [HttpGet]
        public async Task<ActionResult> GetAllValues()
        {
            SearchWidgetModel searchModel=new SearchWidgetModel();

            searchModel.Locations = (new LocationController()).GetAllLocations(); // db.Fetch<LocationModel>(new Sql().Select("*").From(LOCATION_TABLE_NAME));

            searchModel.Categories = (new CategoryController()).GetAllCategoryies(); // db.Fetch<CategoryModel>(new Sql().Select("*").From(CATEGORY_TABLE_NAME));

            searchModel.PropertyTypes = (new PropertyTypeController()).GetAllPropertyTypes(); // db.Fetch<PropertyTypeModel>(new Sql().Select("*").From(TYPE_TABLE_NAME));

            searchModel.FixturesAndFeatures = (new FixturesController()).GetAllFixtures(); // db.Fetch<FixturesAndFeaturesModel>(new Sql().Select("*").From(FIXTURE_TABLE_NAME));

            searchModel.Views = (new ViewsController()).GetAllViews(); // db.Fetch<ViewsModel>(new Sql().Select("*").From(VIEWS_TABLE_NAME));

            searchModel.Facilities = (new PreValueHelper()).GetPreValuesFromAppSettingName("FacilitiesAndAmenities");
            // return this.j

            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_SEARCH, await Task.FromResult(searchModel)); ;
        }

       
        public JsonResult GetLocations(string sPrefix)
        {
            IEnumerable<LocationModel> locations = (new LocationController()).GetAllLocations();

            sPrefix = sPrefix.Replace("\"", "");
            var locationNames = (from N in locations
                                where N.LocationName.ToLower().StartsWith(sPrefix.ToLower())
                            select new {id=N.LocationID, name=N.LocationName });

            return Json(locationNames, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method will be updated to check some values in database
        /// </summary>
        /// <returns></returns>
        private bool ValuesExists()
        {
            return GetAllValues() != null;
        }
    }
}