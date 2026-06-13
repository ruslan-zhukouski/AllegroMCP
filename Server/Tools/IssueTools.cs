using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about disputes and claims")]
public class IssueTools(HttpClient client) : SaleTools(client)
{
    [McpServerTool]
    [Description("Gets the list of all disputes and claims ordered by descending opened date")]
    public Task<string?> GetIssues(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues");

    [McpServerTool]
    [Description("Gets a single dispute or claim")]
    public Task<string?> GetIssue(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("Dispute or claim identifier")] string issueId)
        => GetAsync(accessToken, $"{Endpoint}/issues/{Uri.EscapeDataString(issueId)}");

    [McpServerTool]
    [Description("Gets the list of closed disputes ordered by descending opened date")]
    public Task<string?> GetClosedDisputes(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=DISPUTE_CLOSED");

    [McpServerTool]
    [Description("Gets the list of ongoing disputes ordered by descending opened date")]
    public Task<string?> GetOngoingDisputes(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=DISPUTE_ONGOING");

    [McpServerTool]
    [Description("Gets the list of unresolved disputes ordered by descending opened date")]
    public Task<string?> GetUnresolvedDisputes(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=DISPUTE_UNRESOLVED");

    [McpServerTool]
    [Description("Gets the list of submitted claims ordered by descending opened date")]
    public Task<string?> GetSubmittedClaims(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=CLAIM_SUBMITTED");

    [McpServerTool]
    [Description("Gets the list of accepted claims ordered by descending opened date")]
    public Task<string?> GetAcceptedClaims(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=CLAIM_ACCEPTED");

    [McpServerTool]
    [Description("Gets the list of rejected claims ordered by descending opened date")]
    public Task<string?> GetRejectedClaims(
        [Description("Access token. Can be obtained from a file")] string accessToken,
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync(accessToken, $"{Endpoint}/issues?status=CLAIM_REJECTED");
}
