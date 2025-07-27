using RateOffersAPI1.Controllers;
using RateOffersAPI1.Services.Contrats;
using RateOffersAPI1.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the rate provider service
builder.Services.AddSingleton<IRateProvider, RateProvider>();

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
