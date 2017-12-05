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
using Umbraco.Core;
using Umbraco.Core.Persistence;
using RealHomes.Helper;

namespace RealHomes.Controllers
{
    public class PropertySurfaceController : SurfaceController
    {
        private const string ID_COLUMN_NAME = "PropId";
        private const string TABLE_NAME = "RHProperty as M";
        private const string TABLE_NAME_DB = "RHProperty";

        private const string PARTIAL_VIEW_INDEX = "PropertySurface/_Index.cshtml";
        private const string PARTIAL_VIEW_CREATE = "PropertySurface/_CreateProperty.cshtml";
        private const string PARTIAL_VIEW_EDIT = "PropertySurface/_EditProperty.cshtml";
        private const string PARTIAL_VIEW_DELETE = "PropertySurface/_DeleteProperty.cshtml";
        private const string PARTIAL_VIEW_LIST = "PropertySurface/_Index.cshtml";
        
        //private RealHomesContext db = new RealHomesContext();
        private UmbracoDatabase db = ApplicationContext.Current.DatabaseContext.Database;

        // GET: PropertySurface
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            IEnumerable<PropertyViewModel> PropertyModels = db.Fetch<PropertyViewModel>(new Sql().Select("*").From(TABLE_NAME));

            return PartialView(StringConstants.PARTIAL_VIEW_PATH+PARTIAL_VIEW_INDEX,await Task.FromResult(PropertyModels));

        }

        // GET: PropertySurface/Details/5
        //return the View containing the details of a single property
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = new Sql().Select("*").From(TABLE_NAME).Where<PropertyViewModel>(x => x.PropertyId == id, DatabaseContext.SqlSyntax);

            PropertyViewModel propertyModel = await Task.FromResult((PropertyViewModel)db.Fetch<PropertyViewModel>(query).FirstOrDefault());
   
            if (propertyModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(propertyModel);
        }

