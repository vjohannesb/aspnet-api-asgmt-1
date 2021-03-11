using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Services
{
    public class DbService : IDbService
    {
        private readonly SqlDbContext _context;

        public DbService(SqlDbContext context)
            => _context = context;

        /*** Customers ***/
        public async Task<IEnumerable<CustomerModel>> GetCustomersAsync()
            => await _context.Customers.ToListAsync();

        public async Task<CustomerModel> GetCustomerAsync(string Id)
            => await _context.Customers.FindAsync(Id);

        public async Task<IActionResult> CreateCustomerAsync(CustomerModel model)
        {
            if (EmailAlreadyRegistered(model.Email))
                return new ConflictObjectResult(new { Message = $"Customer email ({model.Email}) already registered." });

            _context.Customers.Add(model);

            try
            {
                await _context.SaveChangesAsync();
                return new CreatedAtActionResult("CreateCustomer", "Customers", new { id = model.Id }, model);
            } 
            catch (DbUpdateException)
            {
                if (CustomerExists(model.Id))
                    return new ConflictResult();
                else
                    // Log?
                    throw;
            }
        }

        public async Task<IActionResult> UpdateCustomerAsync(CustomerModel model)
        {
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(model.Id))
                    return new NotFoundObjectResult(model);
                else
                    throw;
            }

            return new NoContentResult();
        }

        private bool CustomerExists(Guid id)
            => _context.Customers.Any(c => c.Id == id);

        private bool EmailAlreadyRegistered(string email)
            => _context.Customers.Any(c => c.Email == email);





        /*** Tickets ***/
        public async Task<ActionResult<IEnumerable<TicketModel>>> GetTicketsAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();

            return new OkObjectResult(tickets);
        }

        public async Task<TicketModel> GetTicketAsync(string ticketId)
            => await _context.Tickets.FindAsync(ticketId);

        public async Task<IActionResult> CreateTicketAsync(TicketModel model)
        {
            model.DateCreated = DateTime.Now;
            model.DateUpdated = DateTime.Now;

            _context.Tickets.Add(model);

            try
            {
                await _context.SaveChangesAsync();
                return new CreatedAtActionResult("CreateTicket", "Tickets", new { id = model.TicketId}, model);
            }
            catch (DbUpdateException)
            {
                if (TicketExists(model.TicketId))
                    return new ConflictResult();
                else
                    // Log?
                    throw;
            }
        }

        public async Task<IActionResult> UpdateTicketAsync(TicketModel model)
        {
            model.DateUpdated = DateTime.Now;

            _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(model.TicketId))
                    return new NotFoundObjectResult(model);
                else
                    throw;
            }

            return new NoContentResult();
        }

        private bool TicketExists(Guid id)
            => _context.Tickets.Any(t => t.TicketId == id);
    }
}
