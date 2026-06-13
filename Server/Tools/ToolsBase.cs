using Server.Services;
using System.Net.Http.Headers;

namespace Server.Tools;

public abstract class ToolsBase(HttpClient client, ITokenProvider provider)
{
    protected static readonly string BaseEndpoint = "https://api.allegro.pl";

    protected Task<string?> GetAsync(string uri)
        => SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));

    protected async Task<string?> SendAsync(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", provider.GetAccessToken());
        request.Headers.Accept.Add(new("application/vnd.allegro.public.v1+json"));
        request.Headers.Accept.Add(new("application/vnd.allegro.beta.v1+json"));
        request.Headers.UserAgent.Add(
            new ProductInfoHeaderValue("AllegroMCPServer", "1.0"));

        var response = await client.SendAsync(request);

        if (response?.IsSuccessStatusCode ?? false)
            return await response.Content.ReadAsStringAsync();

        return default;
    }
}
