using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class TicketModel
    {
        [Key]
        [Required]
        public string TicketId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        public virtual string CustomerId { get; set; }
        public virtual CustomerModel Customer { get; set; }

        public virtual string AssignedAdminId { get; set; }
        public virtual AdminModel AssignedAdmin { get; set; }
    }

    // Explicit numrering för tydlighets skull
    public enum TicketStatus
    {
        Open = 0,
        Active = 1,
        Closed = 2
    }
}
