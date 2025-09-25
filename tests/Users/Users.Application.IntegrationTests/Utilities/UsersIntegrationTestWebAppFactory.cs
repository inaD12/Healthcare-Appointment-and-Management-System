using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NSubstitute;
using Shared.Application.IntegrationTests.Extentensions;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Users.Application.Features.Users.Consumers;
using Users.Infrastructure.Features.DBContexts;
using Users.Infrastructure.Features.Identity;

namespace Users.Application.IntegrationTests.Utilities;

public class UsersIntegrationTestWebAppFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly KeycloakContainer _keycloakContainer;
    
    public UsersIntegrationTestWebAppFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("usersdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilDatabaseIsAvailable(NpgsqlFactory.Instance)
            )
            .Build();


        _keycloakContainer = new KeycloakBuilder()
            .WithImage("quay.io/keycloak/keycloak:latest")
            .WithResourceMapping("../../../../../../src/Users/Users.Infrastructure/Features/Import/realm-export.json", "/opt/keycloak/data/import")
            .WithCommand("--import-realm")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var baseUrl = _keycloakContainer.GetBaseAddress();
        
        builder.ConfigureAppConfiguration((config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["KeyCloak:AdminUrl"] = $"{baseUrl}admin/realms/hams/",
                ["KeyCloak:TokenUrl"] = $"{baseUrl}realms/hams/protocol/openid-connect/token",
            });
        });
        
        builder.ConfigureTestServices(serviceCollection =>
        {
            serviceCollection.AddTestDbContext<UsersDbContext>(opt => opt.UseNpgsql(_dbContainer.GetConnectionString()));

            serviceCollection.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<UserCreatedDomainEventConsumer>();
            });

            var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            serviceCollection.AddSingleton(mockHttpContextAccessor);

            serviceCollection.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = _keycloakContainer.GetBaseAddress();
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _keycloakContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _keycloakContainer.DisposeAsync();
    }
}
