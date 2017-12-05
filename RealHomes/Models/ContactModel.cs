
using System.ComponentModel.DataAnnotations;

namespace RealHomes.Models
{
    public class ContactModel
    {
        
        [Required]
        public string txtName { get; set; }

        [Required]
        [EmailAddress]
        public string txtEmail { get; set; }

        [Required]
        public string txtSubject { get; set; }

        [Required]
        public string txtMessage { get; set; }





    }
}