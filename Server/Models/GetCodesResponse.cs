using System.Text.Json.Serialization;

namespace Server.Models;

/// <summary>
/// Response model for Allegro device authorization flow.
/// </summary>
public class GetCodesResponse
{
    /// <summary>
    /// User code - recommended to display to user in format XXX XXX XXX for better readability when retyping.
    /// </summary>
    [JsonPropertyName("user_code")]
    public string? UserCode { get; set; }

    /// <summary>
    /// Device code - required to obtain access token.
    /// </summary>
    [JsonPropertyName("device_code")]
    public string? DeviceCode { get; set; }

    /// <summary>
    /// Number of seconds for which both codes are valid.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Required interval (in seconds) between subsequent authorization status queries.
    /// If you query more frequently you'll receive HTTP 400 response with "slow_down" code.
    /// </summary>
    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    /// <summary>
    /// URL for user verification.
    /// </summary>
    [JsonPropertyName("verification_uri")]
    public string? VerificationUri { get; set; }

    /// <summary>
    /// URL for user verification with pre-filled user code.
    /// </summary>
    [JsonPropertyName("verification_uri_complete")]
    public string? VerificationUriComplete { get; set; }

    /// <summary>
    /// Formats the user code for better readability (e.g., "cbt 3zd u4g" from "cbt3zdu4g").
    /// </summary>
    public string GetFormattedUserCode()
    {
        if (string.IsNullOrEmpty(UserCode) || UserCode.Length < 3)
            return UserCode ?? string.Empty;

        return string.Join(" ",
            UserCode.Chunk(3)
                .Select(chunk => new string(chunk)));
    }
}
