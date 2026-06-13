using Server.Services;

namespace Server.Tools;

public abstract class SaleTools(HttpClient client, ITokenProvider provider) : ToolsBase(client, provider)
{
    protected static readonly string Endpoint = $"{BaseEndpoint}/sale";
}
