using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.TestsLib;

public class TestApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private readonly string _environment;

    public TestApplicationFactory()
    {
        _environment = "Development";
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);
        builder.ConfigureServices(services =>
        {
            services.AddLogging(logBuilder => logBuilder
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Trace));            
        });
        
        
        return base.CreateHost(builder);
    }
}