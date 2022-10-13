using App.LibQueue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQueue(builder.Configuration["GoogleProjectId"]);

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