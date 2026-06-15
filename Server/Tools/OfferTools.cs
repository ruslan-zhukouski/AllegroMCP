using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve offers data")]
public class OfferTools(HttpClient client, ITokenProvider provider) : SaleTools(client, provider)
{
    [McpServerTool]
    [Description("Gets a list of seller's offers")]
    public Task<string?> GetOffers(
        [Description("The maximum number of offers returned in the response")] int limit = 10,
        [Description("The place to download the next portion of data from")] int offset = 0)
        => GetAsync($"{Endpoint}/offers?limit={limit}&offset={offset}");

    [McpServerTool]
    [Description("Gets all data of the particular offer")]
    public Task<string?> GetOffer(
        [Description("Offer identifier")] string offerId)
        => GetAsync($"{Endpoint}/product-offers/{Uri.EscapeDataString(offerId)}");

    [McpServerTool]
    [Description("Gets selected data for a specific offer")]
    public Task<string?> GetSelectedDataFromOffer(
        [Description("Offer identifier")] string offerId,
        [Description("Comma-separated list of fields to include. Available options are stock and price")] string filter)
        => GetAsync($"{Endpoint}/product-offers/{Uri.EscapeDataString(offerId)}/parts?include={Uri.EscapeDataString(filter)}");

    [McpServerTool]
    [Description("Gets stock and price information for a specific offer")]
    public Task<string?> GetStockAndPriceFromOffer(
        [Description("Offer identifier")] string offerId)
        => GetSelectedDataFromOffer(offerId, "stock,price");

    [McpServerTool]
    [Description("Updates available stock for an offer")]
    public Task<string?> UpdateStock(
        [Description("Offer identifier")] string offerId,
        [Description("New available stock quantity")] int available)
        => UpdateOffer(offerId, JsonSerializer.Serialize(new { stock = new { available } }));

    [McpServerTool]
    [Description("Updates price of an offer")]
    public Task<string?> UpdatePrice(
        [Description("Offer identifier")] string offerId,
        [Description("New price")] decimal price)
        => UpdateOffer(offerId, JsonSerializer.Serialize(new { sellingMode = new { price = new { amount = price } } }));

    [McpServerTool]
    [Description("Links an offer to a product")]
    public async Task<string?> UpdateUnderlyingProduct(
        [Description("Offer identifier")] string offerId,
        [Description("New product identifier (GTIN/EAN)")] string ean)
    {
        var json = await GetOffer(offerId);
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

        return await UpdateOffer(offerId, body);
    }

    [McpServerTool]
    [Description("Starts or activates an offer")]
    public Task<string?> StartOffer(
        [Description("Offer identifier")] string offerId)
        => UpdateOffer(offerId, JsonSerializer.Serialize(new { publication = new { status = "ACTIVE" } }));

    [McpServerTool]
    [Description("Stops or ends an offer")]
    public Task<string?> StopOffer(
        [Description("Offer identifier")] string offerId)
        => UpdateOffer(offerId, JsonSerializer.Serialize(new { publication = new { status = "ENDED" } }));

    private Task<string?> UpdateOffer(string offerId, string body)
        => SendAsync(
            new HttpRequestMessage(HttpMethod.Patch, $"{Endpoint}/product-offers/{Uri.EscapeDataString(offerId)}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/vnd.allegro.public.v1+json")
            });
}
