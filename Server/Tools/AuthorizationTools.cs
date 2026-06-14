using ModelContextProtocol.Server;
using Server.Models;
using Server.Services;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Server.Tools;

[McpServerToolType]
[Description("Implements OAuth2 Device Flow for Allegro. To integrate with your app, you'll receive a user code and a device code. The user enters the user code you provided on a dedicated page (verification uri). If you want this link to be clickable (e.g., you want to send it by email or present it as a QR code), use verification_uri_complete. The user then consents to the app accessing their data and making changes on their behalf (if they haven't previously given their consent). At this time, your app queries a dedicated endpoint using the device code (device_code) to receive, among other things, an access token, which you can use to invoke REST API resources on the user's behalf")]
public class AuthorizationTools(IHttpClientFactory factory, ITokenProvider provider)
{
    // Read client credentials from environment variables to avoid embedding secrets in source
    private static readonly string ClientId = Environment.GetEnvironmentVariable("ALLEGRO_CLIENT_ID")
        ?? throw new InvalidOperationException("ALLEGRO_CLIENT_ID not configured");
    private static readonly string ClientSecret = Environment.GetEnvironmentVariable("ALLEGRO_CLIENT_SECRET")
        ?? throw new InvalidOperationException("ALLEGRO_CLIENT_SECRET not configured");

    private static readonly string DeviceAuthorizationEndpoint = "https://allegro.pl/auth/oauth";

    [McpServerTool]
    [Description("Gets user and device codes and verification uri the user should visit to provide user code")]
    public Task<string?> GetUserAndDeviceCodes()
        => PostAuthorization($"device?client_id={Uri.EscapeDataString(ClientId)}", string.Empty);

    [McpServerTool]
    [Description("Generates access and refresh tokens")]
    public async Task<string> GenerateTokens(
        [Description("The device code received from the device authorization endpoint")] string deviceCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceCode, nameof(deviceCode));

        var answer = await PostAuthorization("token",
            $"grant_type=urn:ietf:params:oauth:grant-type:device_code&device_code={Uri.EscapeDataString(deviceCode)}");

        ArgumentNullException.ThrowIfNullOrWhiteSpace(answer, nameof(answer));

        var tokens = JsonSerializer.Deserialize<Tokens>(answer);

        ArgumentNullException.ThrowIfNull(tokens, nameof(tokens));

        tokens.ExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(tokens.ExpiresIn);

        return provider.Save(tokens);
    }

    [McpServerTool]
    [Description("Refreshes a pair of access and refresh tokens using refresh token which can be obtained from a file")]
    public async Task<string> RefreshTokens(
        [Description("The refresh token received from the token endpoint")] string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken, nameof(refreshToken));

        var answer = await PostAuthorization("token",
            $"grant_type=refresh_token&refresh_token={Uri.EscapeDataString(refreshToken)}");

        ArgumentNullException.ThrowIfNullOrWhiteSpace(answer, nameof(answer));

        var tokens = JsonSerializer.Deserialize<Tokens>(answer);

        ArgumentNullException.ThrowIfNull(tokens, nameof(tokens));

        tokens.ExpiresOn = DateTime.UtcNow + TimeSpan.FromSeconds(tokens.ExpiresIn);

        return provider.Save(tokens);
    }

    private async Task<string?> PostAuthorization(string uri, string body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{DeviceAuthorizationEndpoint}/{uri}")
        {
            Content = new StringContent(
                body, Encoding.UTF8, "application/x-www-form-urlencoded"),
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":" + ClientSecret)));

        var response = await factory
            .CreateClient()
            .SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
