using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;
using System.Globalization;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about orders")]
public class OrderTools(HttpClient client, ITokenProvider provider) : ToolsBase(client, provider)
{
    private static readonly string Endpoint = $"{BaseEndpoint}/order";

    [McpServerTool]
    [Description("Gets a list of new orders")]
    public Task<string?> GetNewOrders()
        => GetAsync($"{Endpoint}/checkout-forms?status=READY_FOR_PROCESSING&fulfillment.status=NEW");

    [McpServerTool]
    [Description("Gets a list of all unprocessed returned items that are dispatched, in-transit or delivered")]
    public Task<string?> GetReturns(
        [Description("Number of days to look back")] int days = 90)
        => GetAsync($"{Endpoint}/customer-returns?status=DISPATCHED&status=IN_TRANSIT&status=DELIVERED{GetFilterByDays(days)}");

    [McpServerTool]
    [Description("Gets a list of just created returned items")]
    public Task<string?> GetCreatedReturns(
        [Description("Number of days to look back")] int days = 90)
        => GetAsync($"{Endpoint}/customer-returns?status=CREATED{GetFilterByDays(days)}");

    [McpServerTool]
    [Description("Gets a list of in-transit returned items")]
    public Task<string?> GetInTransitReturns(
        [Description("Number of days to look back")] int days = 90)
        => GetAsync($"{Endpoint}/customer-returns?status=IN_TRANSIT{GetFilterByDays(days)}");

    [McpServerTool]
    [Description("Gets a list of just delivered returned items")]
    public Task<string?> GetDeliveredReturns(
        [Description("Number of days to look back")] int days = 90)
        => GetAsync($"{Endpoint}/customer-returns?status=DELIVERED{GetFilterByDays(days)}");

    // ISO 8601 Format: ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    private static string ToISO8601Format(DateTime dateTime) => dateTime.ToString("o", CultureInfo.InvariantCulture);
    private static string GetFilterByDays(int days)
    {
        if (days <= 0)
            return string.Empty;

        return $"&createdAt.gte={ToISO8601Format(DateTime.UtcNow.Date.AddDays(-days))}";
    }
}
