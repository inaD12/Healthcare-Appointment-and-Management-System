using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shared.Application.IntegrationTests.Extentensions;
using Testcontainers.PostgreSql;
using Users.Application.Features.Users.Consumers;
using Users.Infrastructure.DBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public class UsersIntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime

{
	private readonly PostgreSqlContainer _dbContainer;

	public UsersIntegrationTestWebAppFactory()
	{
		_dbContainer = new PostgreSqlBuilder()
		.WithImage("postgres:latest")
		.WithDatabase("usersdb")
		.WithUsername("postgres")
		.WithPassword("postgres")
		.Build();
	}
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(serviceCollection =>
		{
			serviceCollection.AddTestDbContext<UsersDBContext>(opt => opt.UseNpgsql(_dbContainer.GetConnectionString()));

			serviceCollection.AddMassTransitTestHarness(cfg =>
			{
				cfg.AddConsumer<UserCreatedDomainEventConsumer>();
			});

			var mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
			serviceCollection.AddSingleton(mockHttpContextAccessor);
		});
	}
	public new Task DisposeAsync()
	{
		return _dbContainer.DisposeAsync().AsTask();
	}

	public Task InitializeAsync()
	{
		return _dbContainer.StartAsync();
	}
}
