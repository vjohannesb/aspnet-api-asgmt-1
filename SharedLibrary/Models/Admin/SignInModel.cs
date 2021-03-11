using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.Admin
{
    public class SignInModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be at least 5 characters long.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ResponseModel
    {
        public ResponseModel(bool succeeded, string token)
        {
            Succeeded = succeeded;
            Token = token;
        }

        public bool Succeeded { get; set; }

        public string Token { get; set; }

    }
}
