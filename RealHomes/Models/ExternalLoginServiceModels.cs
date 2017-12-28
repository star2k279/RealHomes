using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RealHomes.Models
{
    public class ExternalLoginServiceListModel
    {
        public string ReturnUrl { get; set; }
    }

    public class ExternalLoginConfirmaitonModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}