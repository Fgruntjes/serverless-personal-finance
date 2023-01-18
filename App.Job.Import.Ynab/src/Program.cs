using App.Lib;
using App.Lib.Queue;

await AppWebApplication.CreateAndRun(
    args,
    builder =>
    {
        builder.Services.AddQueue(builder.Configuration["GoogleProjectId"]);
    });

public partial class Program
{
}