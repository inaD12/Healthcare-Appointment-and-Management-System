using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.IntegrationTests.Utilities;
using Shared.Domain.Enums;
using Users.Application.Auth.PasswordManager;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Utilities;
using Users.Infrastructure.DBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public abstract class BaseUsersIntegrationTest : BaseSharedIntegrationTest, IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
	protected BaseUsersIntegrationTest(IntegrationTestWebAppFactory integrationTestWebAppFactory)
		: base(integrationTestWebAppFactory.Services.CreateScope())
	{
		PasswordManager = ServiceScope.ServiceProvider.GetRequiredService<IPasswordManager>();
	}

	protected IPasswordManager PasswordManager { get; }
	private IRepositoryManager _repositoryManager;

	protected IRepositoryManager RepositoryManager
	{
		get
		{
			if (_repositoryManager == null)
			{
				_repositoryManager = ServiceScope.ServiceProvider.GetRequiredService<IRepositoryManager>();
			}
			return _repositoryManager;
		}
	}

	protected async Task<string> CreateUserAsync()
	{
		var user = new User(
				Guid.NewGuid().ToString(),
				UsersTestUtilities.TakenEmail,
				UsersTestUtilities.ValidPasswordHash,
				UsersTestUtilities.ValidSalt,
				Roles.Doctor,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate.ToUniversalTime(),
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress,
				false
			);

		await RepositoryManager.User.AddAsync(user);
		await RepositoryManager.User.SaveChangesAsync();

		return user.Email;
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
