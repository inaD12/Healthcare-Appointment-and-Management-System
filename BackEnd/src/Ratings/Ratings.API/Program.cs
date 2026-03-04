using Ratings.Application.Extensions;
using Serilog;
using Shared.API.Extensions;
using Shared.API.Helpers;
using Shared.API.Utilities;
using Ratings.API.Extensions;
using Ratings.Infrastructure.Extensions;

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

app.UseExceptionHandler();

EndpointMapper.MapAllEndpoints(app);

app.Run();

namespace Ratings.API
{
    public partial class Program {}
}