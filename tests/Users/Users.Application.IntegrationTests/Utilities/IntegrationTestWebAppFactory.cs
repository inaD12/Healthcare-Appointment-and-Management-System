using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Shared.Application.IntegrationTests.Extentensions;
using Testcontainers.PostgreSql;
using Users.Infrastructure.DBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime

{
	private readonly PostgreSqlContainer _dbContainer;

	public IntegrationTestWebAppFactory()
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
