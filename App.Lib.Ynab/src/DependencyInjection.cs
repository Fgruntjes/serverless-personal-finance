using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using Refit;

namespace App.Lib.Ynab;

public static class DependencyInjection
{
    private const double ThrottleTimeInMs = 1000;

    public static void AddYnabClient(this IServiceCollection servicesCollection)
    {
        servicesCollection.AddScoped<YnabRefreshTokenHandler>();

        servicesCollection
            .AddHttpClient(nameof(IApiClient))
            .AddTypedClient((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<YnabOptions>>();
                var settings = new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    }
                };
                return RestService.For<IApiClient>(options.Value.BaseAddress, settings);
            })
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
            .AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(
                    3, attempt => TimeSpan.FromMilliseconds(ThrottleTimeInMs * Math.Pow(2, attempt))))
            .AddTransientHttpErrorPolicy(builder =>
                builder.CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)))
            .AddHttpMessageHandler<YnabRefreshTokenHandler>();
    }
}