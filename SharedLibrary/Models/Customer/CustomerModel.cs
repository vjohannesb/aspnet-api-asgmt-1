using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models.Customer
{
    public class CustomerModel : UserModel
    {
        public CustomerModel() { }

        public CustomerModel(CustomerViewModel cvm)
        {
            CustomerId = cvm.CustomerId;
            FirstName = cvm.FirstName;
            LastName = cvm.LastName;
            Email = cvm.Email;
        }

        [Key]
        [Required]
        public int CustomerId { get; set; }

        [NotMapped]
        public string DisplayName => $"{FirstName} {LastName}";
    }
}
