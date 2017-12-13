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
using Examine;
using Examine.SearchCriteria;
using Examine.Providers;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Globalization;
using System.Web.Configuration;

namespace RealHomes.Controllers
{
    public class PropertyCMSController : SurfaceController
    {

        private const string PARTIAL_VIEW_LIST = "PropertyCMS/_PropertyList.cshtml";
        private const string PARTIAL_VIEW_DETAIL = "PropertyCMS/_Details.cshtml";
        private const string ERROR_SETTING_NAME = "ErrorPageId";
        /// <summary>
        /// Implementing CMS-Properties based search and mapping the result onto properties models
        /// </summary>
        /// <param name="iPageno"></param>
        /// <param name="sSortName"></param>
        /// <param name="sLocationKey"></param>
        /// <param name="iServiceId"></param>
        /// <param name="sLocationName"></param>
        /// <param name="sCategory"></param>
        /// <param name="sType"></param>
        /// <param name="sDevhold"></param>
        /// <param name="iMinbed"></param>
        /// <param name="iMaxbed"></param>
        /// <param name="iMinPrice"></param>
        /// <param name="facilities"></param>
        /// <param name="iMaxPrice"></param>
        /// <param name="views"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> GetPropertiesBL(int iPageno, string sSortName, string sLocationKey, int iServiceId, string sLocationName, string sCategory, string sType, string sDevhold, int iMinbed, int  iMaxbed, int iMinPrice, string[] facilities, int iMaxPrice, string[] views)
        {
            int iPageSize = Convert.ToInt32(StringConstants.PAGE_SIZE);
            PagedPropertyViews pagedView = new PagedPropertyViews();
            
            try
            {

                if (Request.IsAjaxRequest())
                {
                    //AdvSearchModel srchModel = new AdvSearchModel();
                    var searcher = Examine.ExamineManager.Instance.SearchProviderCollection[StringConstants.EXMINE_SEARCHER_NAME];
                    IEnumerable<SearchResult> pagedResults;

                    IBooleanOperation query = GetCriteria(iServiceId, sLocationName, sCategory, sType, sDevhold, iMinbed, iMaxbed, iMinPrice, facilities, iMaxPrice, views, ref searcher);


                    //Get the search result
                    var results = searcher.Search(query.Compile());
                    if (results.Count() > 0)
                        pagedView.TotalRecords = results.Count();
                    else
                        pagedView.TotalRecords = 0;

                    
                    
                    pagedResults = results.Skip((iPageno - 1) * iPageSize).Take(iPageSize);
                   
                    
                    if (pagedResults.Count() > 0)
                    {
                        //Prepare Property Models to be displayed on the result page
                        List<PropertyCMSViewModel> properties = new List<PropertyCMSViewModel>();
                        foreach (var result in pagedResults)
                        {
                            PropertyCMSViewModel item = new PropertyCMSViewModel();
                            IPublishedContent node = Umbraco.Content(result.Fields["id"]);

                            item = MapNodeToModel(node);
                           
                            properties.Add(item);

                        }
                        pagedView.CMSProperties = properties;


                    }
                }
                
                    pagedView.TotalPages = Convert.ToInt32(Math.Ceiling((double)pagedView.TotalRecords / iPageSize));
                    pagedView.CurrentPage = iPageno;

                    SetPagingValues(ref pagedView);

                    PagedPropertyViews pagedresults = new PagedPropertyViews();

                    //PagedPropertyCMSView pgrslt = new PagedPropertyCMSView();
                    return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_LIST, await System.Threading.Tasks.Task.FromResult(pagedView));
                  

            }
            catch (Exception x)
            {
                //log the error
                return null;
            }
        }
        //
        private IBooleanOperation GetCriteria(int iServiceId, string sLocationName, string sCategory, string sType, string sDevhold, int iMinbed, int iMaxbed, int iMinPrice, string[] facilities, int iMaxPrice, string[] views, ref BaseSearchProvider searcher)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();
            PropertyCMSViewModel srchModel = new PropertyCMSViewModel();
            string fct; string uv;

