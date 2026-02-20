using Patients.API.Extensions;
using Patients.Application.Extensions;
using Patients.Infrastructure.Extensions;
using Serilog;
using Shared.API.Extensions;
using Shared.API.Helpers;
using Shared.API.Utilities;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Host.ConfigureSerilog();

builder.Services
    .AddApplicationLayer(config)
    .AddInfrastructureLayer(config)
    .AddApiLayer(config);

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

app.UseExceptionHandler();

EndpointMapper.MapAllEndpoints(app);

app.Run();

namespace Patients.API
{
    public partial class Program {}
}