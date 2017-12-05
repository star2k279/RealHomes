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
    public class VisitEnquiryCMSViewModel
    {
        public string RefNo { get; set; }

        public string NAME_PROPERTY_NAME { get { return "prospectName"; } }
        public string ProspectName { get; set; }

        public string EMAIL_PROPERTY_NAME { get { return "email"; } }
        public string Email { get; set; }

        public string PROPDETAIL_PROPERTY_NAME { get { return "propertyDetail"; } }
        public string PropertyDetail { get; set; }

        public string ENQDETAIL_PROPERTY_NAME { get { return "enquiryDetail"; } }
        public string EnquiryDetail { get; set; }

        

    }
}