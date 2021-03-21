using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Customer;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.Ticket
{
    public class TicketViewModel
    {
        public TicketViewModel() { }

        public TicketViewModel(TicketModel tm)
        {
            TicketId = tm.TicketId;
            Description = tm.Description;
            DateCreated = tm.DateCreated;
            DateUpdated = tm.DateUpdated;
            Status = tm.Status;
            CustomerId = tm.CustomerId;
            Customer = tm.Customer != null ? new CustomerViewModel(tm.Customer) : null;
            AdminId = tm.AdminId;
            Administrator = tm.Admin != null ? new AdminViewModel(tm.Admin) : null;
        }

        public int TicketId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        public int? CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }

        public int? AdminId { get; set; }
        public AdminViewModel Administrator { get; set; }
    }
}
