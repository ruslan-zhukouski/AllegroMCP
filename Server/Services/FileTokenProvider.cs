using Server.Models;
using System.Text.Json;

namespace Server.Services;

public class FileTokenProvider(string? pathToTokensFile = null) : ITokenProvider
{
    private static readonly bool IsDocker = bool.Parse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false");
    private readonly string pathToTokensFile = pathToTokensFile ?? (IsDocker ? "/home/app/allegro_tokens.txt"
        : Path.Combine(AppContext.BaseDirectory, "allegro_tokens.txt"));

    // Gets an access token from a file. If it is null, try to refresh tokens
    public string? GetAccessToken()
    {
        var tokens = RemoveExpiredTokens() ??
            throw new InvalidOperationException("Access token is not found. Please generate tokens.");
        if (tokens.ExpiresOn <= DateTime.UtcNow)
            throw new InvalidOperationException("Access token is expired. Please refresh tokens.");
        return tokens.AccessToken;
    }

    // Gets a refresh token from a file. If it is null, you should re-generate new pair of tokens
    public string? GetRefreshToken() => RemoveExpiredTokens()?.RefreshToken;

    // Loads tokens from a file if present. Returns null if not found or empty
    public Tokens? Load()
    {
        try
        {
            if (File.Exists(pathToTokensFile))
            {
                var json = File.ReadAllText(pathToTokensFile).Trim();
                return JsonSerializer.Deserialize<Tokens>(json);
            }
        }
        catch
        {
            // Ignore file IO errors and return null so interactive flow continues
        }

        return null;
    }

    // Deletes a file with access and refresh tokens if they are expired, returns tokens otherwise
    public Tokens? RemoveExpiredTokens()
    {
        var tokens = Load();

        if (tokens == null || tokens.ExpiresOn <= DateTime.UtcNow - TimeSpan.FromDays(90) - TimeSpan.FromHours(12))
        {
            // Delete a file with tokens if tokens are expired for more than 90 days (refresh token lifetime according to Allegro API documentation)
            Save();
            return null;
        }

        return tokens;
    }

    // Persists tokens to a file. Overwrites existing content
    public string Save(Tokens? tokens = null)
    {
        try
        {
            if (tokens is null)
            {
                // Remove file if token cleared
                if (File.Exists(pathToTokensFile))
                    File.Delete(pathToTokensFile);
                return pathToTokensFile;
            }

            var json = JsonSerializer.Serialize(tokens);

            // Ensure directory exists
            var dir = Path.GetDirectoryName(pathToTokensFile);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(pathToTokensFile, json);
        }
        catch
        {
            // Ignore errors writing token file to avoid breaking main flow
        }

        return pathToTokensFile;
    }
}