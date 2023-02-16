using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Extensions.Migration;

namespace App.Lib.Database;

public static class DependencyInject
{
    private static Boolean SerializersRegistered;
    private static object SerializeREgisterLock = new ();

    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("Database"));

        services.AddSingleton<DatabaseContext>();
        services.AddSingleton<DocumentMapFactory>();
        services.AddSingleton<DistributedLockFactory>();

        services.AddScoped<IOAuthTokenStorage, OAuthTokenStorage>();
    }

    public static void UseDatabase(this IApplicationBuilder app)
    {
        lock (SerializeREgisterLock)
        {
            if (!SerializersRegistered)
            {
                var dataProtector = app.ApplicationServices.GetRequiredService<IDataProtectionProvider>();
                BsonSerializer.RegisterSerializer(typeof(EncryptedString), new EncryptedStringSerializer(dataProtector));
                SerializersRegistered = true;
            }            
        }

        app.UseMongoMigration(m => m);
    }
}
