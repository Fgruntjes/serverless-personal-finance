using App.Function.Banktransaction.Import.Service;
using App.LibDatabase;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSentry();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase();
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("Database"));

builder.Services.AddScoped<BankTransactionImportService>();

var app = builder.Build();
app.MapControllers();
if (!app.Environment.IsDevelopment())
{
    app.UseSentryTracing();
}
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program
{
}