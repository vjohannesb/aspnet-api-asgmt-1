using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.Admin
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
