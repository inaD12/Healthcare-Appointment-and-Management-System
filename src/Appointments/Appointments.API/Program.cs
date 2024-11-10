using Appointments.API.Extentions;
using Appointments.API.Middlewares;
using Appointments.Application.DependancyInjection;
using Appointments.Infrastructure.DependancyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.ConfigureDBs(config);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InjectAuthentication(config);

builder.Services.AddSwagger();

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer(config);

builder.Host.ConfigureSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.Run();

