using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

namespace App.Lib;

public static class AppWebApplication
{
    private const string CORSDevelopmentPolicy = "CORSDevelopmentPolicy";
    private const string CORSProductionPolicy = "CORSProductionPolicy";

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
        builder.Configuration
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

        builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGenNewtonsoftSupport();
        builder.Services.AddLogging();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: CORSDevelopmentPolicy,
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000");
                });
            options.AddPolicy(name: CORSProductionPolicy, _ => { });
        });

        await configureBuilder(builder);
        builder.WebHost.UseSentry();

        var app = builder.Build();
        app.MapControllers();
        if (!app.Environment.IsDevelopment())
        {
            app.UseSentryTracing();
            app.UseCors(CORSProductionPolicy);
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(CORSDevelopmentPolicy);
        }

        await configureApp(app);

        app.Run();
    }
}