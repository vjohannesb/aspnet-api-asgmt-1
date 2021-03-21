using System.ComponentModel.DataAnnotations;

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
        [CompareProperty(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
