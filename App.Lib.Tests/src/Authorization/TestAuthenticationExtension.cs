using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Lib.Tests.Authorization;

public static class TestAuthenticationExtension
{
    public static IHostBuilder ConfigureTestAuthServices(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddAuthentication(defaultScheme: TestAuthenticationHandler.TestScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationHandler.TestScheme, _ => { });

            services.AddSingleton<IAuthorizationHandler, TestAuthorizationHandler>();
        });

        return builder;
    }
}