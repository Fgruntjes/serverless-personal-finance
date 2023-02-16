using Microsoft.Extensions.Configuration;

namespace App.Lib.Configuration;

public class DevEnvConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => new DevEnvConfigurationProvider();
}