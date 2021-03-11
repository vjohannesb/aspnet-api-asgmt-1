using SharedLibrary.Models.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models.Ticket
{
    public class TicketRequestModel
    {
        public TicketRequestModel() { }

        public TicketRequestModel(TicketModel tm)
        {
            TicketId = tm.TicketId;
            Description = tm.Description;
            DateCreated = tm.DateCreated;
            DateUpdated = tm.DateUpdated;
            Status = tm.Status;
            CustomerId = tm.CustomerId;
            AssignedAdminId = tm.AssignedAdminId;
        }

        public Guid TicketId { get; set; }

        [Required(ErrorMessage = "Description required.")]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? AssignedAdminId { get; set; }
    }
}
