using App.Lib;
using App.Lib.Database;
using App.Lib.Ynab;

await AppWebApplication.CreateAndRun(args, builder =>
{
    builder.Services.AddYnabClient();
    builder.Services.AddDatabase(builder.Configuration);
});

public partial class Program
{
}