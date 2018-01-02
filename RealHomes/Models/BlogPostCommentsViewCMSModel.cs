using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class BlogPostCommentsViewCMSModel
    {


        public string PostId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public string NAME_PROPERTY_NAME
        {
            get
            {
                return "commenterName";
            }
        }
        public string EMAIL_PROPERTY_NAME
        {
            get
            {
                return "commenterEmail";
            }
        }
        public string SUBJECT_PROPERTY_NAME
        {
            get
            {
                return "commenterSubject";
            }
        }
        public string COMMENT_PROPERTY_NAME
        {
            get
            {
                return "comment";
            }
        }
    }
}