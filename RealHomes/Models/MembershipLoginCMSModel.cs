using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace RealHomes.Models
    {
        public class MembershipLoginCMSModel
        {
            public string UserName { get; set; }
            public string DisplayName { get; set; }
           
            public string Email { get; set; }
            public string Password { get; set; }

         
        }
}