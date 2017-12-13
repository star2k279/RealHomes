using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class RealHomesDataTypesCMSModel
    {
        public string COUNTRY_SETTING_NAME { get { return "Country"; } }
        public string CITY_SETTING_NAME { get { return "City"; } }
        public string COMMUNITY_SETTING_NAME { get { return "Community"; } }
        public string LOCATION_SETTING_NAME { get { return "Location"; } }
        public string SERVICE_SETTING_NAME {get{ return "Service";}}
        public string CATEGORY_SETTING_NAME { get { return "Category"; } }
        public string TYPE_SETTING_NAME { get { return "PropertyType"; } }
        public string FITTING_SETTING_NAME { get { return "FittingsAndFixtures"; } }
        public string UNITVIEW_SETTING_NAME { get { return "UnitViews"; } }
        public string FACILITIES_SETTING_NAME { get { return "FacilitiesAndAmenities"; } }
        public string MINBED_SETTING_NAME { get { return "MinBed"; } }
        public string MAXBED_SETTING_NAME { get { return "MaxBed"; } }
        public string MINPRICE_SETTING_NAME { get { return "MinPrice"; } }
        public string MAXPRICE_SETTING_NAME { get { return "MaxPrice"; } }
        public string DEVHOLD_SETTING_NAME { get { return "DevelopmentHold"; } }
        public string STATUS_SETTING_NAME { get { return "AvailabilityStatus"; } }

        //Membership Data Types

        public string MEMBERTYPE_SETTING_NAME { get { return "MemberType"; } }

         //DOC TYPE SETTING NAMES 
        
         public string PROPERTYLISTDT_SETTING_NAME { get { return "property-list"; } }
         public string PROPERTYDT_SETTING_NAME { get { return "properties"; } }
         public string ENQUIRIESDT_SETTING_NAME { get { return "enquiries"; } }
         public string OFFERSDT_SETTING_NAME { get { return "offers"; } }
         public string PROPERTYLIST_SETTING_NAME { get { return "ErrorPageId"; } }
    
        //Content Type (Node) Setting Names

        public string PROPDETAILCT_SETTING_NAME { get { return "PropertyDetailPageID"; } }
        public string AGENTDETAILCT_SETTING_NAME { get { return "AgentDetailPageID"; } }
    }
}