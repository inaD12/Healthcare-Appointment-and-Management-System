using Users.Application;
using Users.Infrastructure;
using UsersAPI.Extentions;
using Serilog;
using UsersAPI.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Shared.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.ConfigureDBs(config);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.ConfigureAppSettings(config);
builder.Services.InjectAuthentication(config);
builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.InjectMassTransit();
builder.Services.AddFluentEmail(config);
builder.Services.ConfigureCors();

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer();

builder.Host.ConfigureSerilog();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = false;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	await app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

EndpointMapper.MapAllEndpoints(app);

//app.UseHttpsRedirection();

app.Run();

public partial class Program { }