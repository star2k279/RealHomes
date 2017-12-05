using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

using Umbraco.Web;

namespace RealHomes.Helper
{
        public class UmbracoUtils
        {
        /// <summary>  
        /// Create and publish the document programatically  
        /// </summary>  
        /// <param name="nodeName"></param>  
        /// <param name="properties"></param>  
        /// <param name="documentType"></param>  
        /// <param name="parentId"></param>  
        /// <returns>node id</returns>  
        public int CreateDetailContentNode(string nodeName, Dictionary<string, string> properties, List<string> roles = null)
        {
            try
            {
                //parentid = 1189
                Int32 uid = 0;
                ContentService cs = (ContentService)ApplicationContext.Current.Services.ContentService;

                UserService us = (UserService)ApplicationContext.Current.Services.UserService;

                Umbraco.Core.Models.Membership.IUser usr = us.GetByUsername("najm_us_sahar@hotmail.com");

                if (usr != null) uid = usr.Id;
                

                // Create the Node under 
                Content parent = (Content)cs.GetById(1189);
                
                Content c = (Content)cs.CreateContent(nodeName, parent, "propertyDetail");

                // Add values to the generic properties of the document   
                if (properties != null)
                {
                    foreach (string property in properties.Keys)
                    {
                        //c.getProperty(properties[property]).Value = properties[property];
                        //c.Properties.
                    }
                }
                // Set the publish status of the document and there by create a new version   
                /*if (roles != null)
                {
                    int loginDocId = Constants.NODE_ID_HOME;
                    int errorDocId = Constants.NODE_ID_HOME;
                    umbraco.cms.businesslogic.web.Access.ProtectPage(false, d.Id, loginDocId, errorDocId);
                    foreach (string role in roles)
                    {
                        umbraco.cms.businesslogic.web.Access.AddMembershipRoleToDocument(d.Id, role);
                    }
                }*/
                // d.Publish(u);
                // Tell the runtime environment to publish this document   
                cs.Publish(c, uid);
                return c.Id;
            }
            catch (Exception ex) { return 0; }

        }

        public int CreateContentNode(string nodeName,string nodeTypeAlias, int parentNode, Dictionary<string, string> properties, List<string> roles = null)
        {
            try
            {
                //parentid = 1189
                Int32 uid = 0;
                ContentService cs = (ContentService)ApplicationContext.Current.Services.ContentService;

                UserService us = (UserService)ApplicationContext.Current.Services.UserService;

                Umbraco.Core.Models.Membership.IUser usr = us.GetByUsername("najm_us_sahar@hotmail.com");

                if (usr != null) uid = usr.Id;


                // Create the Node under 
                Content parent = (Content)cs.GetById(parentNode);

                IContent c = (IContent)cs.CreateContent(nodeName, parent, nodeTypeAlias);

                foreach (string property in properties.Keys)
                {
                    c.SetValue(property, properties[property].ToString());
                }
                
                cs.SaveAndPublishWithStatus(c, uid);
                
                return c.Id;
            }
            catch (Exception ex) { return 0; }

        }
    }
    }