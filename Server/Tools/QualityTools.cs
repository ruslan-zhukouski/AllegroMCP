using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about sales quality")]
public class QualityTools(HttpClient client, ITokenProvider provider) : SaleTools(client, provider)
{
    [McpServerTool]
    [Description("Gets sales quality information for the shop")]
    public Task<string?> GetQuality()
        => GetAsync($"{Endpoint}/quality");
}
