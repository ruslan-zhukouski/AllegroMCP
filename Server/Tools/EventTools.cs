using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve offer events")]
public class EventTools(HttpClient client) : SaleTools(client)
{
    [McpServerTool]
    [Description("Gets a list of offer events")]
    public Task<string?> GetOfferEvents(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of offer events that will be returned in the response")] int limit = 10,
        [Description("The ID of the last seen event. Events that occured after the given event will be returned")] string? from = null)
    {
        var filter = string.IsNullOrWhiteSpace(from) ? string.Empty : $"&from={Uri.EscapeDataString(from)}";
        return GetAsync(accessToken, $"{Endpoint}/offer-events?limit={limit}{filter}");
    }
}
