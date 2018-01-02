using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class PagedBlogPostCMSViewModel
    {
        public IEnumerable<BlogPostCMSViewModel> BlogPostModels;
        public int TotalRecords;
        public int TotalPages;
        public int CurrentPage;
        public int StartPage;
        public int EndPage;
    }
}