using System.Net.Http.Headers;
using System.Text.Json;
using Duende.AccessTokenManagement.OpenIdConnect;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [Authorize]
    public class CallApiModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        public string Json = string.Empty;

        // public async Task OnGet()
        // {
        //     var accessToken = await HttpContext.GetTokenAsync("access_token");
        //     var client = new HttpClient();
        //     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //     var content = await client.GetStringAsync("https://localhost:6001/identity");

        //     var parsed = JsonDocument.Parse(content);
        //     var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

        //     Json = formatted;
        // }

        // public async Task OnGet()
        // {
        //     var tokenInfo = await HttpContext.GetUserAccessTokenAsync();
        //     var client = new HttpClient();
        //     client.SetBearerToken(tokenInfo.Token!.AccessToken!);

        //     var content = await client.GetStringAsync("https://localhost:6001/identity");

        //     var parsed = JsonDocument.Parse(content);
        //     var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

        //     Json = formatted;
        // }

        public async Task OnGet()
        {
            var client = httpClientFactory.CreateClient("apiClient");

            var content = await client.GetStringAsync("https://localhost:6001/identity");

            var parsed = JsonDocument.Parse(content);
            var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

            Json = formatted;
        }


    }
}
