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
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using RealHomes.Helper;

namespace RealHomes.Controllers
{

    public class AdvSearchController : SurfaceController
    {
       
        private const string PARTIAL_VIEW_SEARCH = "AdvancedUmbracoSearch/_AdvSearchControl.cshtml";
        private RealHomesCMSEntitiesModel cmsModel = new RealHomesCMSEntitiesModel();




        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        [HttpGet]
        public async Task<ActionResult> GetAllValues()
        {
            try
            {
                AdvSearchModel searchModel = new AdvSearchModel();
                

                searchModel.Locations = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.LOCATION_SETTING_NAME);

                searchModel.Services = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.SERVICE_SETTING_NAME);

                searchModel.Categories = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.CATEGORY_SETTING_NAME);

                searchModel.PropertyTypes = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.TYPE_SETTING_NAME);

                searchModel.Fittings = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.FITTING_SETTING_NAME);

                searchModel.UnitViews = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.UNITVIEW_SETTING_NAME);

                searchModel.Facilities = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.FACILITIES_SETTING_NAME);

                searchModel.minBeds = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.MINBED_SETTING_NAME);

                searchModel.maxBeds = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.MAXBED_SETTING_NAME);

                searchModel.minPrice = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.MINPRICE_SETTING_NAME);

                searchModel.maxPrice = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.MAXPRICE_SETTING_NAME);

                searchModel.developmentHold = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.DEVHOLD_SETTING_NAME);


                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_SEARCH, await Task.FromResult(searchModel));
            }
            catch(Exception ex)
            {
                return null;
            }
        }



        [HttpPost]
        public ActionResult SearchProperties(AdvSearchModel searchModel)
        {
            //create search criteria


            return null;

        }
        public JsonResult GetLocations(string sPrefix)
        {
            //IEnumerable<LocationModel> locations = (new LocationController()).GetAllLocations();
            List<SelectListItem> locations = (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsModel.LOCATION_SETTING_NAME);

            sPrefix = sPrefix.Replace("\"", "");
            var locationNames = (from N in locations
                                 where N.Text.ToLower().StartsWith(sPrefix.ToLower())
                                 select new { id = N.Value, name = N.Text });

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
 