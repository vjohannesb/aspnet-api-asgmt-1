using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Ticket;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public TicketsController(SqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketRequestModel>>> GetTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();
            var requestModels = tickets.Select(ticket => new TicketRequestModel(ticket)); 
            return new OkObjectResult(requestModels);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketModel>> GetTicket(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound();

            var requestModel = new TicketRequestModel(ticket);

            return new OkObjectResult(requestModel);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(Guid id, TicketRequestModel model)
        {
            var ticket = new TicketModel(model);
            ticket.DateUpdated = DateTime.UtcNow;

            if (id != ticket.TicketId)
                return BadRequest();

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketModelExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketModel>> PostTicket(TicketRequestModel model)
        {
            var ticket = new TicketModel(model);
            ticket.DateCreated = DateTime.UtcNow;
            ticket.DateUpdated = DateTime.UtcNow;

            _context.Tickets.Add(ticket);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TicketModelExists(ticket.TicketId))
                    return Conflict();
                else
                    throw;
            }

            // Location header = api/tickets/{ticketId} + en payload med TicketId som Result
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, 
                new ResponseModel(true, ticket.TicketId.ToString()));
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var ticketModel = await _context.Tickets.FindAsync(id);
            if (ticketModel == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticketModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketModelExists(Guid id)
            => _context.Tickets.Any(e => e.TicketId == id);
    }
}
