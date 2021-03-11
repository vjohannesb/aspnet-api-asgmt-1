using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SharedLibrary.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly SqlDbContext _context;

        private readonly IIdentityService _identity;

        public AdminController(SqlDbContext context, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
            => await _identity.CreateAdminAsync(model);

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var response = await _identity.SignInAsync(model);

            if (response.Succeeded)
                return new OkObjectResult(response);

            return Unauthorized();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminViewModel>>> GetAdmins()
        {
            var admins = await _context.Administrators.ToListAsync();
            var viewModels = admins.Select(admin => new AdminViewModel(admin));
            return new OkObjectResult(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AdminViewModel>>> GetAdmin(Guid id)
        {
            var admin = await _context.Administrators.FindAsync(id);
            return new OkObjectResult(new AdminViewModel(admin));
        }

        // Då AdminController kräver auth returnerar denna funktion bara Ok om giltig token finns med i requesten
        [HttpPost("validate")]
        public IActionResult ValidateToken()
            => Ok();
    }
}
