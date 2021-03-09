using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class AdminModel : User
    {
        [Key]
        [Required]
        public string AdminId { get; set; }
        
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] AdminSalt { get; set; }
        
        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] AdminHash { get; set; }

        public virtual IEnumerable<TicketModel> AssignedTickets { get; set; }

    }
}
