using App.Function.Banktransaction.Import.Service;
using App.Lib;
using App.Lib.Database;

await AppWebApplication.CreateAndRun(
    args,
    builder =>
    {
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddScoped<BankTransactionImportService>();
    },
    app =>
    {
        app.UseDatabase();
    });

public partial class Program
{
}