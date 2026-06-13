using Server.Tools;

namespace Server.Tests;

public class Tools
{
    readonly HttpClient client = new();

    private static readonly string PathToTokensFile = "../../../../Host/bin/debug/net10.0/allegro_tokens.txt";

    [Fact]
    public void Is_AccessToken_Valid()
    {
        var accessToken = AuthorizationTools.GetAccessToken(PathToTokensFile);
        Assert.NotNull(accessToken);
    }
}
