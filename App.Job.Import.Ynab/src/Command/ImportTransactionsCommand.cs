using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace App.Job.Import.Ynab.Command;

public class ImportTransactionsCommand : Command<ImportTransactionsCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<auth_token>")]
        [Description("Personal access token or file containing token")]
        public string AuthToken { get; init; }

        [CommandOption("-d|--dry-run")]
        [DefaultValue(false)]
        public bool DryRun { get; init; }

        [CommandOption("-e|--ynab-endpoint")]
        [DefaultValue("https://api.youneedabudget.com")]
        public string YnabEndpoint { get; init; }

        [CommandOption("-a|--data-endpoint")]
        [DefaultValue("https://asset-management.gruntjes.net")]
        public string DataEndpoint { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        return 0;
    }

    private string? GetFileContentOrValue(string value)
    {
        if (!File.Exists(value))
        {
            return value;
        }

        try
        {
            return File.ReadAllText(value);
        }
        catch (UnauthorizedAccessException)
        {
            AnsiConsole.MarkupLine($"Could not read file: [red]{value}[/]");
            return null;
        }
    }
}