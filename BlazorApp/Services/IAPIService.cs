using Microsoft.Extensions.Logging;
using SharedLibrary.Models.Admin;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IAPIService
    {
        // Properties (samla alla URL:s på ett ställe)
        public string BaseUrl { get; }
        public string TicketsUrl { get; }
        public string AdminsUrl { get; }
        public string CustomersUrl { get; }
        public string ValidateUrl { get; }
        public string RegisterUrl { get; }
        public string StatusUrl { get; }

        // Token
        public Task SaveTokenAsync(string token);
        public Task<string> GetTokenAsync();
        public Task<string> GetDisplayName();

        // Identity
        public Task<HttpResponseMessage> SignIn(SignInModel model);
        public Task SignOut();

        // API
        public Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url, object serializeContent = null, bool auth = false);
        public void LogWarningIfDebug(string message, ILogger logger);

    }
}
