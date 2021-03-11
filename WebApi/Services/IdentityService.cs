using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Models.Admin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data;

namespace WebApi.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SqlDbContext _context;

        private readonly IConfiguration _configuration;

        public IdentityService(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> CreateAdminAsync(RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.LastName)  ||
                string.IsNullOrEmpty(model.Password)  ||
                string.IsNullOrEmpty(model.Email))
                return new BadRequestObjectResult(
                    new { status = 400, title = "Bad Request", 
                        Message = "First name, last name, email and password are required." });

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
                // Om id osannolikt nog redan finns registrerat
                if (AdminExists(admin.Id))
                    return new ConflictObjectResult(new { id = admin.Id });
                else
                    throw;
            }

            // Returnera bara id & email (ej lösen), något extra säkert
            return new CreatedAtActionResult(
                "GetAdmin", 
                "Admin",
                new { id = admin.Id },
                new { id = admin.Id, email = admin.Email }
                );

        }

        public async Task<ResponseModel> SignInAsync(SignInModel model)
        {
            if (!EmailRegistered(model.Email))
                return new ResponseModel(false, null);

            var admin = await _context.Administrators.FirstOrDefaultAsync(admin => admin.Email == model.Email);

            if (!admin.ValidatePasswordHash(model.Password))
                return new ResponseModel(false, null);

            var tokenHandler = new JwtSecurityTokenHandler();
            var _secretKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim("UserId", admin.Id.ToString()) }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha512Signature)
            };

            //var _accessToken = new SessionToken {
            //    Id = admin.Id,
            //    AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor))
            //};

            var _accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            //if (AdminTokenIssued(admin.Id))
            //    _context.SessionTokens.Update(_accessToken);
            //else
            //    _context.SessionTokens.Add(_accessToken);

            return new ResponseModel(true, _accessToken);
        }

        //private bool AdminTokenIssued(string id)
        //    => _context.SessionTokens.Any(st => st.Id == id);

        private bool EmailRegistered(string email)
            => _context.Administrators.Any(a => a.Email == email);

        private bool AdminExists(Guid id)
            => _context.Administrators.Any(a => a.Id == id);
    }
}
