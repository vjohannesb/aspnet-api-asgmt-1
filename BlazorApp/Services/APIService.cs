using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Customer;
using SharedLibrary.Models.Ticket;
using SharedLibrary.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorApp.Services
{
    public class APIService : IAPIService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private string _token;
        private readonly string _signInUrl = "https://localhost:44330/api/admin/signin";

        public APIService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // Helper för "snyggare" sparning av token i LocalStorage
        public async Task SaveTokenAsync(string token)
            => await _localStorage.SetItemAsync("accessToken", token);

        // Helper för "snyggare" hämtning av token i LocalStorage
        // (GetItemAsStringAsync returnerar null om "accessToken" inte finns)
        public async Task<string> GetTokenAsync() 
            => (string.IsNullOrEmpty(_token) || string.IsNullOrWhiteSpace(_token)) 
                ? await _localStorage.GetItemAsStringAsync("accessToken")
                : _token;

        // Helper för "snyggare" hämtning av DisplayName i accessToken
        public async Task<string> GetDisplayName()
        {
            _token = await GetTokenAsync();
            if (!string.IsNullOrEmpty(_token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(_token);
                return jsonToken.Claims.FirstOrDefault(claim => claim.Type == "DisplayName")?.Value;
            }
            return null;
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
                    _token = await GetTokenAsync();
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

                // GET Request istället för POST, exempelvis (mest för debugging)
                if (ex.Message.Contains("GET Request cannot have a body."))
                    statusCode = HttpStatusCode.BadRequest;

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
            _token = null;
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
