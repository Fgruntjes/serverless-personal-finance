using Microsoft.Extensions.Configuration;

namespace App.Lib.Configuration;

public static class ConfigurationExtension
{
    public static T MustGetValue<T>(this IConfiguration configuration, string path)
    {
        var value = configuration.GetValue<T>(path);
        if (value == null)
        {
            throw new InvalidOperationException($"Required configuration `{path}` missing.");
        }
        return value;
    }
}