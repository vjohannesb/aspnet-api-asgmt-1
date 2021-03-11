using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Ticket
{
    public class TicketModel
    {

        public TicketModel() { }

        public TicketModel(TicketRequestModel trm)
        {
            Description = trm.Description;
            DateCreated = trm.DateCreated;
            DateUpdated = trm.DateUpdated;
            Status = trm.Status;
            CustomerId = trm.CustomerId;
            AssignedAdminId = trm.AssignedAdminId;
        }

        [Key]
        [Required]
        public Guid TicketId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        public virtual Guid? CustomerId { get; set; }
        public virtual CustomerModel Customer { get; set; }

        public virtual Guid? AssignedAdminId { get; set; }
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
