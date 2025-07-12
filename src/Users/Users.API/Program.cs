using Serilog;
using Shared.API.Extensions;
using Shared.API.Helpers;
using Shared.API.Middlewares;
using Shared.API.Utilities;
using Users.Application.Extensions;
using Users.Extensions;
using Users.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();

builder.Services
	.AddApplicationLayer(config)
	.AddInfrastructureLayer(config)
	.AddAPILayer(config);

var app = builder.Build();

await app.SetUpDatabaseAsync();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseCors(AppPolicies.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

EndpointMapper.MapAllEndpoints(app);

app.Run();

public partial class Program { }