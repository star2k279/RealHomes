using System.ComponentModel.DataAnnotations;

namespace RealHomes.Models.UmbracoIdentity
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
