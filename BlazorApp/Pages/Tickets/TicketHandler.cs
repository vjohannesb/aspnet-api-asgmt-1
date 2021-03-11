using Newtonsoft.Json;
using SharedLibrary.Models.Admin;
using SharedLibrary.Models.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Pages.Tickets
{
    public static class TicketHandler
    {
        public static async Task<ResponseModel> SubmitTicketAsync(string token, string url, TicketRequestModel ticket)
        {
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.PostAsJsonAsync<TicketRequestModel>(url, ticket);
            if (response.IsSuccessStatusCode)
            {
                ticket = new TicketRequestModel();
                return new ResponseModel(true, $"/tickets/{response.Headers.Location.LocalPath.Split("/").LastOrDefault()}");
            }
            else
                return new ResponseModel(false, $"[{response.StatusCode}] {response.ReasonPhrase}");
        }
    }
}
