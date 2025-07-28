using OrchestratorMicroService.API.Middlewares;
using OrchestratorMicroService.Application;
using OrchestratorMicroService.Application.Options;
using OrchestratorMicroService.Infrastructure;
using OrchestratorMicroService.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<OrchestratorOptions>(builder.Configuration.GetSection("OrchestratorSettings"));
builder.Services.Configure<ApiProviderSettings>(builder.Configuration.GetSection("ApiProviders"));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
