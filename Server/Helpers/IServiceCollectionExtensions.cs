using Microsoft.Extensions.DependencyInjection;
using Server.Services;

namespace Server.Helpers;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAllegroMCP(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<ITokenProvider, FileTokenProvider>();
        return services;
    }
}
