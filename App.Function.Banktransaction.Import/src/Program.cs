using App.Function.Banktransaction.Import.Service;
using App.LibDatabase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("Database"));
builder.Services.AddDatabase();
builder.Services.AddScoped<BankTransactionImportService>();

var app = builder.Build();
app.MapControllers();
app.Run();

public partial class Program
{
}