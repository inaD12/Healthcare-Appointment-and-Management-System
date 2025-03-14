﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.IntegrationTests.Utilities;
using Shared.Domain.Enums;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Managers.Interfaces;
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
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

		var user = new User(
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
		await unitOfWork.SaveChangesAsync();

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
