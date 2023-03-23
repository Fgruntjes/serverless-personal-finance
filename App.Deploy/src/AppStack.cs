using App.Deploy.Component;
using App.Deploy.Component.Function.Integration;
using App.Deploy.Config;
using Pulumi;

namespace App.Deploy;

internal class AppStack : Stack
{
    public AppStack()
    {
        var config = new AppConfig();

        // Infra
        new Auth0Component($"{config.Environment}-auth", config);
        var database = new DatabaseComponent($"{config.Environment}-database", config);

        // Cloud functions
        var ynabComponent = new YnabComponent($"{config.Environment}-fn-integration-ynab", config, database);

        // Frontend
        new FrontendComponent($"{config.Environment}-frontend", config, new Dictionary<string, ICloudFunctionComponent>
        {
            {"FUNCTION_INTEGRATION_YNAB", ynabComponent}
        });
    }
}