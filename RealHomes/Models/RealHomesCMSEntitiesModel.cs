using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class RealHomesCMSEntitiesModel
    {
        public string CITY_SETTING_NAME { get { return "City"; } }
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
        

    }
}