            fct = facilities == null ? "" : string.Join(",", facilities);
            uv = views == null ? "" : string.Join(",", views);


            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.And);

            IBooleanOperation query = null;
            try
            {
                //Built the exmine query
                if (iServiceId == 1)
                {
                    query = searchCriteria.Field(srchModel.SERVICE_PROPERTY_NAME, "Sale");

                }
                else if (iServiceId == 2)
                    //service Id 0 means first time search page loading.
                    query = searchCriteria.Field(srchModel.SERVICE_PROPERTY_NAME, "Rent");
                else if (iServiceId == 3)
                    //service Id 0 means first time search page loading.
                    query = searchCriteria.Field(srchModel.SERVICE_PROPERTY_NAME, "Short Stay");

                if (sCategory != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.CATEGRY_PROPERTY_NAME, sCategory);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.CATEGRY_PROPERTY_NAME, sCategory);
                    }

                }

                if (sLocationName != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.LOCATION_PROPERTY_NAME, sLocationName);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.LOCATION_PROPERTY_NAME, sLocationName);
                    }
                }
                //sType

                if (sType != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.TYPE_PROPERTY_NAME, sType);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.TYPE_PROPERTY_NAME, sType);
                    }
                }
                //, sDevhold
                if (sDevhold != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.DEVHOLD_PROPERTY_NAME, sDevhold);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.DEVHOLD_PROPERTY_NAME, sDevhold);
                    }
                }
                //, iMinbed, iMaxbed, 
                if ((iMinbed > 0 && iMaxbed > 0) || (iMinbed <= 0 && iMaxbed > 0))
                {
                    if (query == null)
                    {

                        query = searchCriteria.Range(srchModel.BED_PROPERTY_NAME, iMinbed, iMaxbed, true, true);

                    }
                    else
                    {
                        query = query.And().Range(srchModel.BED_PROPERTY_NAME, iMinbed, iMaxbed, true, true);
                    }
                }
                else if (iMinbed > 0 && iMaxbed <= 0)
                {
                    if (query == null)
                    {

                        query = searchCriteria.Range(srchModel.BED_PROPERTY_NAME, iMinbed, 1000, true, true);

                    }
                    else
                    {
                        query = query.And().Range(srchModel.BED_PROPERTY_NAME, iMinbed, 1000, true, true);
                    }
                }
                //iMinPrice, iMaxPrice, 
                if ((iMinPrice > 0 && iMaxPrice > 0) || (iMinPrice <= 0 && iMaxPrice > 0))
                {
                    if (query == null)
                    {

                        query = searchCriteria.Range(srchModel.PRICE_PROPERTY_NAME, iMinPrice, iMaxPrice, true, true);

                    }
                    else
                    {
                        query = query.And().Range(srchModel.PRICE_PROPERTY_NAME, iMinPrice, iMaxPrice, true, true);
                    }
                }
                else if (iMinPrice > 0 && iMaxPrice <= 0)
                {
                    if (query == null)
                    {

                        query = searchCriteria.Range(srchModel.PRICE_PROPERTY_NAME, iMinPrice, Convert.ToInt64(StringConstants.MAXSELLPRICE), true, true);

                    }
                    else
                    {
                        query = query.And().Range(srchModel.PRICE_PROPERTY_NAME, iMinPrice, Convert.ToInt64(StringConstants.MAXSELLPRICE), true, true);
                    }
                }
                //fixtures
                if (fct.Length > 0)
                {
                    /*int count = 0;
                    foreach (string value in fct.Split(','))
                    {
                        if (query == null)
                        {
                            query = searchCriteria.Field(srchModel.FACILITIES_PROPERTY_NAME, value);
                        }
                        else
                        {
                            if (count == 0)
                            {
                                query = query.And().Field(srchModel.FACILITIES_PROPERTY_NAME, value);
                                count++;
                            }
                            else
                            {
                                query = query.Or().Field(srchModel.FACILITIES_PROPERTY_NAME, value);
                                count++;
                            }
                        }

                    }*/

                    //Grouped OR for fcilities search 
                    
                    if (query == null)
                    {
                        query = searchCriteria.GroupedOr(new[] { srchModel.FACILITIES_PROPERTY_NAME }, fct.Split(','));
                    }
                    else
                    {
                        query = query.And().GroupedOr(new[] { srchModel.FACILITIES_PROPERTY_NAME }, fct.Split(','));
                    }
                }
                //views
                if (uv.Length > 0)
                {
                    /*int count = 0;
                    foreach (string value in uv.Split(','))
                    {
                        if (query == null)
                        {
                            query = searchCriteria.Field(srchModel.VIEWS_PROPERTY_NAME, value);
                        }
                        else
                        {
                            if (count == 0)
                            {
                                query = query.And().Field(srchModel.VIEWS_PROPERTY_NAME, value);
                                count++;
                            }
                            else
                            {
                                query = query.Or().Field(srchModel.VIEWS_PROPERTY_NAME, value);
                                count++;
                            }
                        }

                    }*/

                    //using Grouped OR for unit views searching 
                    if (query == null)
                    {
                        query = searchCriteria.GroupedOr(new[] { srchModel.VIEWS_PROPERTY_NAME }, uv.Split(','));
                    }
                    else
                    {
                        query = query.And().GroupedOr(new[] { srchModel.VIEWS_PROPERTY_NAME }, uv.Split(','));
                    }
                }

                return query;
            }
            catch(Exception ex)
            {
                return null;
            }
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

        

        [HttpGet]
        public ActionResult Showdetail(string PropId)
        {
            try
            {
                IPublishedContent node = Umbraco.Content(PropId);
            if (node == null )
            {
                string errorPageId = WebConfigurationManager.AppSettings[ERROR_SETTING_NAME] as string;
                RedirectToUmbracoPage(Convert.ToInt32(errorPageId));
            }
           
                PropertyCMSViewModel model = MapNodeToModel(node);

                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_DETAIL, model);
            }
            catch(Exception ex)
            {
                //log the exception properly
                RedirectToCurrentUmbracoPage();
                return null;
            }
        }


        public PropertyCMSViewModel MapNodeToModel(IPublishedContent node)
        {
            PropertyCMSViewModel item = new PropertyCMSViewModel();

            //item.Ownername = node.GetPropertyValue("owner").ToString();
            item.PropertyId = Convert.ToInt64(node.Id);
            item.PropertyTitle = node.GetPropertyValue(item.TITLE_PROPERTY_NAME)== null ? "" : node.GetPropertyValue(item.TITLE_PROPERTY_NAME).ToString();
            item.CategoryName = node.GetPropertyValue(item.CATEGRY_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.CATEGRY_PROPERTY_NAME).ToString();
            item.TypeName = node.GetPropertyValue(item.TYPE_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.TYPE_PROPERTY_NAME).ToString();
            //City and region Mapping
            item.LocationName = node.GetPropertyValue(item.LOCATION_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.LOCATION_PROPERTY_NAME).ToString();
            item.ServiceName = node.GetPropertyValue(item.SERVICE_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.SERVICE_PROPERTY_NAME).ToString();
            item.Address = node.GetPropertyValue(item.ADDRESS_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.ADDRESS_PROPERTY_NAME).ToString();

            item.ReferenceNo = node.GetPropertyValue(item.REFNO_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.REFNO_PROPERTY_NAME).ToString();
            item.MainFeature = node.GetPropertyValue(item.MAINFEATURE_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.MAINFEATURE_PROPERTY_NAME).ToString();
            item.TotalSize = Convert.ToInt64(node.GetPropertyValue(item.TOTALAREA_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.TOTALAREA_PROPERTY_NAME));
            item.BuildUpArea = Convert.ToInt64(node.GetPropertyValue(item.BAREA_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.BAREA_PROPERTY_NAME));
            item.ReraPermitNo = Convert.ToInt64(node.GetPropertyValue(item.RERANO_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.RERANO_PROPERTY_NAME));

            item.SellPrice = Convert.ToInt64(node.GetPropertyValue(item.SELLPRICE_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.SELLPRICE_PROPERTY_NAME));

            item.carparking = Convert.ToInt16(node.GetPropertyValue(item.PARKING_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.PARKING_PROPERTY_NAME));
            item.baths = Convert.ToInt16(node.GetPropertyValue(item.BATHROOM_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.BATHROOM_PROPERTY_NAME));
            item.bedrooms = Convert.ToInt16(node.GetPropertyValue(item.BED_PROPERTY_NAME).ToString() == "" ? 0 : node.GetPropertyValue(item.BED_PROPERTY_NAME));
                        
            
            item.CreatedOn = node.CreateDate;
            item.StatusName = node.GetPropertyValue(item.STATUS_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.STATUS_PROPERTY_NAME).ToString();
            item.DevHoldName = node.GetPropertyValue(item.DEVHOLD_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.DEVHOLD_PROPERTY_NAME).ToString();

            if (node.GetPropertyValue(item.IMAGES_PROPERTY_NAME) != null)
            {
                item.Images = node.GetPropertyValue<IEnumerable<IPublishedContent>>(item.IMAGES_PROPERTY_NAME);
            }

            if (node.GetPropertyValue(item.IMAGEGALLERY_PROPERTY_NAME) != null)
            {
                item.ImageGallery = node.GetPropertyValue<IEnumerable<IPublishedContent>>(item.IMAGEGALLERY_PROPERTY_NAME);
            }
            
            //item.RegionName = node.GetPropertyValue("regionName").ToString();
            
            item.Overview = node.GetPropertyValue(item.UNITOVERVIEW_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.UNITOVERVIEW_PROPERTY_NAME).ToString();
            item.NearestMetros = node.GetPropertyValue(item.NEARESTMETRO_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.NEARESTMETRO_PROPERTY_NAME).ToString();
            item.NearestSchools = node.GetPropertyValue(item.NEARESTSCHOOLS_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.NEARESTSCHOOLS_PROPERTY_NAME).ToString();

            if (node.GetPropertyValue<IEnumerable<string>>(item.FACILITIES_PROPERTY_NAME) != null)
            {
                item.Facilities = node.GetPropertyValue<IEnumerable<string>>(item.FACILITIES_PROPERTY_NAME);
            }
            if (node.GetPropertyValue<IEnumerable<string>>(item.FITTING_PROPERTY_NAME) != null)
            {
                item.Fixtures = node.GetPropertyValue<IEnumerable<string>>(item.FITTING_PROPERTY_NAME);
            }
            if (node.GetPropertyValue<IEnumerable<string>>(item.VIEWS_PROPERTY_NAME) != null)
            {
                item.Views = node.GetPropertyValue<IEnumerable<string>>(item.VIEWS_PROPERTY_NAME);
            }

            item.OwnerId = node.GetPropertyValue(item.OWNERID_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.OWNERID_PROPERTY_NAME).ToString();
            item.UserName = node.CreatorName;
            item.PropertyDetailPageID = node.GetPropertyValue(item.DETAILPAGEID_PROPERTY_NAME)==null?"": node.GetPropertyValue(item.DETAILPAGEID_PROPERTY_NAME).ToString();
            

            return item;
        }
       
    }
}