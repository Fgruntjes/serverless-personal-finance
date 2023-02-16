using DotNetEnv;
using Microsoft.Extensions.Configuration;

namespace App.Lib.Configuration;

public class DevEnvConfigurationProvider : ConfigurationProvider
{
    public override void Load()
    {
        var devVars = LoadFile();

        Data["App:Environment"] = devVars["APP_ENVIRONMENT"];
        Data["App:Frontend"] = GetFrontendUrl(devVars["APP_ENVIRONMENT"], devVars["GOOGLE_PROJECT_ID"]);
        Data["Auth0:Domain"] = devVars["AUTH0_DOMAIN"];
    }

    private string GetFrontendUrl(string appEnvironment, string googleProjectId)
    {
        // If changed also change .github/workflows/deploy.yaml jobs.deploy
        switch (appEnvironment)
        {
            case "dev":
                return "http://localhost:3000";
            case "main":
                return $"https://{googleProjectId}.pages.dev";
            default:
                return $"https://{appEnvironment}.{googleProjectId}.pages.dev";
        }
    }

    private Dictionary<string, string> LoadFile()
    {
        return Env
            .NoEnvVars()
            .LoadMulti(GetEnvFiles())
            .ToDictionary();
    }

    private string[] GetEnvFiles()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        string ansibleScript;
        do
        {
            ansibleScript = Path.Combine(currentDirectory, "deploy", "run-ansible.sh");
            currentDirectory = Directory.GetParent(currentDirectory).FullName;
        } while (!File.Exists(ansibleScript));

        var deployDirectory = Directory.GetParent(ansibleScript).FullName;

        return new[]
        {
            Path.Combine(deployDirectory, ".env"),
            Path.Combine(deployDirectory, ".env.deploy.local"),
            Path.Combine(deployDirectory, ".env.local"),
        };
    }
}