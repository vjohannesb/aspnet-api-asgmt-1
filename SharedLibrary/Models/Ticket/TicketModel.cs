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

        public TicketModel()
        {
        }

        public TicketModel(TicketViewModel tvm)
        {
            TicketId = tvm.TicketId;
            Description = tvm.Description;
            DateCreated = tvm.DateCreated;
            DateUpdated = tvm.DateUpdated;
            Status = tvm.Status;
            CustomerId = tvm.CustomerId;
            AssignedAdminId = tvm.AdminId;
        }

        [Key]
        [Required]
        public int TicketId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        public virtual int? CustomerId { get; set; }
        public virtual CustomerModel Customer { get; set; }

        public virtual int? AssignedAdminId { get; set; }
        public virtual AdminModel AssignedAdmin { get; set; }

        [NotMapped]
        public AdminViewModel AdminViewModel { get; set; }

        [NotMapped]
        public CustomerViewModel CustomerViewModel { get; set; }
    }

    // Explicit numrering för tydlighets skull
    public enum TicketStatus
    {
        Open = 0,
        Active = 1,
        Closed = 2
    }
}
