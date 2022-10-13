using Microsoft.Extensions.DependencyInjection;

namespace App.LibDatabase;

public static class DependencyInject
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseContext>();
        services.AddSingleton<DocumentMapFactory>();
    }
}
