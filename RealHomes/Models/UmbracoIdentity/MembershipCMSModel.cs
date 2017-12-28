using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace RealHomes.Models
{
    public class MembershipCMSModel
    {
        public string GROUP_ALIAS { get { return "Client"; } }

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


        public string COUNTRY_PROPERTY_NAME { get { return "memberCountry"; } }
        public int Country { get; set; }

        public string CITY_PROPERTY_NAME { get { return "memberCity"; } }
        public int City { get; set; }

        public string CONTACT_PROPERTY_NAME { get { return "memberContactNo"; } }
        public string ContactNo { get; set; }


    }
}