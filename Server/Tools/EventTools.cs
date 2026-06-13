using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve offer events")]
public class EventTools(HttpClient client, ITokenProvider provider) : SaleTools(client, provider)
{
    [McpServerTool]
    [Description("Gets a list of offer events")]
    public Task<string?> GetOfferEvents(
        [Description("The maximum number of offer events that will be returned in the response")] int limit = 10,
        [Description("The ID of the last seen event. Events that occured after the given event will be returned")] string? from = null)
    {
        var filter = string.IsNullOrWhiteSpace(from) ? string.Empty : $"&from={Uri.EscapeDataString(from)}";
        return GetAsync($"{Endpoint}/offer-events?limit={limit}{filter}");
    }
}
