using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace App.Lib.Database;

public static class DependencyInject
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("Database"));

        services.AddSingleton<DatabaseContext>();
        services.AddSingleton<DocumentMapFactory>();
        services.AddScoped<OAuthTokenStorage>();
    }
}
