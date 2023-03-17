using App.Deploy.Config;
using Pulumi;
using Pulumi.Gcp.CloudRunV2;
using Pulumi.Gcp.CloudRunV2.Inputs;

namespace App.Deploy.Component.Function.Integration;

internal class YnabComponent : ComponentResource, ICloudFunctionComponent
{
    private const string ProtectionCertificatePath = "/etc/secrets/gcloud_data_protection_certificate";
    private readonly Service _service;

    [Output]
    public Output<string> Url
    {
        get => _service.Uri;
        // ReSharper disable once UnusedMember.Global
        set { }
    }

    public YnabComponent(
        string name,
        AppConfig config,
        DatabaseComponent database
    ) : this(name, config, database, new ComponentResourceOptions())
    {
    }

    public YnabComponent(
        string name,
        AppConfig config,
        DatabaseComponent database,
        ComponentResourceOptions opts
    ) : base("app:function:integration:ynab", name, opts)
    {
        _service = new Service($"{name}-service", new()
        {
            Name = name,
            Ingress = "INGRESS_TRAFFIC_ALL",
            Location = config.Gcp.Region,
            Labels = new Dictionary<string, string>
            {
                {"environment", config.Environment}
            },
            Template = new ServiceTemplateArgs
            {
                Containers = new[]
                {
                    new ServiceTemplateContainerArgs
                    {
                        Image = config.ContainerImage("App.Function.Integration.Ynab"),
                        Ports = new ServiceTemplateContainerPortArgs
                        {
                            ContainerPort = 80
                        },
                        Envs = new ServiceTemplateContainerEnvArgs[]
                        {
                            new(){ Name = "App__Frontend", Value = config.Frontend },
                            new(){ Name = "App__Environment", Value = config.Environment },
                            new(){ Name = "Auth0__Domain", Value = config.Auth0.Domain },
                            new(){ Name = "Sentry__Dsn", Value = config.Sentry.Dsn },
                            new(){ Name = "Database__ConnectionString", Value = database.ConnectionString },
                            new(){ Name = "Database__DatabaseName", Value = database.DatabaseName },
                            new(){ Name = "Ynab__ClientId", Value = config.Ynab.ClientId },
                            new(){ Name = "Ynab__ClientSecret", Value = config.Ynab.ClientSecret },
                            new(){ Name = "Security__ProtectionCertificate", Value = ProtectionCertificatePath },
                        }
                    },
                },
            },
        });
    }
}