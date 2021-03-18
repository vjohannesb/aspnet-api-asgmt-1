using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Customer
{
    public class CustomerModel : UserModel
    {
        [Key]
        [Required]
        public int CustomerId { get; set; }

        [NotMapped]
        public string DisplayName => $"{FirstName} {LastName}";
    }
}
