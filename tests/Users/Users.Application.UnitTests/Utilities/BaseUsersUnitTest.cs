using Contracts.Enums;
using Contracts.Events;
using Contracts.Results;
using FluentEmail.Core;
using NSubstitute;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Users.Infrastructure.MessageBroker;

public abstract class BaseUsersUnitTest
{
	protected IRepositoryManager RepositoryManager { get; }
	protected IFactoryManager FactoryManager { get; }
	protected IPasswordManager PasswordManager { get; }
	protected ITokenManager TokenManager { get; }
	protected IEmailVerificationSender EmailVerificationSender { get; }
	protected IFluentEmail FluentEmail { get; }
	protected IEventBus EventBus { get; }

	public BaseUsersUnitTest()
	{
		RepositoryManager = Substitute.For<IRepositoryManager>();
		FactoryManager = Substitute.For<IFactoryManager>();
		PasswordManager = Substitute.For<IPasswordManager>();
		TokenManager = Substitute.For<ITokenManager>();
		EmailVerificationSender = Substitute.For<IEmailVerificationSender>();
		FluentEmail = Substitute.For<IFluentEmail>();
		EventBus = Substitute.For<IEventBus>();

		var user = new User(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPasswordHash,
			UsersTestUtilities.ValidSalt,
			Roles.Patient,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			false
			);

		var takenUser = new User(
			UsersTestUtilities.TakenId,
			UsersTestUtilities.TakenEmail,
			UsersTestUtilities.ValidPasswordHash,
			UsersTestUtilities.ValidSalt,
			Roles.Patient,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			false
			);

		var emailErrorUser = new User(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.EmailSendingErrorEmail,
			UsersTestUtilities.ValidPasswordHash,
			UsersTestUtilities.ValidSalt,
			Roles.Patient,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			false
			);

		var emailVerificationToken = new EmailVerificationToken(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.TakenId,
			UsersTestUtilities.CurrentDate,
			UsersTestUtilities.SoonDate,
			takenUser
			);

		UserCreatedEvent eveent = FactoryManager.UserCreatedEventFactory.CreateUserCreatedEvent(
					UsersTestUtilities.ValidId,
					UsersTestUtilities.ValidEmail,
					Roles.Patient
					);

		FactoryManager.UserFactory.CreateUser(
			UsersTestUtilities.UnusedEmail,
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<DateTime>(),
			Arg.Any<int>(),
			Arg.Any<string>(),
			Arg.Any<Roles>()
			).Returns(user);

		FactoryManager.UserFactory.CreateUser(
			UsersTestUtilities.EmailSendingErrorEmail,
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<DateTime>(),
			Arg.Any<int>(),
			Arg.Any<string>(),
			Arg.Any<Roles>()
			).Returns(emailErrorUser);


		RepositoryManager.User.GetUserByEmailAsync(
			UsersTestUtilities.TakenEmail)
				.Returns(Result<User>.Success(takenUser));

		RepositoryManager.User.GetUserByEmailAsync(
			UsersTestUtilities.EmailSendingErrorEmail)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

		RepositoryManager.User.GetUserByEmailAsync(
			UsersTestUtilities.UnusedEmail)
				.Returns(Result<User>.Failure(Responses.UserNotFound));


		RepositoryManager.User.GetUserByIdAsync(
			UsersTestUtilities.ValidId)
				.Returns(Result<User>.Success(user));

		RepositoryManager.User.GetUserByIdAsync(
			UsersTestUtilities.InvalidId)
				.Returns(Result<User>.Failure(Responses.UserNotFound));


		PasswordManager.HashPassword(UsersTestUtilities.ValidPassword, out Arg.Any<string>())
				.Returns(UsersTestUtilities.ValidPasswordHash);


		PasswordManager.VerifyPassword(UsersTestUtilities.InvalidPassword, Arg.Any<string>(), Arg.Any<string>())
				 .Returns(false);

		PasswordManager.VerifyPassword(UsersTestUtilities.ValidPassword, Arg.Any<string>(), Arg.Any<string>())
				 .Returns(true);


		EmailVerificationSender.SendEmailAsync(user)
				.Returns(Result.Success(Responses.RegistrationSuccessful));

		EmailVerificationSender.SendEmailAsync(emailErrorUser)
				.Returns(Result.Failure(Responses.InternalError));


		EventBus.PublishAsync(eveent, CancellationToken.None)
			.Returns(Task.CompletedTask);


		TokenManager.CreateToken(Arg.Any<string>())
			.Returns(UsersTestUtilities.TokenDTO);

		FactoryManager.EmailTokenFactory.CreateToken(
			Arg.Any<string>(),
			UsersTestUtilities.TakenId,
			Arg.Any<DateTime>(),
			Arg.Any<DateTime>()
			)
		  .Returns(emailVerificationToken);
	}
}
