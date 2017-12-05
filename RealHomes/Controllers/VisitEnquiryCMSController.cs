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
    public class VisitEnquiryCMSController : SurfaceController
    {
        private const string PARENTNODE_SETTING = "enquiries";
        private const string NODE_NAME = "VisitEnquiry-";
        private const string NODE_TYPE_ALIAS = "viewEnquiry";
       
        private const string VIEW_NAME = "PropertyCMS/_VisitEnquiry.cshtml";

        [HttpGet]
        public async Task<string> CreateEnquiry(string sRefNo, string sName, string sEmail,string sPropDetail, string sEnqDetail)
        {
            try
            {
                //Create a dynamic node in the CMS under Enquiries Node
                UmbracoUtils utils = new UmbracoUtils();
                string result; VisitEnquiryCMSViewModel model = new VisitEnquiryCMSViewModel();
                Dictionary<string, string> properties = new Dictionary<string, string>();
                int parent = Convert.ToInt32(WebConfigurationManager.AppSettings[PARENTNODE_SETTING]);

                properties.Add(model.NAME_PROPERTY_NAME,sName);
                properties.Add(model.EMAIL_PROPERTY_NAME, sEmail);
                properties.Add(model.PROPDETAIL_PROPERTY_NAME, sPropDetail);
                properties.Add(model.ENQDETAIL_PROPERTY_NAME, sEnqDetail);

                string nodeName = NODE_NAME + sRefNo+"-"+ DateTime.Now.ToString("MMddyyhhmmss");
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
            catch(Exception ex)
            { return ""; }
        }
    }
}