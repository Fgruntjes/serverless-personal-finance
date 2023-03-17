using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using App.Lib.Authorization;
using App.Lib.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

namespace App.Lib;

public static class AppWebApplication
{
    public const string SwaggerBuildEnvironment = "SwaggerBuild";

    public static async Task CreateAndRun(string[] args)
    {
        await CreateAndRun(args, _ => { }, _ => { });
    }

    public static async Task CreateAndRun(
        string[] args,
        Action<WebApplication> configureApp)
    {
        await CreateAndRun(
            args,
            _ => { },
            configureApp);
    }

    public static async Task CreateAndRun(
        string[] args,
        Func<WebApplication, Task> configureApp)
    {
        await CreateAndRun(
            args,
            _ => { },
            configureApp);
    }

    public static async Task CreateAndRun(
        string[] args,
        Action<WebApplicationBuilder> configureBuilder)
    {
        await CreateAndRun(
            args,
            configureBuilder,
            _ => { });
    }

    public static async Task CreateAndRun(
        string[] args,
        Func<WebApplicationBuilder, Task> configureBuilder)
    {
        await CreateAndRun(
            args,
            configureBuilder,
            _ => { });
    }

    public static async Task CreateAndRun(
        string[] args,
        Action<WebApplicationBuilder> configureBuilder,
        Action<WebApplication> configureApp)
    {
        await CreateAndRun(
            args,
            builder =>
            {
                configureBuilder(builder);
                return Task.CompletedTask;
            },
            app =>
            {
                configureApp(app);
                return Task.CompletedTask;
            });
    }

    public static async Task CreateAndRun(
        string[] args,
        Func<WebApplicationBuilder, Task> configureBuilder,
        Action<WebApplication> configureApp)
    {
        await CreateAndRun(
            args,
            configureBuilder,
            app =>
            {
                configureApp(app);
                return Task.CompletedTask;
            });
    }

    public static async Task CreateAndRun(
        string[] args,
        Action<WebApplicationBuilder> configureBuilder,
        Func<WebApplication, Task> configureApp)
    {
        await CreateAndRun(
            args,
            builder =>
            {
                configureBuilder(builder);
                return Task.CompletedTask;
            },
            configureApp);
    }

    public static async Task CreateAndRun(
        string[] args,
        Func<WebApplicationBuilder, Task> configureBuilder,
        Func<WebApplication, Task> configureApp)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.OptionsKey));
        builder.Services.Configure<Auth0Options>(builder.Configuration.GetSection(Auth0Options.OptionsKey));

        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddDevEnvVariables();
        }

        builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IAuthContext, AuthContext>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        builder.Services.AddLogging();
        ConfigureDataProtection(builder);
        builder.Services.AddCors();
        builder.Services.AddAppAuthentication(builder.Configuration);

        await configureBuilder(builder);
        builder.WebHost.UseSentry();

        var app = builder.Build();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(policy =>
        {
            var settings = app.Services.GetRequiredService<IOptions<AppOptions>>();
            policy.WithOrigins(settings.Value.Frontend)
                .WithHeaders(
                    "Authorization",
                    AuthContext.HeaderTenant
                )
                .AllowCredentials()
                .AllowAnyMethod();
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseSentryTracing();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        await configureApp(app);

        app.Run();
    }

    private static void ConfigureDataProtection(WebApplicationBuilder builder)
    {
        if (builder.Environment.EnvironmentName == SwaggerBuildEnvironment)
        {
            return;
        }

        var certificate = builder.Configuration.MustGetValue<string>("Security:ProtectionCertificate");

        builder.Services.AddDataProtection()
                    .ProtectKeysWithCertificate(new X509Certificate2(certificate));
    }
}
