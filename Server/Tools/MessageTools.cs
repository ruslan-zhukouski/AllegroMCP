using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve and write messages")]
public class MessageTools(HttpClient client, ITokenProvider provider) : ToolsBase(client, provider)
{
    private static readonly string Endpoint = $"{BaseEndpoint}/messaging/threads";

    [McpServerTool]
    [Description("Gets the list of user threads sorted by last message date, starting from newest")]
    public Task<string?> GetThreads(
        [Description("The maximum number of threads returned in the response")] int limit = 10,
        [Description("Index of the first returned thread from all results")] int offset = 0)
        => GetAsync($"{Endpoint}?limit={limit}&offset={offset}");

    [McpServerTool]
    [Description("Gets user thread with provided identifier")]
    public Task<string?> GetThread(
        [Description("Thread identifier")] string threadId)
        => GetAsync($"{Endpoint}/{threadId}");
}
