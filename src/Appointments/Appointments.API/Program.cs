using Appointments.API.Extentions;
using Appointments.API.Middlewares;
using Appointments.Application.DependancyInjection;
using Appointments.Infrastructure.DependancyInjection;
using Hangfire;
using Serilog;
using Shared.Application.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.ConfigureDBs(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InjectAuthentication(config);//Assembly.GetExecutingAssembly()

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddSwagger();
builder.Services.ConfigureAppSettings(config);
builder.Services.InjectMassTransit();
builder.Services.ConfigureHangFire(config);

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer(config);

builder.Host.ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHangfireDashboard();

EndpointMapper.MapAllEndpoints(app);

//app.UseHttpsRedirection();

app.Run();

