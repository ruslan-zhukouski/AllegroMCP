using ModelContextProtocol.Server;
using Server.Services;
using System.ComponentModel;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about disputes and claims")]
public class IssueTools(HttpClient client, ITokenProvider provider) : SaleTools(client, provider)
{
    [McpServerTool]
    [Description("Gets the list of all disputes and claims ordered by descending opened date")]
    public Task<string?> GetIssues(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues");

    [McpServerTool]
    [Description("Gets a single dispute or claim")]
    public Task<string?> GetIssue(
        [Description("Dispute or claim identifier")] string issueId)
        => GetAsync($"{Endpoint}/issues/{Uri.EscapeDataString(issueId)}");

    [McpServerTool]
    [Description("Gets the list of closed disputes ordered by descending opened date")]
    public Task<string?> GetClosedDisputes(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=DISPUTE_CLOSED");

    [McpServerTool]
    [Description("Gets the list of ongoing disputes ordered by descending opened date")]
    public Task<string?> GetOngoingDisputes(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=DISPUTE_ONGOING");

    [McpServerTool]
    [Description("Gets the list of unresolved disputes ordered by descending opened date")]
    public Task<string?> GetUnresolvedDisputes(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=DISPUTE_UNRESOLVED");

    [McpServerTool]
    [Description("Gets the list of submitted claims ordered by descending opened date")]
    public Task<string?> GetSubmittedClaims(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=CLAIM_SUBMITTED");

    [McpServerTool]
    [Description("Gets the list of accepted claims ordered by descending opened date")]
    public Task<string?> GetAcceptedClaims(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=CLAIM_ACCEPTED");

    [McpServerTool]
    [Description("Gets the list of rejected claims ordered by descending opened date")]
    public Task<string?> GetRejectedClaims(
        [Description("The maximum number of issues in a response")] int limit = 10,
        [Description("Index of first returned issue")] int offset = 0)
        => GetAsync($"{Endpoint}/issues?status=CLAIM_REJECTED");
}
