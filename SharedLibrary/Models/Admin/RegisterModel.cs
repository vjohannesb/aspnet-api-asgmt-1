using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SharedLibrary.Models.Admin
{
    public class RegisterModel
    {
        
        [Required(ErrorMessage = "First name is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be at least 5 characters long.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Your password must contain at least 10 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [CompareProperty(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
