using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RealHomes.Models
{
    public class AgentCMSModel
    {
        public string GROUP_ALIAS { get { return "Agent"; } }

        public string NAME_PROPERTY_NAME { get { return "Username"; } }
        [Required]
        public string UserName { get; set; }


        public string DISPLAYNAME_PROPERTY_NAME { get { return "Name"; } }
        public string DisplayName { get; set; }


        public string PASSWORD_PROPERTY_NAME { get { return "Password"; } }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$", ErrorMessage = "Password must be minimum 8 character long and should contain at least one upper case letter, at least one lower case letter and numbers.")]
        public string Password { get; set; }

        public string EMAIL_PROPERTY_NAME { get { return "Email"; } }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public string TYPE_PROPERTY_NAME { get { return "Type"; } }
        public int Type { get; set; }


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

        public string _PROPERTY_NAME { get { return "agentSocialSecurityNo"; } }
        public string SocialSecurityNo { get; set; }

        public string JOINDATE_PROPERTY_NAME { get { return "agentJoiningDate"; } }
        public string JoinDate { get; set; }

        public string RERA_PROPERTY_NAME { get { return "agentReraNo"; } }
        public string ReraNo { get; set; }

        public string IMAGE_PROPERTY_NAME { get { return "agentImage"; } }
        public string Image { get; set; }

}
}