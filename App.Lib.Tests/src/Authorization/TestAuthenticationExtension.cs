using App.Lib.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace App.Lib.Tests.Authorization;

public static class TestAuthenticationExtension
{
    public static IWebHostBuilder ConfigureTestAuthServices(this IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .AddAuthentication(defaultScheme: TestAuthenticationHandler.TestScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationHandler.TestScheme, _ => { });

            services.AddSingleton<IAuthorizationHandler, TestAuthorizationHandler>();
        });

        return builder;
    }
}