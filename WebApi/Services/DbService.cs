using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary.Models;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data;

namespace WebApi.Services
{
    public class DbService : IDbService
    {
        private readonly SqlDbContext _context;
        private readonly ILogger<DbService> _logger;

        public DbService(SqlDbContext context, ILogger<DbService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*** Customers ***/
        public async Task<IEnumerable<CustomerModel>> GetCustomersAsync()
            => await _context.Customers.ToListAsync();

        public async Task<CustomerModel> GetCustomerAsync(int? id)
            => await _context.Customers.FindAsync(id);

        public async Task<IActionResult> CreateCustomerAsync(CustomerModel customer)
        {
            _context.Customers.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
                // Location header = api/customers/{customerId} + en payload med CustomerId som Result
                return new CreatedAtActionResult(nameof(CustomersController.GetCustomer), "Customers",
                    new { id = customer.CustomerId },
                    new ResponseModel(true, customer.CustomerId.ToString()));
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                    return new ConflictResult();
                else
                    // Log?
                    throw;
            }
        }

        private bool CustomerExists(int id)
            => _context.Customers.Any(c => c.CustomerId == id);





        /*** Tickets ***/
        public async Task<TicketModel> GetTicketAsync(int? ticketId)
            => await _context.Tickets
                .Include(t => t.Customer)
                .Include(t => t.AssignedAdmin)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

        public async Task<IEnumerable<TicketModel>> GetTicketsAsync()
            => await _context.Tickets
                .Include(t => t.Customer)
                .Include(t => t.AssignedAdmin)
                .ToListAsync();


        public IActionResult SortTickets(IEnumerable<TicketModel> tickets, string sort, string order)
        {
            var ticketViewModels = tickets.Select(t => new TicketViewModel(t));

            if (string.IsNullOrEmpty(sort))
                return new OkObjectResult(ticketViewModels);

            switch (sort)
            {
                case "id":
                    if (order == "desc")
                        ticketViewModels = ticketViewModels.OrderByDescending(t => t.TicketId).ToList();
                    else
                        ticketViewModels = ticketViewModels.OrderBy(t => t.TicketId).ToList();
                    break;
                case "status":
                    if (order == "desc")
                        ticketViewModels = ticketViewModels.OrderByDescending(t => t.Status).ToList();
                    else
                        ticketViewModels = ticketViewModels.OrderBy(t => t.Status).ToList();
                    break;
                case "created":
                    if (order == "desc")
                        ticketViewModels = ticketViewModels.OrderByDescending(t => t.DateCreated).ToList();
                    else
                        ticketViewModels = ticketViewModels.OrderBy(t => t.DateCreated).ToList();
                    break;
                case "updated":
                    if (order == "desc")
                        ticketViewModels = ticketViewModels.OrderByDescending(t => t.DateUpdated).ToList();
                    else
                        ticketViewModels = ticketViewModels.OrderBy(t => t.DateUpdated).ToList();
                    break;
                // Specialare då Customer kan vara tomt,
                // detta ser till så nullvärden alltid hamnar längst ner
                case "customer":
                    if (order == "desc")
                        ticketViewModels = ticketViewModels.OrderBy(t => t.Customer == null)
                            .ThenByDescending(t => t.Customer?.FirstName).ToList();
                    else
                        ticketViewModels = ticketViewModels.OrderBy(t => t.Customer == null)
                            .ThenBy(t => t.Customer?.FirstName).ToList();
                    break;
                default:
                    break;
            }
            return new OkObjectResult(ticketViewModels);
        }

        public async Task<IActionResult> CreateTicketAsync(TicketModel ticket)
        {
            ticket.DateCreated = DateTime.Now;
            ticket.DateUpdated = DateTime.Now;

            _context.Tickets.Add(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                if (TicketExists(ticket.TicketId))
                    return new ConflictResult();
                else
                {
                    _logger.LogError($"[{dbEx.Source}] {dbEx.Message}");
                    _logger.LogError(dbEx.StackTrace);
                    return new StatusCodeResult(500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{ex.Source}] {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return new StatusCodeResult(500);
            }

            // Location header = api/tickets/{ticketId}, + en payload med TicketId som Result
            return new CreatedAtActionResult(nameof(TicketsController.GetTicket), "Tickets", 
                new { id = ticket.TicketId },
                new ResponseModel(true, ticket.TicketId.ToString()));
        }

        public async Task<IActionResult> UpdateTicketAsync(TicketModel ticket)
        {
            ticket.DateUpdated = DateTime.Now;
            _context.Entry(ticket).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                if (!TicketExists(ticket.TicketId))
                    return new NotFoundResult();
                else
                {
                    _logger.LogError($"[{dbEx.Source}] {dbEx.Message}");
                    _logger.LogError(dbEx.StackTrace);
                    return new StatusCodeResult(500);
                }
            }
            return new OkObjectResult(new ResponseModel(true, ticket.TicketId.ToString()));
        }

        public async Task<IActionResult> DeleteTicketAsync(TicketModel ticket)
        {
            _context.Tickets.Remove(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                if (!TicketExists(ticket.TicketId))
                    return new NotFoundResult();
                else
                {
                    _logger.LogError($"[{dbEx.Source}] {dbEx.Message}");
                    _logger.LogError(dbEx.StackTrace);
                    return new StatusCodeResult(500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{ex.Source}] {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return new StatusCodeResult(500);
            }

            return new NoContentResult();
        }

        private bool TicketExists(int id)
            => _context.Tickets.Any(t => t.TicketId == id);
    }
}
