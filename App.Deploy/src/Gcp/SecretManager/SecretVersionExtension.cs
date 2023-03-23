using Pulumi.Gcp.CloudRunV2.Inputs;
using Pulumi.Gcp.SecretManager;

namespace App.Deploy.Gcp.SecretManager;

internal static class SecretVersionExtension
{
    public static ServiceTemplateContainerEnvValueSourceArgs ToServiceTemplateContainerEnv(this SecretVersion version)
    {
        return new ServiceTemplateContainerEnvValueSourceArgs
        {
            SecretKeyRef = new ServiceTemplateContainerEnvValueSourceSecretKeyRefArgs
            {
                Secret = version.Secret,
                Version = version.Name,
            }
        };
    }

    public static ServiceTemplateVolumeSecretArgs ToServiceTemplateVolume(this SecretVersion version)
    {
        return new ServiceTemplateVolumeSecretArgs
        {
            Secret = version.Secret,
            Items = new ServiceTemplateVolumeSecretItemArgs
            {
                Version = version.Name
            }
        };
    }
}