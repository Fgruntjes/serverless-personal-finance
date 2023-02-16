using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace App.Lib.Authorization;

public static class AppAuthenticationExtensions
{
    public static IServiceCollection AddAppAuthentication(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        var policies = config["Auth0:Policies"]?.Split(',') ?? Array.Empty<string>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{config["Auth0:Domain"]}/";
                options.Audience = $"{config["App:Environment"]}:{config["Auth0:Audience"]}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        services
            .AddAuthorization(options =>
            {
                foreach (var policy in policies)
                {
                    var requirement = new HasScopeRequirement(policy, $"https://{config["Auth0:Domain"]}/");
                    options.AddPolicy(policy, p => p.Requirements.Add(requirement));
                }
            });

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        return services;
    }
}