using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.Customer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [VerifyToken]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IDbService _dbService;

        public CustomersController(IDbService dbService)
        {
            _dbService = dbService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            var customers = await _dbService.GetCustomersAsync();
            var customerViewModels = customers.Select(c => new CustomerViewModel(c));
            return Ok(customerViewModels);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            var customer = await _dbService.GetCustomerAsync(id);
            return customer == null
                ? NotFound()
                : new CustomerViewModel(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerViewModel model)
        {
            var customer = new CustomerModel(model);
            return await _dbService.CreateCustomerAsync(customer);
        }
    }
}
