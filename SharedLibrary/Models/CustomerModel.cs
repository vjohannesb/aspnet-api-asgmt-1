using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class CustomerModel : User
    {
        [Key]
        [Required]
        public string CustomerId { get; set; }

        public virtual IEnumerable<TicketModel> Tickets { get; set; }
    }
}
