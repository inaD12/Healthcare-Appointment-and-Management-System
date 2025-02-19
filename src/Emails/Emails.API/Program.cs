using Emails.API.Extensions;
using Emails.Application.Extensions;
using Serilog;
using Shared.API.Extensions;
using Shared.API.Middlewares;
using Shared.API.Utilities;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();

builder.Services
	.AddApplicationLayer(config)
	.AddAPILayer(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
