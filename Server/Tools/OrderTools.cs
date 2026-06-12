using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about orders")]
public class OrderTools(HttpClient client) : ToolsBase(client)
{
    private static readonly string OrderEndpoint = $"{BaseEndpoint}/order";

    [McpServerTool]
    [Description("Gets a list of new orders")]
    public Task<string?> GetNewOrders(
        [Description("Access token. Can be obtained from a file")] string accessToken)
        => GetAsync(accessToken, $"{OrderEndpoint}/checkout-forms?status=READY_FOR_PROCESSING&fulfillment.status=NEW");
}
