using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.Customer;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public CustomersController(SqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerModel(Guid id)
        {
            var customerModel = await _context.Customers.FindAsync(id);

            if (customerModel == null)
            {
                return NotFound();
            }

            return customerModel;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerModel(Guid id, CustomerModel customerModel)
        {
            if (id != customerModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerModel>> PostCustomerModel(CustomerModel customerModel)
        {
            _context.Customers.Add(customerModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerModel", new { id = customerModel.Id }, customerModel);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerModel(Guid id)
        {
            var customerModel = await _context.Customers.FindAsync(id);
            if (customerModel == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerModelExists(Guid id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
