using App.Function.Banktransaction.Import.Service;
using App.LibDatabase;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("Database"));

builder.Services.AddScoped<BankTransactionImportService>();

builder.WebHost.UseSentry();

var app = builder.Build();
app.MapControllers();
if (!app.Environment.IsDevelopment())
{
    app.UseSentryTracing();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program
{
}