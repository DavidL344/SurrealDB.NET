using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurrealDB.NET.API;
using SurrealDB.NET.Models;

namespace SurrealDB.NET.Extensions;

public static class SurrealDbExtensions
{
    public static void AddSurrealDb(this IServiceCollection services, SurrealDbConfiguration loginCredentials)
    {
        services.AddSingleton<HttpClient>();
        
        services.AddSingleton(_ => loginCredentials);
        
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<SurrealDbConfiguration>();
            var httpClient = provider.GetRequiredService<HttpClient>();
            
            return new Surreal(configuration, httpClient);
        });
    }
}
