using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "A first name of between 1 and 50 characters is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "A first name of between 2 and 50 characters is required.")]
        [DataType(DataType.Text)]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A last name of between 1 and 50 characters is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "A last name of between 2 and 50 characters is required.")]
        [DataType(DataType.Text)]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "An email of between 3 and 100 characters is required.")]
        [EmailAddress]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        public virtual IEnumerable<TicketModel> Tickets { get; set; }
    }
}
