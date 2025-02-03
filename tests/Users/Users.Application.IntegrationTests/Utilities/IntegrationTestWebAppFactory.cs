using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Shared.Application.IntegrationTests.Extentensions;
using Testcontainers.PostgreSql;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
		.WithImage("postgres:latest")
		.WithDatabase("appointments.database")
		.WithUsername("posgtres")
		.WithPassword("postgres")
		.Build();
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(serviceCollection =>
		{
			serviceCollection.AddTestDbContext<UsersDBContext>(opt => opt.UseNpgsql(_dbContainer.GetConnectionString()));
		});
	}
	public new Task DisposeAsync()
	{
		return _dbContainer.StopAsync();
	}

	public Task InitializeAsync()
	{
		return _dbContainer.StartAsync();
	}
}
