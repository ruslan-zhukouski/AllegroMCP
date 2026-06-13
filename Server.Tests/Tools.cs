using Server.Services;

namespace Server.Tests;

public class Tools
{
    readonly FileTokenProvider provider = new(PathToTokensFile);

    private static readonly string PathToTokensFile = "../../../../Host/bin/debug/net10.0/allegro_tokens.txt";

    [Fact]
    public void Is_AccessToken_Valid()
    {
        var accessToken = provider.GetAccessToken();
        Assert.NotNull(accessToken);
    }
}
