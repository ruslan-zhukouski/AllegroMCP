using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about sales quality")]
public class QualityTools(HttpClient client) : SaleTools(client)
{
    [McpServerTool]
    [Description("Gets sales quality information for the shop")]
    public Task<string?> GetQuality(
        [Description("Access token. Can be obtained from a file")] string accessToken)
        => GetAsync(accessToken, $"{Endpoint}/quality");
}
