using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IDbService
    {

        public Task<CustomerModel> GetCustomerAsync(string customerId);
        public Task<IEnumerable<CustomerModel>> GetCustomersAsync();
        public Task<IActionResult> CreateCustomerAsync(CustomerModel model);
        public Task<IActionResult> UpdateCustomerAsync(CustomerModel model);


        public Task<TicketModel> GetTicketAsync(string ticketId);
        public Task<ActionResult<IEnumerable<TicketModel>>> GetTicketsAsync();
        public Task<IActionResult> CreateTicketAsync(TicketModel model);
        public Task<IActionResult> UpdateTicketAsync(TicketModel model);


    }
}
