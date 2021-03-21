using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.Models.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IIdentityService
    {
        public Task<IActionResult> CreateAdminAsync(RegisterModel model);

        public Task<AdminModel> GetAdminAsync(int id);

        public Task<IEnumerable<AdminModel>> GetAdminsAsync();

        public Task<ResponseModel> SignInAsync(SignInModel model);

        public Task<ResponseModel> SignOutAsync(string token);
    }
}
