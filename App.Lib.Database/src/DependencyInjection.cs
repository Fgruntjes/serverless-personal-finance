using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Extensions.Migration;

namespace App.Lib.Database;

public static class DependencyInject
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("Database"));

        services.AddSingleton<DatabaseContext>();
        services.AddSingleton<DocumentMapFactory>();
        services.AddSingleton<DistributedLockFactory>();

        services.AddScoped<IOAuthTokenStorage, OAuthTokenStorage>();
    }

    public static void UseDatabaseMigrations(this IApplicationBuilder app)
    {
        app.UseMongoMigration(m => m);
    }
}
