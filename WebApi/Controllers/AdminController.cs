using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Filters;
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

        // FIXA TILL IACTIONRESULT PÅ SIGNINASYNC??
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var response = await _identity.SignInAsync(model);
            return response.Succeeded
                ? Ok(response)
                : Unauthorized();
        }

        [VerifyToken]
        [HttpPost("signout")]
        new public async Task<IActionResult> SignOut()
        {
            // Då [Authorize] + [VerifyToken] redan verifierat token bör denna alltid lyckas
            HttpContext.Request.Headers.TryGetValue("Authorization", out var auth);
            var token = auth.ToString().Split(" ")[1];

            var result = await _identity.SignOutAsync(token);
            return result.Succeeded
                ? Ok(result)
                : Unauthorized();
        }

        [VerifyToken]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminViewModel>>> GetAdmins()
        {
            var admins = await _context.Administrators.ToListAsync();
            var viewModels = admins.Select(admin => new AdminViewModel(admin));
            return new OkObjectResult(viewModels);
        }

        [VerifyToken]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AdminViewModel>>> GetAdmin(int id)
        {
            var admin = await _context.Administrators.FindAsync(id);
            return admin == null
                ? NotFound()
                : Ok(new AdminViewModel(admin));
        }

        // [Authorize] validerar först token gentemot SecretKey (och expiry),
        // [VerifyToken] verifierar sedan att det stämmer gentemot databasen
        // - returnerar alltså bara Ok() om båda är ok
        [VerifyToken]
        [HttpPost("validate")]
        public ActionResult ValidateToken()
            => Ok();
    }
}
