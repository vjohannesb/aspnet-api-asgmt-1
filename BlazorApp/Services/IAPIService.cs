using Microsoft.Extensions.Logging;
using SharedLibrary.Models;
using SharedLibrary.Models.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IAPIService
    {
        public Task SignOut();

        public Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url, object serializeContent = null, bool auth = false);

        public Task<HttpResponseMessage> SignIn(SignInModel model);
        //public Task<HttpStatusCode> ValidateTokenAsync();

        public Task<string> GetTokenAsync();

        public Task SaveTokenAsync(string token);

        public void LogWarningIfDebug(string message, ILogger logger);

        public Task<string> GetDisplayName();

        //public Task<T> Get<T>(string url);

        //public Task<ResponseModel> Post(object serializeContent, string url, bool auth);
    }
}
