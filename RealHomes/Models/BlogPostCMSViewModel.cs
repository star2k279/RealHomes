using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Mvc;

using Umbraco.Core.Models;
using System.Web.Mvc;

namespace RealHomes.Models
{
    public class BlogPostCMSViewModel
    {


        public string PostId { get; set; }
        public string PostUrl { get; set; }

        public string TITLE_PROPERTY_NAME
        {
            get
            {
                return "pageTitle";
            }
        }
        public string PostTitle { get; set; }

        public string EXERPT_PROPERTY_NAME
        {
            get
            {
                return "exerpt";
            }
        }
        public string Exerpt { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string FULLTEXT_PROPERTY_NAME
        {
            get
            {
                return "fullPostText";
            }
        }
        public string FullBodyText { get; set; }

        public string IMAGE_PROPERTY_NAME
        {
            get
            {
                return "blogPostImage";
            }
        }
        public IPublishedContent Image { get; set; }

        public string CAT_PROPERTY_NAME
        {
            get
            {
                return "categories";
            }
        }
        public List<SelectListItem> Categories;

        public string LABEL_PROPERTY_NAME
        {
            get
            {
                return "labels";
            }
        }
        public IEnumerable<string> Labels;

        public string KEYWORD_PROPERTY_NAME
        {
            get
            {
                return "blogKeywords";
            }
        }
        public string SEOKeywords { get; set; }

        public string DESC_PROPERTY_NAME
        {
            get
            {
                return "blogDescription";
            }
        }
        public string BlogDescription { get; set; }

        public string HIDENAV_PROPERTY_NAME
        {
            get
            {
                return "blogHideInNavigation";
            }
        }
        public bool HideInNavigation { get; set; }

    }
}