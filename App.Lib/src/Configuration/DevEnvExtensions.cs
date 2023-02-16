using Microsoft.Extensions.Configuration;

namespace App.Lib.Configuration;

public static class DevEnvExtensions
{
    public static IConfigurationBuilder AddDevEnvVariables(this IConfigurationBuilder builder)
    {
        return builder.Add(new DevEnvConfigurationSource());
    }
}