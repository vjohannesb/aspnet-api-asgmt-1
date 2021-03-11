using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Customer
{
    public class CustomerRegisterModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "A first name of between 2 and 50 characters is required.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "A last name of between 2 and 50 characters is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "An email address is required.")]
        [EmailAddress]
        public string Email { get; set; } 
    }
}
