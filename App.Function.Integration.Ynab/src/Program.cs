using App.Lib;
using App.Lib.Database;
using App.Lib.Ynab;

await AppWebApplication.CreateAndRun(
    args,
    builder =>
    {
        builder.Services.AddYnabClient(builder.Configuration);
        builder.Services.AddDatabase(builder.Configuration);
    },
    app =>
    {
        app.UseDatabaseMigrations();
    });

public partial class Program
{
}