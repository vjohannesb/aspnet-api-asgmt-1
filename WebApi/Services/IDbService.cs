using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IDbService
    {
        public Task<CustomerModel> GetCustomerAsync(int? id);
        public Task<IEnumerable<CustomerModel>> GetCustomersAsync();
        public Task<IActionResult> CreateCustomerAsync(CustomerModel model);


        public Task<TicketModel> GetTicketAsync(int? ticketId);

        /// <summary>
        /// Konverterar TicketModels till TicketViewModels och försöker sortera per givna parametrar
        /// </summary>
        public IActionResult SortTickets(IEnumerable<TicketModel> tickets, string sort, string order);
        public Task<IEnumerable<TicketModel>> GetTicketsAsync();
        public Task<IActionResult> CreateTicketAsync(TicketModel model);
        public Task<IActionResult> UpdateTicketAsync(TicketModel model);
        public Task<IActionResult> DeleteTicketAsync(TicketModel ticket);
    }
}
