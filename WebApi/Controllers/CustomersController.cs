using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using SharedLibrary.Models.Customer;
using WebApi.Data;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Authorize]
    [VerifyToken]
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
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            var customerViewModels = customers.Select(c => new CustomerViewModel(c));
            return Ok(customerViewModels);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer == null
                ? NotFound()
                : new CustomerViewModel(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerModel customerModel)
        {
            if (id != customerModel.CustomerId)
                return BadRequest();

            _context.Entry(customerModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerViewModel>> PostCustomer(CustomerViewModel model)
        {
            var customer = new CustomerModel(model);
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                // Location header = api/customers/{customerId} + en payload med CustomerId som Result
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId },
                    new ResponseModel(true, customer.CustomerId.ToString()));
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                    return Conflict();
                else
                    throw;
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.CustomerId == id);
        }
    }
}
