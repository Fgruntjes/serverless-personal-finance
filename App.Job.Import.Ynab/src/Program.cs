using App.Job.Import.Ynab.Command;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddLogging();

var app = new CommandApp<ImportTransactionsCommand>(new DependencyInjectionRegistrar(services));
app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif
});

return app.Run(args);