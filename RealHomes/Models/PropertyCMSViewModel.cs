using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using Umbraco.Core.Persistence;
using Umbraco.Core.Models;

namespace RealHomes.Models
{
   
    public class PropertyCMSViewModel
    {
        public long PropertyId { get; set; }
        
        public string PropertyTitle { get; set; }

        public string ReferenceNo { get; set; }

        public string Address { get; set; }

        public string LocationName { get; set; }

        public string MainFeature { get; set; }

        public long TotalSize { get; set; }

        public long BuildUpArea { get; set; }

        public long ReraPermitNo { get; set; }

        public long SellPrice { get; set; }

        public short carparking { get; set; }
        
        public short bedrooms { get; set; }
        
        public short baths { get; set; }
        
        public string PropertyDetailPageID { get; set; }
        
        public long UserId { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public string RegionName { get; set; }
        
        public string CategoryName { get; set; }
        
        public string TypeName { get; set; }
        
        public string ServiceName { get; set; }

        public string OwnerId { get; set; }

        public string Ownername { get; set; }
        
        public string UserName { get; set; }
        
        public LocationModel PropertyLocation { get; set; }

        public string DevHoldName { get; set; }

        public string StatusName { get; set; }

        public IEnumerable<IPublishedContent> Images { get; set; }

        public IEnumerable<IPublishedContent> ImageGallery { get; set; }

        public string Overview { get; set; }

        public string NearestMetros { get; set; }

        public string NearestSchools { get; set; }

        public IEnumerable<string> Facilities { get; set; }

        public IEnumerable<string> Fixtures { get; set; }

        public IEnumerable<string> Views { get; set; }
        
        



        //Mapping of NODE PROPERTIES NAMES OF CMS Add Property Page

        public string CITY_PROPERTY_NAME { get { return "city"; } }

        public string LOCATION_PROPERTY_NAME { get { return "location"; } }
        public string ADDRESS_PROPERTY_NAME { get { return "address"; } }

        public string SERVICE_PROPERTY_NAME
        {
            get
            { return "services"; }
        }

        public string CATEGRY_PROPERTY_NAME
        {
            get
            {
                return "category";
            }
        }

        public string TYPE_PROPERTY_NAME
        {
            get
            {
                return "ddlPropertyType";
            }
        }
        public string TITLE_PROPERTY_NAME { get { return "propertyTitle"; } }

        public string REFNO_PROPERTY_NAME { get { return "referenceNo"; } }

        public string RERANO_PROPERTY_NAME { get { return "rERAPermitNo"; } }

        public string BATHROOM_PROPERTY_NAME { get { return "bathrooms"; } }

        public string TOTALAREA_PROPERTY_NAME { get { return "totalArea"; } }

        public string BAREA_PROPERTY_NAME { get { return "builtUpArea"; } }

        public string PARKING_PROPERTY_NAME { get { return "carParking"; } }

        public string MAINFEATURE_PROPERTY_NAME { get { return "mainFeature"; } }

        public string SELLPRICE_PROPERTY_NAME { get { return "sellPrice"; } }

        public string STATUS_PROPERTY_NAME { get { return "availabilityStatus"; } }

        public string BED_PROPERTY_NAME
        {
            get
            {
                return "bedrooms";
            }
        }

        public string PRICE_PROPERTY_NAME
        {
            get
            {
                return "sellPrice";
            }
        }

        public string DEVHOLD_PROPERTY_NAME
        {
            get
            { return "developmentHold"; }
        }

        public string IMAGES_PROPERTY_NAME { get { return "Images"; } }

        public string IMAGEGALLERY_PROPERTY_NAME { get { return "ImageGallery"; } }

        public string UNITOVERVIEW_PROPERTY_NAME { get { return "UnitOverview"; } }

        public string NEARESTMETRO_PROPERTY_NAME { get { return "nearestMetroStation"; } }

        public string NEARESTSCHOOLS_PROPERTY_NAME { get { return "nearestSchools"; } }
        public string FITTING_PROPERTY_NAME { get { return "fittingsAndFixtures"; } }

        public string VIEWS_PROPERTY_NAME   { get { return "unitViews";  } }

        public string FACILITIES_PROPERTY_NAME
        {
            get
            {
                return "facilitiesAndAmenities";
            }
        }

        public string OWNERID_PROPERTY_NAME { get { return "ownerID"; } }

        public string DETAILPAGEID_PROPERTY_NAME { get { return "detailPageID"; } }








    }
}