        // GET: PropertySurface/Create
        //Create a new Property in database
        public ActionResult Create()
        {
            //create all the foriegn key lists and set them in the view bag for view usage

            IEnumerable<SelectListItem> cityNames = (from C in (new CityController()).GetAllCities()
                                                     select new SelectListItem { Text = C.CityName, Value = C.CityId.ToString() });

            IEnumerable<SelectListItem> locationNames = (from N in (new LocationController()).GetAllLocations()
                                                         select new SelectListItem { Text = N.LocationName, Value = N.LocationID.ToString() });

            IEnumerable<SelectListItem> categoryNames = (from C in (new CategoryController()).GetAllCategoryies()
                                                         select new SelectListItem { Text = C.CategoryName, Value = C.CategoryId.ToString() });

            IEnumerable<SelectListItem> serviceNames = (from S in (new ServiceController()).GetAllServices()
                                                        select new SelectListItem { Text = S.ServiceName, Value = S.ServiceId.ToString() });

            IEnumerable<SelectListItem> typeNames = (from T in (new PropertyTypeController()).GetAllPropertyTypes()
                                                     select new SelectListItem { Text = T.TypeName +" - "+T.TypeCode, Value = T.TypeId.ToString() });
            //typeNames.FirstOrDefault().Selected = true;

            List<SelectListItem> statusNames = new List<SelectListItem>();
            statusNames.Add(new SelectListItem { Text = "Ready Now", Value = "1" });
            statusNames.Add(new SelectListItem { Text = "In Process", Value = "2" });

            List<SelectListItem> devHolds = new List<SelectListItem>();
            devHolds.Add(new SelectListItem { Text = "Free Hold", Value = "1" });
            devHolds.Add(new SelectListItem { Text = "Ready Now", Value = "1" });

            ViewBag.Cities = cityNames;
            ViewBag.Locations = locationNames;
            ViewBag.Categories = categoryNames;
            ViewBag.Services = serviceNames;
            ViewBag.Types = typeNames;
            ViewBag.Status = statusNames;
            ViewBag.DevHold = devHolds;
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_CREATE);
        }

        // POST: PropertySurface/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// <summary>
        /// Save the record int the database
        /// </summary>
        /// <param name="propertyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProperty( PropertyViewModel propertyModel)
        {
             

            if (!ModelState.IsValid) {   return CurrentUmbracoPage(); }

            //fill the reference number and other data to be calculated
            try
            {
                PropertyDBModel propertyModeltoSave = new PropertyDBModel();
                
                //Get Max ID from the Database
                var str = new Sql().Select("MAX(PropID)+1").From(TABLE_NAME);
                long nextID = db.Fetch<long>(str).FirstOrDefault();
                string fullRefNo = propertyModel.ReferenceNo + nextID.ToString();
                string imagePath = StringConstants.IMAGE_PATH + fullRefNo + "/"+ StringConstants.THUMBNAIL_FOLDER_NAME;
               

                //Load Data from view model into business model
                //propertyModeltoSave.PropId = nextID;
                propertyModeltoSave.PropertyTitle = propertyModel.PropertyTitle;
                propertyModeltoSave.CategoryId = propertyModel.CategoryId;
                propertyModeltoSave.LocationId = propertyModel.LocationId;
                propertyModeltoSave.TypeId = propertyModel.TypeId;
                propertyModeltoSave.ReferenceNo = fullRefNo;
                propertyModeltoSave.MainFeature = propertyModel.MainFeature;
                propertyModeltoSave.Address = propertyModel.Address;
                propertyModeltoSave.TotalSize = propertyModel.TotalSize;
                propertyModeltoSave.BuildUpArea = propertyModel.BuildUpArea;
                propertyModeltoSave.ReraPermitNo = propertyModel.ReraPermitNo;
                propertyModeltoSave.SellPrice = propertyModel.SellPrice;
                propertyModeltoSave.carparking = propertyModel.carparking;
                propertyModeltoSave.bedrooms = propertyModel.bedrooms;
                propertyModeltoSave.baths = propertyModel.baths;
                propertyModeltoSave.AvailabilityStatus = propertyModel.AvailabilityStatus;
                propertyModeltoSave.DevelopmentStatus = propertyModel.DevelopmentStatus;
                propertyModeltoSave.ServiceId = propertyModel.ServiceId;

                propertyModeltoSave.PropertyDetailPageID = fullRefNo;
                propertyModeltoSave.PropertyImagePath = imagePath;
                propertyModeltoSave.PropertyOwnerId = 1;
                propertyModeltoSave.UserId = 1;
                propertyModeltoSave.CreatedOn = System.DateTime.Now;


                db = ApplicationContext.Current.DatabaseContext.Database;

                
                db.Insert(TABLE_NAME_DB, ID_COLUMN_NAME, propertyModeltoSave);

                TempData["PropertySavedSuccess"] = true;
                TempData["ReferenceNo"] = fullRefNo;

                //create a detail node in umbrco  content area
                UmbracoUtils utils = new UmbracoUtils();
                utils.CreateDetailContentNode(fullRefNo, null);

                


                return RedirectToCurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                ErrorController ec = new ErrorController();
                ec.LogErrorinDb(ex);
                return CurrentUmbracoPage();
                //log message

            }
            //return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_CREATE, await Task.FromResult(propertyModel));
        }

        // GET: PropertySurface/Edit/5
        /// <summary>
        /// Retrieve the Property record to edit 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = new Sql().Select("*").From(TABLE_NAME).Where<PropertyViewModel>(x => x.PropertyId == id, DatabaseContext.SqlSyntax);

            PropertyViewModel propertyModel = await Task.FromResult((PropertyViewModel)db.Fetch<PropertyViewModel>(query).FirstOrDefault());

            if (propertyModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_EDIT, await Task.FromResult(propertyModel));
        }

        // POST: PropertySurface/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        /// <summary>
        /// Update the edited property record int he database
        /// </summary>
        /// <param name="propertyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PropertyViewModel propertyModel)
        {
            //Edit([Bind(Include = "PropertyId,PropertyTitle,ReferenceNo,PropertyCategory,PropertyType,MainFeature,Location,TotalSize,BuildUpArea,ReraPermitNo,SellPrice,carparking,bedrooms,baths,AvailabilityStatus,DevelopmentStatus,ServiceType,PropertyDetailPageURL,PropertyImagePath,PropertyOwnerId")] PropertyModel propertyModel)
            if (ModelState.IsValid)
            {
                await Task.FromResult(db.Update(propertyModel));

                return RedirectToCurrentUmbracoPage();
            }
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_EDIT, await Task.FromResult(propertyModel));
        }

        // GET: PropertySurface/Delete/5
        /// <summary>
        /// Retrieve a record from the database to delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = new Sql().Select("*").From(TABLE_NAME).Where<PropertyViewModel>(x => x.PropertyId == id,DatabaseContext.SqlSyntax);

            PropertyViewModel propertyModel = await Task.FromResult((PropertyViewModel)db.Fetch<PropertyViewModel>(query).FirstOrDefault());

            if (propertyModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_DELETE, await Task.FromResult(propertyModel));
        }

        // POST: PropertySurface/Delete/5
        /// <summary>
        /// Archive the record after confirmation 
        /// logging also required after all actions 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            var query = new Sql().Select("*").From(TABLE_NAME).Where<PropertyViewModel>(x => x.PropertyId == id, DatabaseContext.SqlSyntax);

            PropertyViewModel propertyModel = await Task.FromResult((PropertyViewModel)db.Fetch<PropertyViewModel>(query).FirstOrDefault());
            
            await Task.FromResult(db.Delete(propertyModel));
            TempData["RecordDeleted"] = true;


            return RedirectToCurrentUmbracoPage();

            //return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> GetProperties(int iPageno, string sSortName, string sCriteria)
        {
            int iPageSize = Convert.ToInt32(StringConstants.PAGE_SIZE);
            PagedPropertyViews pagedView = new PagedPropertyViews();
            //IEnumerable<PropertyModelView> PropertyModels;
            try
            {
                if (Request.IsAjaxRequest())
                {
                    if (sCriteria == null || sCriteria == "")
                    {
                        var query = new Sql().Select("*").From(TABLE_NAME);
                        pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count;

                        pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();
                    }
                    else
                    {
                        //sCriteria = "serviceType='1'";
                        var query = new Sql().Select("*").From(TABLE_NAME).Where(sCriteria);
                        pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count;

                        pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();

                    }
                }
                else
                {
                    var query = new Sql().Select("*").From(TABLE_NAME);
                    pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count;

                    pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();
                }

               
                pagedView.CurrentPage = iPageno;

                FillPropertyViewModel(ref pagedView.PropertyModels);

                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_LIST, await Task.FromResult(pagedView));
            }
            catch(Exception x)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPropertiesBL(int iPageno,string sSortName,string sLocationKey,int iServiceId, int iLocationID,int iCategoryId, int iTypeId, int idevhold, int iminbed, int imaxbed, int iminPrice, int[] fixtures, int imaxPrice, int[] views)
        {
            string sCriteria = ""; int iPageSize = Convert.ToInt32(StringConstants.PAGE_SIZE);
            PagedPropertyViews pagedView = new PagedPropertyViews();
            //IEnumerable<PropertyModelView> PropertyModels;
            try
            {
               
                if (Request.IsAjaxRequest())
                { 
                    
                    sCriteria = GetCriteria(iServiceId, iLocationID, iCategoryId, iTypeId, idevhold, iminbed, imaxbed, iminPrice, imaxPrice, fixtures, views);

                    if (sCriteria == null || sCriteria == "")
                    {
                        var query = new Sql().Select("*").From(TABLE_NAME);
                        pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count();

                        //IEnumerable<PropertyModelView> PropertyModels = db.Fetch<PropertyModelView>(new Sql().Select("*").From(TABLE_NAME)).OrderBy(m => m.PropertyId).Skip(pagesize * (pagenumber - 1)).Take(pagesize).ToList();
                        pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();

                    }
                    else
                    {
                        var query = new Sql().Select("*").From(TABLE_NAME).Where(sCriteria);
                        pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count();

                        //IEnumerable<PropertyModelView> PropertyModels = db.Fetch<PropertyModelView>(new Sql().Select("*").From(TABLE_NAME).Where(sCriteria)).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();
                        pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();
                        
                    }
                }
                else
                {
                    var query = new Sql().Select("*").From(TABLE_NAME);
                    pagedView.TotalRecords = db.Fetch<PropertyViewModel>(query).Count();

                    pagedView.PropertyModels = db.Fetch<PropertyViewModel>(query).OrderBy(m => m.PropertyId).Skip(iPageSize * (iPageno - 1)).Take(iPageSize).ToList();
                    
                }
                pagedView.TotalPages = Convert.ToInt32(Math.Ceiling((double)pagedView.TotalRecords / iPageSize));
                pagedView.CurrentPage = iPageno;

                SetPagingValues(ref pagedView);
                FillPropertyViewModel(ref pagedView.PropertyModels);
                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_LIST, await Task.FromResult(pagedView));

            }
            catch (Exception x)
            {
                //log the error
                return null;
            }
        }
        //
        private string GetCriteria(int iServiceId, int iLocationID, int iCategoryId, int iTypeId, int idevhold, int iminbed, int imaxbed, int iminprice, int imaxprice, int[] fixtures, int[] views)
        {
            string sCriteria = "";

            if (iServiceId > 0)
                sCriteria = "ServiceId = " + iServiceId;
            else
            {
                //service Id 0 means first time search page loading.
                sCriteria = ""; return sCriteria;
            }

            if (iLocationID > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND LocationId=" + iLocationID;
                else
                    sCriteria = sCriteria + "LocationId=" + iLocationID;
            }

            if (iCategoryId > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND CategoryId=" + iCategoryId;
                else
                    sCriteria = sCriteria + " CategoryId=" + iCategoryId;
            }

            if (iTypeId > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND TypeId=" + iTypeId;
                else
                    sCriteria = sCriteria + " TypeId=" + iTypeId;
            }
            
            if (idevhold > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND DevelopmentStatus=" + idevhold;
                else
                    sCriteria = sCriteria + " DevelopmentStatus=" + idevhold;
            }

            if (iminbed > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND bedrooms>=" + iminbed;
                else
                    sCriteria = sCriteria + " bedrooms>=" + iminbed;
            }
            if (imaxbed > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND bedrooms<=" + imaxbed;
                else
                    sCriteria = sCriteria + " bedrooms<=" + imaxbed;
            }
            if (iminprice > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND sellPrice>=" + iminprice;
                else
                    sCriteria = sCriteria + " sellPrice>=" + iminprice;
            }
            if (imaxprice > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND sellPrice<=" + imaxprice;
                else
                    sCriteria = sCriteria + " sellPrice<=" + imaxprice;
            }

            if (fixtures != null && fixtures.Length>0 )
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND Exists (select * from RHUnitFixtures F where F.PropId = M.PropId AND F.FixId IN("+ string.Join(",", fixtures)  + "))";
                else
                    sCriteria = sCriteria + " Exists (select * from RHUnitFixtures F where F.PropId = M.PropId AND F.FixId IN(" + string.Join(",", fixtures) + "))";
            }
            if (views != null && views.Length > 0)
            {
                if (!String.IsNullOrEmpty(sCriteria) && sCriteria.Length > 0)
                    sCriteria = sCriteria + " AND Exists (select * from RHUnitViews V where V.PropId = M.PropId AND V.ViewId IN(" + string.Join(",", views) + "))";
                else
                    sCriteria = sCriteria + " Exists (select * from RHUnitViews V where V.PropId = M.PropId AND V.ViewId IN(" + string.Join(",", views) + "))";
            }
            /* apply fixtures and views filter
             * select * from rhproperty M
                where Exists (select * from RHUnitFixtures F where F.PropId = M.PropId AND F.FixId IN(5))
                AND Exists (select * from RHUnitViews V where V.PropId = M.PropId AND V.ViewId IN(2))
             * */

            return sCriteria;
        }

        private void SetPagingValues(ref PagedPropertyViews pager)
        {
            pager.StartPage = pager.CurrentPage - 5;
            pager.EndPage = pager.CurrentPage + 4;
            if (pager.StartPage <= 0)
            {
                pager.EndPage -= (pager.StartPage - 1);
                pager.StartPage = 1;
            }
            if (pager.EndPage > pager.TotalPages)
            {
                pager.EndPage = pager.TotalPages;
                if (pager.EndPage > 10)
                {
                    pager.StartPage = pager.EndPage - 9;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillPropertyViewModel(ref IEnumerable<PropertyViewModel> propertyModels)
        {
            foreach(PropertyViewModel pm in propertyModels)
            {

                //Get Location 
                if (pm.LocationId > 0)
                    pm.PropertyLocation = (new LocationController()).GetLocation(pm.LocationId);


                //Get Category Name
                if (pm.CategoryId > 0)
                    pm.CategoryName = (new CategoryController()).GetCategory(pm.CategoryId).CategoryName;


                //Get Service Name
                if (pm.ServiceId > 0)
                   pm.ServiceName = (new ServiceController()).GetService(pm.ServiceId).ServiceName;


                //Get Property Type Name
                if (pm.TypeId > 0)
                    pm.TypeName = (new PropertyTypeController()).GetPropertyType(pm.TypeId).TypeName;
                
            }

        }
    }
}
