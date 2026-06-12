using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve offers data")]
public class OfferTools(HttpClient client) : ToolsBase(client)
{
    private static readonly string OfferEndpoint = $"{BaseEndpoint}/sale";

    [McpServerTool]
    [Description("Gets a list of seller's offers")]
    public Task<string?> GetOffers(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of offers returned in the response")] int limit = 10,
        [Description("The place to download the next portion of data from")] int offset = 0)
        => GetAsync(accessToken, $"{OfferEndpoint}/offers?limit={limit}&offset={offset}");

    [McpServerTool]
    [Description("Gets all data of the particular offer")]
    public Task<string?> GetOffer(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId)
        => GetAsync(accessToken, $"{OfferEndpoint}/product-offers/{Uri.EscapeDataString(offerId)}");

    [McpServerTool]
    [Description("Gets selected data for a specific offer")]
    public Task<string?> GetSelectedDataFromOffer(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId,
        [Description("Comma-separated list of fields to include")] string filter)
        => GetAsync(accessToken, $"{OfferEndpoint}/product-offers/{Uri.EscapeDataString(offerId)}/parts?include={Uri.EscapeDataString(filter)}");

    [McpServerTool]
    [Description("Gets stock and price information for a specific offer")]
    public Task<string?> GetStockAndPriceFromOffer(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId)
        => GetSelectedDataFromOffer(accessToken, offerId, "stock,price");

    [McpServerTool]
    [Description("Updates available stock for an offer")]
    public async Task<string?> UpdateStock(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId,
        [Description("New available stock quantity")] int available)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{OfferEndpoint}/product-offers/{Uri.EscapeDataString(offerId)}")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { stock = new { available } }),
                Encoding.UTF8, "application/vnd.allegro.public.v1+json")
        };

        return await SendAsync(request, accessToken);
    }

    [McpServerTool]
    [Description("Links an offer to a product")]
    public async Task<string?> UpdateUnderlyingProduct(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Offer identifier")] string offerId,
        [Description("New product identifier (GTIN/EAN)")] string ean)
    {
        var json = await GetOffer(accessToken, offerId);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(json, nameof(json));

        var doc = JsonNode.Parse(json) as JsonObject ??
            throw new InvalidOperationException("Failed to parse offer JSON.");

        string? images = null;
        if (doc.TryGetPropertyValue("images", out var imgs) && imgs is JsonArray imgsArr)
            images = imgsArr.ToString();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(images, nameof(images));

        string? description = null;
        if (doc.TryGetPropertyValue("description", out var desc))
            description = desc?.ToString();
        ArgumentNullException.ThrowIfNullOrWhiteSpace(description, nameof(description));

        var body = $$$"""
            {"productSet":[{"product":{"id":"{{{ean}}}","idType":"GTIN"}}],
            "images":{{{images}}}, "description":{{{description}}}}
            """;

        var request = new HttpRequestMessage(HttpMethod.Patch, $"{OfferEndpoint}/product-offers/{Uri.EscapeDataString(offerId)}")
        {
            Content = new StringContent(body,
                Encoding.UTF8, "application/vnd.allegro.public.v1+json")
        };

        return await SendAsync(request, accessToken);
    }
}
