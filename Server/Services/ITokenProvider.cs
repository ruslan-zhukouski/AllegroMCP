using Server.Models;

namespace Server.Services;

public interface ITokenProvider
{
    Tokens? Load();
    string Save(Tokens? tokens = null);
    // Gets an access token. If it is null, try to refresh tokens
    string? GetAccessToken();
    // Gets a refresh token. If it is null, you should re-generate new pair of tokens
    string? GetRefreshToken();
    // Deletes access and refresh tokens if they are expired, returns tokens otherwise
    Tokens? RemoveExpiredTokens();
}
