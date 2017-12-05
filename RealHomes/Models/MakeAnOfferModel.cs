using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class MakeAnOfferModel
    {
        

        public string RefNo { get; set; }

        public string NAME_PROPERTY_NAME { get { return "personName"; } }
        public string ProspectName { get; set; }

        public string EMAIL_PROPERTY_NAME { get { return "email"; } }
        public string Email { get; set; }

        public string OFFER_PROPERTY_NAME { get { return "offerPrice"; } }
        public long OfferPrice;

        public string FINANCING_PROPERTY_NAME { get { return "financingRequired"; } }
        public bool financing;


    }
}