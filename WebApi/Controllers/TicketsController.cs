using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(SqlDbContext context, ILogger<TicketsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Tickets
        // sort/order hämtas från querystring automatiskt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketModel>>> GetTickets(string sort = null, string order = null)
        {
            var tickets = await _context.Tickets.ToListAsync();
            if (string.IsNullOrEmpty(sort))
                return Ok(tickets);

            _logger.LogInformation($"Sort: {sort} -/- Order: {order}");

            order ??= "asc";

            switch (sort)
            {
                case "id":
                    tickets = tickets.OrderBy(t => t.TicketId.ToString()).ToList();
                    break;
                case "status":
                    tickets = tickets.OrderBy(t => t.Status).ToList();
                    break;
                default:
                    break;
            }

            return new OkObjectResult(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketModel>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return ticket == null 
                ? NotFound() 
                : Ok(ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketModel ticket)
        {
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
        public async Task<ActionResult<TicketModel>> PostTicket(TicketModel ticket)
        {
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
        public async Task<IActionResult> DeleteTicket(int id)
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

        private bool TicketModelExists(int id)
            => _context.Tickets.Any(e => e.TicketId == id);
    }
}
