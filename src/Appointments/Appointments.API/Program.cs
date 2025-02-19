using Appointments.API.Extentions;
using Appointments.Application.Extensions;
using Appointments.Infrastructure.Extensions;
using Hangfire;
using Serilog;
using Shared.API.Extensions;
using Shared.API.Middlewares;
using Shared.API.Utilities;

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

app.UseHangfireDashboard();

EndpointMapper.MapAllEndpoints(app);

//app.UseHttpsRedirection();

app.Run();

public partial class Program { }