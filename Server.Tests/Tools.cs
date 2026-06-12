using Server.Tools;

namespace Server.Tests;

public class Tools
{
    readonly HttpClient client = new();

    [Fact]
    public async Task Test()
    {
        var offerTools = new OfferTools(client);
        var orderTools = new OrderTools(client);
    }
}
