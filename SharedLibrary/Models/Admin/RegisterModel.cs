using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SharedLibrary.Models.Admin
{
    public class RegisterModel : UserModel
    {
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
