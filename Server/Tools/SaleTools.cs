namespace Server.Tools;

public abstract class SaleTools(HttpClient client) : ToolsBase(client)
{
    protected static readonly string Endpoint = $"{BaseEndpoint}/sale";
}
