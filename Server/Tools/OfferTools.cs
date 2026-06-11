using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve offers data")]
public class OfferTools(IHttpClientFactory factory)
{
    private static readonly string OfferEndpoint = "https://api.allegro.pl/sale";

    [McpServerTool]
    [Description("Gets a list of seller's offers")]
    public Task<string?> GetOffers(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of offers returned in the response")] int limit = 10,
        [Description("The place to download the next portion of data from")] int offset = 0)
        => GetAsync(accessToken, $"offers?limit={limit}&offset={offset}");

    [McpServerTool]
    [Description("Gets all data of the particular offer")]
    public Task<string?> GetOffer(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId)
        => GetAsync(accessToken, $"product-offers/{Uri.EscapeDataString(offerId)}");

    private async Task<string?> GetAsync(string accessToken, string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{OfferEndpoint}/{uri}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new("application/vnd.allegro.public.v1+json"));
        request.Headers.Accept.Add(new("application/vnd.allegro.beta.v1+json"));
        request.Headers.UserAgent.Add(
            new ProductInfoHeaderValue("AllegroMCPServer", "1.0"));

        var response = await factory
            .CreateClient()
            .SendAsync(request);

        if (response?.IsSuccessStatusCode ?? false)
            return await response.Content.ReadAsStringAsync();

        return default;
    }
}
