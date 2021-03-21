using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Models;
using SharedLibrary.Models.Admin;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SqlDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly JwtSecurityTokenHandler _tokenHandler;

        public IdentityService(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<IActionResult> CreateAdminAsync(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.Email))
            {
                return new BadRequestObjectResult(
                    new
                    {
                        status = 400,
                        title = "Bad Request",
                        Message = "First name, last name, email and password are required."
                    });
            }

            // BadRequest istället för Conflict för att dölja registrerade e-postadresser
            if (EmailRegistered(model.Email))
                return new BadRequestResult();

            var admin = new AdminModel(model);

            admin.CreatePasswordWithHash(model.Password);
            _context.Administrators.Add(admin);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdminExists(admin.AdminId))
                    return new ConflictObjectResult(new { id = admin.AdminId });
                else
                    throw;
            }

            // Returnera ej lösen, något extra säkert
            return new CreatedAtActionResult(
                "GetAdmin",
                "Admin",
                new { id = admin.AdminId },
                new { id = admin.AdminId, email = admin.Email }
                );

        }

        public async Task<ResponseModel> SignInAsync(SignInModel model)
        {
            if (!EmailRegistered(model.Email))
                return new ResponseModel(false, null);

            var admin = await _context.Administrators.FirstOrDefaultAsync(admin => admin.Email == model.Email);

            if (!admin.ValidatePasswordHash(model.Password))
                return new ResponseModel(false, null);

            var _secretKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", admin.AdminId.ToString()),
                        new Claim("DisplayName", $"{admin.FirstName} {admin.LastName}")
                    }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var _accessToken = _tokenHandler.WriteToken(_tokenHandler.CreateToken(tokenDescriptor));
            admin.CreateTokenWithHash(_accessToken);

            try
            {
                _context.Update(admin);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled error. {ex.Message}\n{ex}");
            }

            return new ResponseModel(true, _accessToken);
        }

        public async Task<ResponseModel> SignOutAsync(string token)
        {
            var id = GetUserIdFromToken(token);
            var admin = await _context.Administrators.FindAsync(id);
            if (admin == null)
                return new ResponseModel(false, null);

            admin.Token = null;
            try
            {
                _context.Update(admin);
                await _context.SaveChangesAsync();
                return new ResponseModel(true, id.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseModel(false, ex.ToString());
            }
        }

        private bool EmailRegistered(string email)
            => _context.Administrators.Any(a => a.Email == email);

        private bool AdminExists(int id)
            => _context.Administrators.Any(a => a.AdminId == id);

        public int GetUserIdFromToken(string token)
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var tokenId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            var parsed = int.TryParse(tokenId, out int id);

            return parsed ? id : 0;
        }
    }
}
