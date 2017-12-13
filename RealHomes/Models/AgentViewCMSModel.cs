using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;

namespace RealHomes.Models
{
    public class AgentViewCMSModel
    {
        public string GROUP_ALIAS { get { return "Agent"; } }

        public string ID_PROPERTY_NAME { get { return "id"; } }
        public long AgentId { get; set; }

        public string NAME_PROPERTY_NAME { get { return "Username"; } }
         public string UserName { get; set; }


        public string DISPLAYNAME_PROPERTY_NAME { get { return "Name"; } }
        public string DisplayName { get; set; }


        public string PASSWORD_PROPERTY_NAME { get { return "Password"; } }
        public string Password { get; set; }

        public string EMAIL_PROPERTY_NAME { get { return "Email"; } }
       public string UserEmail { get; set; }

        public string TYPE_PROPERTY_NAME { get { return "Type"; } }
        public string Type { get; set; }


        public string COUNTRY_PROPERTY_NAME { get { return "agentCountry"; } }
        public string Country { get; set; }

        public string CITY_PROPERTY_NAME { get { return "agentCity"; } }
        public string City { get; set; }

        public string MOBILE_PROPERTY_NAME { get { return "agentMobileNo"; } }
        public string MobileNo { get; set; }

        public string CATEGORY_PROPERTY_NAME { get { return "agentServiceCategory"; } }
        public string ServiceCategory { get; set; }

        public string SERVICE_PROPERTY_NAME { get { return "agentServiceDomain"; } }
        public string Service { get; set; }

        public string SSN_PROPERTY_NAME { get { return "agentSocialSecurityNo"; } }
        public string SocialSecurityNo { get; set; }

        public string JOINDATE_PROPERTY_NAME { get { return "agentJoiningDate"; } }
        public string JoinDate { get; set; }

        public int totalExpYrs { get; set; }

        public int totalExpMnths { get; set; }
        public string RERA_PROPERTY_NAME { get { return "agentReraNo"; } }
        public string ReraNo { get; set; }

        public string IMAGE_PROPERTY_NAME { get { return "agentImage"; } }
        public IPublishedContent Image { get; set; }
    }
}