using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;

namespace App.Lib.Queue;

public static class DependencyInjection
{
    public static void AddQueue(this IServiceCollection services, string googleProjectId)
    {
        services.AddRebus(c => c
            .Transport(t => t.UsePubSubAsOneWayClient(googleProjectId)));
    }
}