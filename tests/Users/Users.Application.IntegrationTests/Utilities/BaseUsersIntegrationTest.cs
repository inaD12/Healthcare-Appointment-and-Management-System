using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.IntegrationTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities;
using Shared.Domain.Utilities;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Infrastructure.Auth.Abstractions;
using Users.Domain.Infrastructure.Auth.Models;
using Users.Domain.Utilities;
using Users.Infrastructure.Features.DBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public abstract class BaseUsersIntegrationTest : BaseSharedIntegrationTest, IClassFixture<UsersIntegrationTestWebAppFactory>, IAsyncLifetime
{
	protected BaseUsersIntegrationTest(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory)
		: base(integrationTestWebAppFactory.Services.CreateScope())
	{
		IdentityProviderService = ServiceScope.ServiceProvider.GetRequiredService<IIdentityProviderService>();
		UserRepository = ServiceScope.ServiceProvider.GetRequiredService<IUserRepository>();
		TestHarness = ServiceScope.ServiceProvider.GetTestHarness();
		EmailVerificationTokenRepository = ServiceScope.ServiceProvider.GetRequiredService<IEmailVerificationTokenRepository>();

	}
	protected IIdentityProviderService  IdentityProviderService {get;}
	protected IUserRepository UserRepository { get; }
	protected ITestHarness TestHarness { get; }
	protected IEmailVerificationTokenRepository EmailVerificationTokenRepository { get; }

	protected async Task<User> CreateUserAsync()
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		
		var user = User.Create(
				UsersTestUtilities.ValidEmail,
				Role.Patient,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate.ToUniversalTime(),
				SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH),
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress
			);
		
		await IdentityProviderService.RegisterUserAsync(
			new UserModel(user.Email, UsersTestUtilities.ValidPassword, user.FirstName, user.LastName),
			CancellationToken);

		await UserRepository.AddAsync(user);
		await unitOfWork.SaveChangesAsync();

		return user;
	}

	protected async Task<User> CreateUserAsync(string email)
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

		var user = User.Create(
				email,
				Role.Patient,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate.ToUniversalTime(),
				SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH),
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress
			);

		await IdentityProviderService.RegisterUserAsync(
			new UserModel(user.Email, UsersTestUtilities.ValidPassword, user.FirstName, user.LastName),
			CancellationToken);
		
		await UserRepository.AddAsync(user);
		await unitOfWork.SaveChangesAsync();

		return user;
	}

	protected async Task<EmailVerificationToken> CreateEmailVerificationTokenWithEmailVerifiedUserAsync()
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		var token = await CreateEmailVerificationTokenAsync();

		token.User.VerifyEmail();
		UserRepository.Update(token.User);
		await unitOfWork.SaveChangesAsync();

		return token;
	}
	protected async Task<EmailVerificationToken> CreateEmailVerificationTokenAsync(DateTime? expirationDate = null)
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		var user = await CreateUserAsync();

		var token = EmailVerificationToken.Create(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.PastDate,
			expirationDate ?? UsersTestUtilities.CurrentDate.AddDays(1),
			user
		);

		await EmailVerificationTokenRepository.AddAsync(token);
		await unitOfWork.SaveChangesAsync();

		return token;
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
		var dbContext = ServiceScope.ServiceProvider.GetRequiredService<UsersDbContext>();

		if (dbContext.Users.Any())
		{
			await dbContext.Users.ExecuteDeleteAsync();
		}
	}
}
