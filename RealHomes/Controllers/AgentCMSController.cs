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
    public class AgentCMSController : SurfaceController
    {
        private const string PARTIAL_VIEW_LIST = "AgentCMS/_AgentList.cshtml";
        private const string PARTIAL_VIEW_DETAIL = "AgentCMS/_AgentDetails.cshtml";
        private const string ERROR_SETTING_NAME = "ErrorPageId";
        

        [HttpGet]
        public async Task<ActionResult> GetAgents(int iPageno, string sSortName, string sAgentId, string sCategory, string sService, string sCity)
        {
            int iPageSize = Convert.ToInt32(StringConstants.PAGE_SIZE);
            PagedAgentCMSView pagedView = new PagedAgentCMSView();

            try
            {

                if (Request.IsAjaxRequest())
                {
                    //AdvSearchModel srchModel = new AdvSearchModel();
                    var searcher = Examine.ExamineManager.Instance.SearchProviderCollection[StringConstants.EXMINE_MEM_SEARCHER_NAME];
                    IEnumerable<SearchResult> pagedResults;
                    IBooleanOperation query = GetCriteria(sAgentId,  sCategory,  sService,  sCity, ref searcher);
                                        
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
                        List<AgentViewCMSModel> properties = new List<AgentViewCMSModel>();
                        foreach (var result in pagedResults)
                        {
                            AgentViewCMSModel item = new AgentViewCMSModel();

                            //IMember member = Services.MemberService.GetById(Convert.ToInt32(result.Fields["id"]));

                            IPublishedContent node = Members.GetById(Convert.ToInt32(result.Fields["id"]));

                            item = MapNodeToModel(node);

                            properties.Add(item);

                        }
                        pagedView.CMSAgents = properties;


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
        private IBooleanOperation GetCriteria(string sAgentId,string sCategory,string sService,string sCity, ref BaseSearchProvider searcher)
        {

            Dictionary<string, string> values = new Dictionary<string, string>();
            AgentViewCMSModel srchModel = new AgentViewCMSModel();
            

            var searchCriteria = searcher.CreateSearchCriteria(BooleanOperation.And);

            IBooleanOperation query = null;
            try
            {
                //Built the exmine query
                query = searchCriteria.Field(srchModel.TYPE_PROPERTY_NAME, "Agent");

                if (sAgentId != null && sAgentId != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.NAME_PROPERTY_NAME, sAgentId);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.NAME_PROPERTY_NAME, sAgentId);
                    }
                }
               

                if (sCategory != null && sCategory != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.CATEGORY_PROPERTY_NAME, sCategory);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.CATEGORY_PROPERTY_NAME, sCategory);
                    }

                }

                
                //sService

                if (sService != null && sService != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.SERVICE_PROPERTY_NAME, sService);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.SERVICE_PROPERTY_NAME, sService);
                    }
                }
                //, sDevhold
                if (sCity != null && sCity != "")
                {
                    if (query == null)
                    {

                        query = searchCriteria.Field(srchModel.CITY_PROPERTY_NAME, sCity);

                    }
                    else
                    {
                        query = query.And().Field(srchModel.CITY_PROPERTY_NAME, sCity);
                    }
                }

                //If no criteria was provided from page.
                
                return query;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void SetPagingValues(ref PagedAgentCMSView pager)
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
        public ActionResult Showdetail(string AgentId)
        {
            string errorPageId = WebConfigurationManager.AppSettings[ERROR_SETTING_NAME] as string;
            try
            {
                //IPublishedContent node = Umbraco.Content(AgentId);
                IPublishedContent node = Members.GetById(Convert.ToInt32(AgentId));

                if (node == null)
                {
                    
                    RedirectToUmbracoPage(Convert.ToInt32(errorPageId));
                }

                AgentViewCMSModel model = MapNodeToModel(node);

                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_DETAIL, model);
            }
            catch (Exception ex)
            {
                //log the exception properly
                //RedirectToCurrentUmbracoPage();
                RedirectToUmbracoPage(Convert.ToInt32(errorPageId));
                return null;
            }
        }


        public AgentViewCMSModel MapNodeToModel(IPublishedContent agent)
        {
            AgentViewCMSModel item = new AgentViewCMSModel();
            IMember member = Services.MemberService.GetById(agent.Id);

            
            item.AgentId = Convert.ToInt64(agent.Id);
            item.DisplayName = member.Name == null?"" : member.Name;
            item.UserName = member.Username == null?"": member.Username;
            item.UserEmail = member.Email == null?"":member.Email;
            
            //fill the rest of information form published node content
            item.Type = agent.GetPropertyValue(item.TYPE_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.TYPE_PROPERTY_NAME).ToString();
            item.Country = agent.GetPropertyValue(item.COUNTRY_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.COUNTRY_PROPERTY_NAME).ToString();
            item.City = agent.GetPropertyValue(item.CITY_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.CITY_PROPERTY_NAME).ToString();
            item.ServiceCategory = agent.GetPropertyValue(item.CATEGORY_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.CATEGORY_PROPERTY_NAME).ToString();
            item.Service = agent.GetPropertyValue(item.SERVICE_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.SERVICE_PROPERTY_NAME).ToString();
            item.SocialSecurityNo = agent.GetPropertyValue(item.SSN_PROPERTY_NAME) == null ? "" : agent.GetPropertyValue(item.SSN_PROPERTY_NAME).ToString();
            item.JoinDate = agent.GetPropertyValue(item.JOINDATE_PROPERTY_NAME)== null ? "" : agent.GetPropertyValue(item.JOINDATE_PROPERTY_NAME).ToString();

            SetYearsAndMonths((DateTime.Now - Convert.ToDateTime(item.JoinDate)), ref item);
                
            item.ReraNo = agent.GetPropertyValue(item.RERA_PROPERTY_NAME) == null ?"" : agent.GetPropertyValue(item.RERA_PROPERTY_NAME).ToString();
            
            if (agent.GetPropertyValue(item.IMAGE_PROPERTY_NAME) != null)
            {
                item.Image = agent.GetPropertyValue<IPublishedContent>(item.IMAGE_PROPERTY_NAME);
                    
            }

            //item.Overview = node.GetPropertyValue(item.AgentDetail_PROPERTY_NAME).ToString();
            //item.OwnerId = node.GetPropertyValue(item.OWNERID_PROPERTY_NAME).ToString();
            agent = null;
            member = null;

            return item;
        }

        
            public void SetYearsAndMonths(TimeSpan timespan, ref AgentViewCMSModel item)
            {
                int years = 0,mnths = 0, remDays = 0;

            if (timespan.TotalDays > 300)
            {
                years = (int)timespan.TotalDays / 300;
                remDays = (int)timespan.TotalDays % 300;
                if (remDays > 30)
                {
                    mnths = remDays / 30;
                }
            }
            item.totalExpYrs = years;
            item.totalExpMnths = mnths;
            }
            
            public int GetMonths(TimeSpan timespan)
            {
                return (int)(timespan.Days / 30.436875);
            }
        
    }
}