using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.IntegrationTests.Utilities;
using Users.Application.Managers.Interfaces;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public abstract class BaseUsersIntegrationTest : BaseSharedIntegrationTest, IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
	protected BaseUsersIntegrationTest(IntegrationTestWebAppFactory integrationTestWebAppFactory)
		: base(integrationTestWebAppFactory.Services.CreateScope())
	{
	}

	protected IRepositoryManager RepositoryManager
	{
		get
		{
			var serviceScope = ServiceScope.ServiceProvider.CreateScope();
			var repositoryManager = serviceScope.ServiceProvider.GetRequiredService<IRepositoryManager>();

			return repositoryManager;
		}
	}
	public async Task DisposeAsync()
	{
		await EnsureDatabaseIsEmpty();
	}

	public async Task InitializeAsync()
	{
		await EnsureDatabaseIsEmpty();
	}

	private async Task EnsureDatabaseIsEmpty()
	{
		var dbContext = ServiceScope.ServiceProvider.GetRequiredService<UsersDBContext>();

		if (dbContext.Users.Any())
		{
			await dbContext.Users.ExecuteDeleteAsync();
		}
	}
}
