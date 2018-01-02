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
using RealHomes.Helper;
using System.Collections;

namespace RealHomes.Controllers
{
    public class BlogCMSController : SurfaceController
    {
        private const string PARTIAL_VIEW_LIST = "BlogCMS/_BlogList.cshtml";
        private const string PARTIAL_VIEW_DETAIL = "BlogCMS/_BlogPost.cshtml";
        private const string ERROR_SETTING_NAME = "ErrorPageId";


        [HttpGet]
        public async Task<ActionResult> GetBlogPostList(int iPageno, string sSortName)
        {
            int iPageSize = Convert.ToInt32(StringConstants.BLOG_PAGE_SIZE);
            PagedBlogPostCMSViewModel pagedView = new PagedBlogPostCMSViewModel();
            //IEnumerable<IPublishedContent> pagedResults;
             
            try
            {

                if (Request.IsAjaxRequest())
                {
                    RealHomesDataTypesCMSModel settingsName = new RealHomesDataTypesCMSModel();
                    
                    //Get the All the posts under blog content node
                    var results = Umbraco.TypedContent(WebConfigurationManager.AppSettings[settingsName.BLOGMAINCT_SETTING_NAME]).Children();

                    if (results.Count() > 0)
                        pagedView.TotalRecords = results.Count();
                    else
                        pagedView.TotalRecords = 0;


                    //filtered results for requested page
                    var pagedResults = results.Skip((iPageno - 1) * iPageSize).Take(iPageSize);


                    if (pagedResults.Count() > 0)
                    {
                        //Prepare Property Models to be displayed on the result page
                        List<BlogPostCMSViewModel> BlogPosts = new List<BlogPostCMSViewModel>();
                        foreach (var result in pagedResults)
                        {
                            BlogPostCMSViewModel Post = new BlogPostCMSViewModel();
                            //IPublishedContent node = Umbraco.Content(result.Id);

                            Post = MapNodeToModel(result);

                            BlogPosts.Add(Post);

                        }
                        pagedView.BlogPostModels = BlogPosts;


                    }
                }

                pagedView.TotalPages = Convert.ToInt32(Math.Ceiling((double)pagedView.TotalRecords / iPageSize));
                pagedView.CurrentPage = iPageno;

                SetPagingValues(ref pagedView);

                PagedPropertyViews pagedresults = new PagedPropertyViews();

                
                return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_LIST, await System.Threading.Tasks.Task.FromResult(pagedView));


            }
            catch (Exception x)
            {
                //log the error
                return null;
            }
        }
        

        public BlogPostCMSViewModel MapNodeToModel(IPublishedContent node)
        {
            RealHomesDataTypesCMSModel cmsDataTypesName = new RealHomesDataTypesCMSModel();

            BlogPostCMSViewModel post = new BlogPostCMSViewModel();
            
            post.PostId = node.Id.ToString();
            post.PostUrl = node.Url;
            post.PostTitle = node.GetPropertyValue(post.TITLE_PROPERTY_NAME) == null ? "": node.GetPropertyValue(post.TITLE_PROPERTY_NAME).ToString();
            post.Exerpt = node.GetPropertyValue(post.EXERPT_PROPERTY_NAME) == null ? "" : node.GetPropertyValue(post.EXERPT_PROPERTY_NAME).ToString();
            post.FullBodyText = node.GetPropertyValue(post.FULLTEXT_PROPERTY_NAME) == null ? "": node.GetPropertyValue(post.FULLTEXT_PROPERTY_NAME).ToString();
            
            post.Labels = node.GetPropertyValue(post.LABEL_PROPERTY_NAME) == null ? null : node.GetPropertyValue<IEnumerable<string>>(post.LABEL_PROPERTY_NAME);
            //post.Categories= (new PreValueHelper()).GetPreValuesFromAppSettingName(cmsDataTypesName.BLOGCAT_SETTING_NAME);

            post.Image = node.GetPropertyValue(post.IMAGE_PROPERTY_NAME) == null ? null : node.GetPropertyValue<IPublishedContent>(post.IMAGE_PROPERTY_NAME);

            post.SEOKeywords = node.GetPropertyValue(post.KEYWORD_PROPERTY_NAME) == null ? "" : node.GetPropertyValue(post.KEYWORD_PROPERTY_NAME).ToString();
            post.BlogDescription = node.GetPropertyValue(post.DESC_PROPERTY_NAME) == null ? "" : node.GetPropertyValue(post.DESC_PROPERTY_NAME).ToString();
            post.HideInNavigation = node.GetPropertyValue(post.HIDENAV_PROPERTY_NAME) == null ? true : node.GetPropertyValue<bool>(post.HIDENAV_PROPERTY_NAME);

            return post;
        }


        private void SetPagingValues(ref PagedBlogPostCMSViewModel pager)
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
        public ActionResult ShowBlogPost(string postId)
        {
            if (String.IsNullOrEmpty(postId))
            {
                //Redirect to Error Page
                return RedirectToUmbracoPage(Convert.ToInt32(WebConfigurationManager.AppSettings["ErrorPageId"]));
            }
            BlogPostCMSViewModel postModel = new BlogPostCMSViewModel();
            IPublishedContent postNode = Umbraco.TypedContent(postId);

            postModel = MapNodeToModel(postNode);
            
            return PartialView(StringConstants.PARTIAL_VIEW_PATH + PARTIAL_VIEW_DETAIL,postModel);
        }
        [HttpPost]
        public ActionResult SaveBlogPostComment(BlogPostCommentsViewCMSModel model, string postId)
        {
           


            return null;
        }

    }
}