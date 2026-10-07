using System.Diagnostics;
using System.Text.Json;
using Duende.IdentityModel.Client;

namespace Client;

public class Worker(IHostApplicationLifetime hostApplicationLifetime,IHttpClientFactory httpClientFactory) : BackgroundService
{
    public const string ActivitySourceName = "IdentityClient";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity(
            "Get Discovery Document", ActivityKind.Client);
        try
        {
            // 1. Resolve the HttpClient configured with Aspire Service Defaults
            var client = httpClientFactory.CreateClient("IdentityClient");

            // 2. Configure the policy to handle Aspire's routing resolution
            var policy = new DiscoveryPolicy
            {
                // Turn off strict endpoint validation so local port mapping doesn't break
                ValidateEndpoints = false, 
                
                // Explicitly allow loopback/localhost connections if needed
                RequireHttps = true 
            };

            // 3. Request the document passing the policy
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "https://localhost:5001", // Your Aspire logical service name
                Policy = policy
            });

            if (disco.IsError) 
            {
                throw new Exception(disco.Error);
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.WriteLine(tokenResponse.ErrorDescription);
            }

            Console.WriteLine(tokenResponse.AccessToken);

            // call api
            var apiClient = httpClientFactory.CreateClient("apiClient");
            apiClient.SetBearerToken(tokenResponse.AccessToken!); // AccessToken is always non-null when IsError is false

            var response = await apiClient.GetAsync("https://api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));


            // var disco = await client.GetDiscoveryDocumentAsync("https+http://localhost:5001");
            // if (disco.IsError)
            // {
            //     Console.WriteLine(disco.Error);
            //     Console.WriteLine(disco.Exception);
            // }
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }


        hostApplicationLifetime.StopApplication();
    }
}
