using System;
using System.Web.Mvc;
using System.Collections.Generic;

using System.Configuration;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;
using RealHomes.Helper;
using umbraco.cms.businesslogic.web;

using System.Threading.Tasks;
using System.Web.Configuration;
using RealHomes.Models;
namespace RealHomes.Controllers
{
    public class MakeAnOfferCMSController : SurfaceController
    {
        private const string PARENTNODE_SETTING = "offers";
        private const string NODE_NAME = "Offer-";
        private const string NODE_TYPE_ALIAS = "addOffers";

        private const string VIEW_NAME = "PropertyCMS/_Offers.cshtml";

        [HttpGet]
        public async Task<string> SaveOffer(string sRefNo, string sName, string sEmail, long OfferPrice, bool financing)
        {
            try
            {
                //Create a dynamic node in the CMS under Enquiries Node
                UmbracoUtils utils = new UmbracoUtils();
                string result; MakeAnOfferModel model = new MakeAnOfferModel();
                Dictionary<string, string> properties = new Dictionary<string, string>();
                int parent = Convert.ToInt32(WebConfigurationManager.AppSettings[PARENTNODE_SETTING]);

                properties.Add(model.NAME_PROPERTY_NAME, sName);
                properties.Add(model.EMAIL_PROPERTY_NAME, sEmail);
                properties.Add(model.OFFER_PROPERTY_NAME, OfferPrice.ToString());
                properties.Add(model.FINANCING_PROPERTY_NAME, (financing ? "YES":"NO"));

                string nodeName = NODE_NAME + sRefNo + "-" + DateTime.Now.ToString("MMddyyhhmmss");
                //dddd, dd MMMM yyyy HH:mm:ss

                int newId = utils.CreateContentNode(nodeName, NODE_TYPE_ALIAS, parent, properties);
                //Set values in the 
                if (newId != 0)
                {
                    //IPublishedContent node =  Umbraco.Content(newId);

                    result = "SUCCESS";
                }
                else
                {
                    result = "ERROR";
                }

                return await System.Threading.Tasks.Task.FromResult(result);
            }
            catch (Exception ex)
            { return ""; }
        }
    }
}