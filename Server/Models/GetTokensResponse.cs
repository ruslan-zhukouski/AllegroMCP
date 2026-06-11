using System.Text.Json.Serialization;

namespace Server.Models;

/// <summary>
/// Response model for Allegro OAuth2 token endpoint.
/// Contains access token and related information.
/// </summary>
public class GetTokensResponse
{
    /// <summary>
    /// The access token to be used for API requests.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Type of token (typically "bearer").
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    /// <summary>
    /// Refresh token used to obtain a new access token when the current one expires.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Number of seconds until the access token expires.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Space-separated list of scopes granted with this token (e.g., "allegro_api").
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// JWT Token ID - unique identifier for this token.
    /// </summary>
    [JsonPropertyName("jti")]
    public string? Jti { get; set; }

    /// <summary>
    /// Returns the UTC DateTime when this access token will expire.
    /// </summary>
    public DateTime GetExpirationTime()
    {
        return DateTime.UtcNow.AddSeconds(ExpiresIn);
    }

    /// <summary>
    /// Checks if the access token is still valid (not expired).
    /// </summary>
    public bool IsValid()
    {
        return DateTime.UtcNow < GetExpirationTime();
    }
}

public class Tokens : GetTokensResponse
{
    [JsonPropertyName("expires_on")]
    public DateTime ExpiresOn { get; set; }
}