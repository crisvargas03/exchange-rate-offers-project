using RateOffersAPI1.Controllers;
using RateOffersAPI1.Services.Contrats;
using RateOffersAPI1.Services.Implementations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurate Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .CreateLogger();


// Register the rate provider service
builder.Services.AddSingleton<IRateProvider, RateProvider>();

// Configure Serilog as the logging provider
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapRatesExchamgesEndpoints();

app.Run();
