using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibrary.Models;
using SharedLibrary.Models.Admin;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public class APIService : IAPIService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly string _signInUrl;
        private readonly string _signOutUrl;

        public string BaseUrl => "https://localhost:44330/api";
        public string TicketsUrl => $"{BaseUrl}/tickets";
        public string AdminsUrl => $"{BaseUrl}/admin";
        public string CustomersUrl => $"{BaseUrl}/customers";
        public string ValidateUrl => $"{AdminsUrl}/validate";
        public string RegisterUrl => $"{AdminsUrl}/register";
        public string StatusUrl => $"{TicketsUrl}/status";

        public APIService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;

            _signInUrl = $"{AdminsUrl}/signin";
            _signOutUrl = $"{AdminsUrl}/signout";
        }

        // Helper för "snyggare" sparning av token i LocalStorage
        public async Task SaveTokenAsync(string token)
            => await _localStorage.SetItemAsync("accessToken", token);

        // Helper för "snyggare" hämtning av token i LocalStorage
        public async Task<string> GetTokenAsync()
            => await _localStorage.GetItemAsStringAsync("accessToken");

        // Helper för "snyggare" hämtning av DisplayName i accessToken
        public async Task<string> GetDisplayName()
        {
            var _token = await GetTokenAsync();
            if (string.IsNullOrEmpty(_token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(_token);
            return jsonToken.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;
        }

        /* "Allomfattande" funktion för att skicka requests till API.
         * Hanterar exceptions och returnerar felmeddelande i form av HttpResponseMessage
         *   för felhantering där requesten görs
         * Använder EnsureSucessStatusCode för närmare kontroll av icke-ok requests */
        public async Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url,
            object serializeContent = null, bool auth = false)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                if (serializeContent != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(serializeContent), Encoding.UTF8, "application/json");

                if (auth)
                {
                    var _token = await GetTokenAsync();
                    if (string.IsNullOrEmpty(_token))
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return response;
            }
            // Triggas exempelvis om API-servern inte är igång
            catch (HttpRequestException ex)
            {
                var statusCode = ex.StatusCode ?? HttpStatusCode.ServiceUnavailable;
                var content = new StringContent(ex.Message);

                return new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                };
            }
            // Resterande exceptions är allra troligast pga bad request
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Unhandled exception. " + ex.Message)
                };
            }
        }

        // Helper för att logga in
        public async Task<HttpResponseMessage> SignIn(SignInModel model)
        {
            var response = await SendToAPIAsync(HttpMethod.Post, _signInUrl, model);
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
                await SaveTokenAsync(payload.Result);
            }
            return response;
        }

        // Helper för att logga ut
        public async Task SignOut()
        {
            await SendToAPIAsync(HttpMethod.Post, _signOutUrl, auth: true);
            await _localStorage.RemoveItemAsync("accessToken");
        }

        // Helper för "snyggare" villkorad varnings-loggning. Tar in ILogger<T> från anrop
        // så att filen, där felet loggas ifrån, inkluderas i varningsmeddelandet.
        public void LogWarningIfDebug(string message, ILogger logger)
        {
#if DEBUG
            logger.LogWarning(message);
#endif
        }
    }
}
