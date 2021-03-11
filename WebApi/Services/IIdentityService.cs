using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.Admin;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IIdentityService
    {
        public Task<IActionResult> CreateAdminAsync(RegisterModel model);

        public Task<ResponseModel> SignInAsync(SignInModel model);
    }
}
