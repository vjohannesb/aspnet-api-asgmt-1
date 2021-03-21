using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.Ticket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [VerifyToken]
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TicketsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        // GET: api/Tickets
        // sort/order hämtas från querystring
        [HttpGet]
        public async Task<IActionResult> GetTickets(string sort = null, string order = null)
        {
            var tickets = await _dbService.GetTicketsAsync();
            return _dbService.SortTickets(tickets, sort, order);
        }

        // GET: api/Tickets/status
        // För att lättare hämta hur många av varje ticket det finns (visas på startsidan)
        [HttpGet("status")]
        public async Task<ActionResult<Dictionary<string, int>>> GetTicketStatus()
        {
            var tickets = await _dbService.GetTicketsAsync();
            return new Dictionary<string, int>
            {
                { "Open", tickets.Where(t => t.Status == TicketStatus.Open).Count() },
                { "Active", tickets.Where(t => t.Status == TicketStatus.Active).Count() },
                { "Closed", tickets.Where(t => t.Status == TicketStatus.Closed).Count() }
            };
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var ticket = await _dbService.GetTicketAsync(id);
            return ticket == null
                ? NotFound()
                : Ok(new TicketViewModel(ticket));
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketViewModel model)
        {
            if (id != model.TicketId)
                return new BadRequestResult();

            var ticket = new TicketModel(model);
            return await _dbService.UpdateTicketAsync(ticket);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> PostTicket(TicketViewModel model)
        {
            var ticket = new TicketModel(model);
            return await _dbService.CreateTicketAsync(ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _dbService.GetTicketAsync(id);
            if (ticket == null)
                return NotFound();

            return await _dbService.DeleteTicketAsync(ticket);
        }
    }
}
