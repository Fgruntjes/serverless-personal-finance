using App.Lib.Ynab.Rest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Refit;

namespace App.Lib.Ynab;

public static class DependencyInjection
{
    private const double ThrottleTimeInMs = 1000;

    public static void AddYnabClient(this IServiceCollection servicesCollection, IConfiguration configuration)
    {
        servicesCollection.Configure<YnabOptions>(configuration.GetSection(YnabOptions.OptionsKey));
        
        servicesCollection.AddScoped<IConnectService, ConnectService>();
        servicesCollection.AddScoped<RefreshTokenHandler>();

        servicesCollection
            .AddRefitClient<IApiClient>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<YnabOptions>>();
                client.BaseAddress = new Uri(options.Value.BaseAddress);
            })
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
            .AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(
                    3, attempt => TimeSpan.FromMilliseconds(ThrottleTimeInMs * Math.Pow(2, attempt))))
            .AddTransientHttpErrorPolicy(builder =>
                builder.CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)))
            .AddHttpMessageHandler<RefreshTokenHandler>();
    }
}