using Appointments.Infrastructure.Features.DBContexts;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using Shared.Application.IntegrationTests.Extentensions;
using Testcontainers.PostgreSql;

namespace Appointments.Application.IntegrationTests.Utilities;

public class AppointmentsIntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime

{
	private readonly PostgreSqlContainer _dbContainer;

	public AppointmentsIntegrationTestWebAppFactory()
	{
		_dbContainer = new PostgreSqlBuilder()
		.WithImage("postgres:latest")
		.WithDatabase("appointmentsdb")
		.WithUsername("postgres")
		.WithPassword("postgres")
		.Build();
	}
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(serviceCollection =>
		{
			serviceCollection.AddTestDbContext<AppointmentsDBContext>(opt => opt.UseNpgsql(_dbContainer.GetConnectionString()));

			serviceCollection.AddMassTransitTestHarness(cfg =>
			{
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
