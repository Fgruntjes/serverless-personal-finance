using App.Lib.Configuration;

namespace App.Deploy.Config;

public class AppConfig
{
    private readonly Pulumi.Config _config;

    public string Tag => _config.Require("tag");
    public string Environment => _config.Require("environment");
    public string ProjectDir => _config.Require("projectDir");
    public string ProjectSlug => Gcp.Project;
    public string DataProtectionCert => _config.Require("dataProtectionCert");
    
    public GcpConfig Gcp { get; }
    public MongoDbConfig MongoDb { get; }
    public Auth0Config Auth0 { get; }
    public SentryConfig Sentry { get; }
    public YnabConfig Ynab { get; }
    public CloudflareConfig Cloudflare { get; }

    public string FrontendUrl => DevEnvConfigurationProvider.GetFrontendUrl(Environment, ProjectSlug);

    public AppConfig()
    {
        _config = new Pulumi.Config("app");
        Gcp = new GcpConfig("gcp");
        Auth0 = new Auth0Config("auth0");
        Sentry = new SentryConfig("sentry");
        Ynab = new YnabConfig("ynab");

        // Custom appended so pulumi does not throw missing key errors 
        MongoDb = new MongoDbConfig("mongodbatlasCustom");
        Cloudflare = new CloudflareConfig("cloudflareCustom");
    }

    public string ContainerImage(string name)
    {
        return $"{Gcp.Region}-docker.pkg.dev/{ProjectSlug}/docker/{Environment}/{name.ToLower()}:{Tag}";
    }
}