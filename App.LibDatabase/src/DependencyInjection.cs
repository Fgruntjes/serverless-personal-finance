using Microsoft.Extensions.DependencyInjection;

namespace App.LibDatabase;

public static class DependencyInject
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<DbContext>();
        services.AddSingleton<DocumentMapFactory>();
    }
}
