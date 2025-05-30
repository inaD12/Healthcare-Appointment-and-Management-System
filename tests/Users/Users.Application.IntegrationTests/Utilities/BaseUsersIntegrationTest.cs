using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.API.Abstractions;
using Shared.Application.IntegrationTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Utilities;
using Users.Infrastructure.DBContexts;

namespace Users.Application.IntegrationTests.Utilities;

public abstract class BaseUsersIntegrationTest : BaseSharedIntegrationTest, IClassFixture<UsersIntegrationTestWebAppFactory>, IAsyncLifetime
{
	protected BaseUsersIntegrationTest(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory)
		: base(integrationTestWebAppFactory.Services.CreateScope())
	{
		PasswordManager = ServiceScope.ServiceProvider.GetRequiredService<IPasswordManager>();
		UserRepository = ServiceScope.ServiceProvider.GetRequiredService<IUserRepository>();
		TestHarness = ServiceScope.ServiceProvider.GetTestHarness();
		ClaimsExtractor = ServiceScope.ServiceProvider.GetRequiredService<IClaimsExtractor>();
		EmailVerificationTokenRepository = ServiceScope.ServiceProvider.GetRequiredService<IEmailVerificationTokenRepository>();

	}
	protected IUserRepository UserRepository { get; }
	protected IPasswordManager PasswordManager { get; }
	protected ITestHarness TestHarness { get; }
	protected IClaimsExtractor ClaimsExtractor { get; }
	protected IEmailVerificationTokenRepository EmailVerificationTokenRepository { get; }

	protected string Password => UsersTestUtilities.ValidPassword;
	protected async Task<User> CreateUserAsync()
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
		
		var passwordHashResult = PasswordManager.HashPassword(Password);

		var user = User.Create(
				UsersTestUtilities.ValidEmail,
				passwordHashResult.PasswordHash,
				passwordHashResult.Salt,
				Roles.Patient,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate.ToUniversalTime(),
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress
			);

		await UserRepository.AddAsync(user);
		await unitOfWork.SaveChangesAsync();

		return user;
	}

	protected async Task<User> CreateUserAsync(string email)
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

		var passwordHashResult = PasswordManager.HashPassword(Password);

		var user = User.Create(
				email,
				passwordHashResult.PasswordHash,
				passwordHashResult.Salt,
				Roles.Patient,
				UsersTestUtilities.ValidFirstName,
				UsersTestUtilities.ValidLastName,
				UsersTestUtilities.PastDate.ToUniversalTime(),
				UsersTestUtilities.ValidPhoneNumber,
				UsersTestUtilities.ValidAdress
			);

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
		var dbContext = ServiceScope.ServiceProvider.GetRequiredService<UsersDBContext>();

		if (dbContext.Users.Any())
		{
			await dbContext.Users.ExecuteDeleteAsync();
		}
	}
}
