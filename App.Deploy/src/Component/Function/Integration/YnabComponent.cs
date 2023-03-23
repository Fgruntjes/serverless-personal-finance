using App.Deploy.Config;
using App.Deploy.Gcp.SecretManager;
using Pulumi;
using Pulumi.Gcp.CloudRunV2;
using Pulumi.Gcp.CloudRunV2.Inputs;
using Pulumi.Gcp.SecretManager;
using Pulumi.Gcp.SecretManager.Inputs;

namespace App.Deploy.Component.Function.Integration;

internal class YnabComponent : ComponentResource, ICloudFunctionComponent
{
    private const string ProtectionCertificatePath = "/etc/secrets/gcloud_data_protection_certificate";
    private const string DataProtectionCertificateVolumeName = "gcloud_data_protection_certificate";
    private readonly Service _service;
    private readonly string _name;
    private readonly AppConfig _config;

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
        _name = name;
        _config = config;

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
                            new(){ Name = "App__Frontend", Value = config.FrontendUrl },
                            new(){ Name = "App__Environment", Value = config.Environment },
                            new(){ Name = "Auth0__Domain", Value = config.Auth0.Domain },
                            new(){ Name = "Database__DatabaseName", Value = "function_integration_ynab" },
                            new(){ Name = "Ynab__ClientId", Value = config.Ynab.ClientId },
                            new(){ Name = "Security__ProtectionCertificatePath", Value = ProtectionCertificatePath },

                            new(){
                                Name = "Sentry__Dsn",
                                ValueSource = CreateSecret("sentry-dsn", config.Sentry.Dsn)
                                    .ToServiceTemplateContainerEnv()
                            },
                            new()
                            {
                                Name = "Database__ConnectionString",
                                ValueSource = database.ConnectionString
                                    .Apply(str => CreateSecret("database-connection-string", str)
                                        .ToServiceTemplateContainerEnv())
                            },
                            new()
                            {
                                Name = "Ynab__ClientSecret",
                                ValueSource = CreateSecret("ynab-clientsecret", config.Ynab.ClientSecret)
                                    .ToServiceTemplateContainerEnv()
                            },
                        },
                        VolumeMounts = new []
                        {
                            new ServiceTemplateContainerVolumeMountArgs
                            {
                                Name = DataProtectionCertificateVolumeName,
                                MountPath = ProtectionCertificatePath
                            }
                        }
                    },
                },
                Volumes = new[]
                {
                    new ServiceTemplateVolumeArgs()
                    {
                        Name = DataProtectionCertificateVolumeName,
                        Secret = CreateSecret("data-protection-cert", _config.DataProtectionCert)
                            .ToServiceTemplateVolume()
                    },
                }
            },
        });
    }

    private SecretVersion CreateSecret(string identifier, string value)
    {
        var secret = new Secret($"{_name}-{identifier}", new()
        {
            SecretId = $"{_name}-{identifier}",
            Labels = new Dictionary<string, string>
            {
                { "environment", _config.Environment }
            },
            Replication = new SecretReplicationArgs { Automatic = true }
        });

        return new SecretVersion($"{_name}-{identifier}", new SecretVersionArgs
        {
            Secret = secret.Id,
            Enabled = true,
            SecretData = value
        });
    }
}