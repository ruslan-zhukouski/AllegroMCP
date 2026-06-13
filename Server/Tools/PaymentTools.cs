using ModelContextProtocol.Server;
using Server.Models;
using Server.Services;
using System.ComponentModel;
using System.Text.Json;

namespace Server.Tools;

[McpServerToolType]
[Description("Provides methods to retrieve information about payments")]
public class PaymentTools(HttpClient client, ITokenProvider provider) : ToolsBase(client, provider)
{
    private static readonly string Endpoint = $"{BaseEndpoint}/payments";

    [McpServerTool]
    [Description("Checks the balance of the user")]
    public async Task<string?> GetBalance()
    {
        return JsonSerializer.Serialize(new
        {
            AllegroFinance = await GetBalanceForOperator("AF"),
            Przelewy24AF = await GetBalanceForOperator("AF_P24"),
            PayUAF = await GetBalanceForOperator("AF_PAYU"),
            Przelewy24 = await GetBalanceForOperator("P24"),
            PayU = await GetBalanceForOperator("PAYU")
        });
    }

    private async Task<decimal> GetBalanceForOperator(string op)
    {
        var json = await GetAsync($"{Endpoint}/payment-operations?limit=1&wallet.paymentOperator={op}");
        ArgumentNullException.ThrowIfNullOrEmpty(json, nameof(json));

        var response = JsonSerializer.Deserialize<GetPaymentOperationsResponse>(json,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        if ((response?.PaymentOperations ?? []).Count == 1)
        {
            var balance = response!.PaymentOperations![0].Wallet?.Balance;

            if (balance is not null)
            {
                if (decimal.TryParse(balance.Amount, out var amount))
                    return amount;
            }
        }
        return 0;
    }
}
