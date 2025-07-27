using RateOffersAPI3.Controllers;
using RateOffersAPI3.Middlewares;
using RateOffersAPI3.Services.Contrats;
using RateOffersAPI3.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the rate provider service
builder.Services.AddSingleton<IRateProvider, RateProvider>();
builder.Services.AddSingleton<IConvertService, ConvertService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.MapJsonRateEndpoints();

app.Run();
