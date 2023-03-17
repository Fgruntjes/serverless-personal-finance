using App.Deploy.Config;
using Pulumi;
using Pulumi.Auth0;
using Pulumi.Cloudflare;
using Pulumi.Cloudflare.Inputs;
using Pulumi.Command.Local;

namespace App.Deploy.Component;

internal class FrontendComponent : ComponentResource
{
    private readonly string _name;
    private readonly AppConfig _config;
    private readonly IDictionary<string, ICloudFunctionComponent> _functions;
    private readonly Client _authClient;
    private Command _buildCommand;

    [Output]
    public Output<string> FrontendUrl
    {
        get => Output.Create($"https://{_config.Environment}.{_config.ProjectSlug}.pages.dev"
            .Replace("https://main.", "https://"));
        // ReSharper disable once UnusedMember.Global
        set { }
    }

    public FrontendComponent(
        string name,
        AppConfig config,
        IDictionary<string, ICloudFunctionComponent> functions
        ) : this(name, config, functions, new ComponentResourceOptions())
    {
    }

    public FrontendComponent(
        string name,
        AppConfig config,
        IDictionary<string, ICloudFunctionComponent> functions,
        ComponentResourceOptions opts
    ) : base("app:frontend", name, opts)
    {
        _name = name;
        _config = config;
        _functions = functions;

        _authClient = new Client($"{name}-client", new()
        {
            AppType = "spa",
            AllowedOrigins = new[] { FrontendUrl },
            Callbacks = new[] { FrontendUrl },
        });

        BuildFrontend();
        EnsureWrangler();
        DeployCloudflarePage();
    }

    private void BuildFrontend()
    {
        var envMap = new InputMap<string>
        {
            { "REACT_APP_APP_ENVIRONMENT", _config.Environment },
            { "REACT_APP_AUTH0_CLIENT_ID", _authClient.ClientId },
            { "REACT_APP_AUTH0_DOMAIN", _config.Auth0.Domain },
        };

        foreach (var kvp in _functions)
        {
            envMap.Add($"REACT_APP_{kvp.Key}_BASE", kvp.Value.Url);
        }

        _buildCommand = new Command($"{_name}-build", new CommandArgs
        {
            Create = "(npm ci && npm run build) 2>&1",
            Environment = envMap,
            Dir = $"{_config.ProjectDir}/frontend-web"
        });
    }

    private void EnsureWrangler()
    {
        new Command($"{_name}-install-dependencies", new CommandArgs
        {
            Create = "npm i wrangler 2>&1",
        });
    }

    private void DeployCloudflarePage()
    {
        var project = new PagesProject($"{_name}-project", new()
        {
            Name = _config.ProjectSlug,
            ProductionBranch = "main",
            AccountId = _config.Cloudflare.AccountId
        });

        new Command(
            $"{_name}-deploy",
            new CommandArgs
            {
                Create = project.Name.Apply(name => string.Join(
                    ' ',
                    "npx wrangler pages publish build",
                    "--commit-dirty=true",
                    $"--branch=\"{_config.Environment}\" ",
                    $"--project-name=\"{name}\" ",
                    "2>&1")),
                Dir = $"{_config.ProjectDir}/frontend-web",
            },
            new CustomResourceOptions
            {
                DependsOn = { _buildCommand, project }
            });
    }
}