using FluentEmail.Core;
using FluentEmail.Core.Models;
using NSubstitute;
using Shared.Domain.Enums;
using Shared.Domain.Events;
using Shared.Domain.Results;
using Shared.Infrastructure.MessageBroker;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

public abstract class BaseUsersUnitTest
{
	protected IRepositoryManager RepositoryManager { get; }
	protected IFactoryManager FactoryManager { get; }
	protected IPasswordManager PasswordManager { get; }
	protected ITokenManager TokenManager { get; }
	protected IFluentEmail FluentEmail { get; }
	protected IEventBus EventBus { get; }

	protected readonly List<User> Doctors;

	public BaseUsersUnitTest()
	{
		RepositoryManager = Substitute.For<IRepositoryManager>();
		FactoryManager = Substitute.For<IFactoryManager>();
		PasswordManager = Substitute.For<IPasswordManager>();
		TokenManager = Substitute.For<ITokenManager>();
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

		var doctor = new User(
			UsersTestUtilities.ValidId,
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPasswordHash,
			UsersTestUtilities.ValidSalt,
			Roles.Doctor,
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

		Doctors = new List<User> { doctor };

		UserCreatedEvent eveent = FactoryManager.UserCreatedEventFactory.CreateUserCreatedEvent(
					UsersTestUtilities.ValidId,
					UsersTestUtilities.ValidEmail,
					Roles.Patient
					);

		FactoryManager.UserFactory.CreateUser(
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<DateTime>(),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<Roles>()
			).Returns(callInfo =>
			{
				var email = callInfo.ArgAt<string>(0);

				if (email == UsersTestUtilities.EmailSendingErrorEmail)
					return emailErrorUser;
				return user;
			});

		RepositoryManager.User.GetByEmailAsync(
			Arg.Any<string>())
				.Returns(callInfo =>
				{
					var email = callInfo.ArgAt<string>(0);

					if (email == UsersTestUtilities.TakenEmail)
						return Result<User>.Success(takenUser);
					return Result<User>.Failure(Responses.UserNotFound);

				});

		RepositoryManager.User.GetByIdAsync(
			Arg.Any<string>())
				.Returns(callInfo =>
				{
					var id = callInfo.ArgAt<string>(0);

					if (id == UsersTestUtilities.ValidId)
						return Result<User>.Success(user);
					return Result<User>.Failure(Responses.UserNotFound);
				});

		RepositoryManager.User.DeleteByIdAsync(
			Arg.Any<string>())
				.Returns(Result.Success());

		PasswordManager.HashPassword(UsersTestUtilities.ValidPassword, out Arg.Any<string>())
				.Returns(UsersTestUtilities.ValidPasswordHash);

		PasswordManager.VerifyPassword(
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>())
			.Returns(callInfo =>
			{
				var password = callInfo.ArgAt<string>(0);
				return password == UsersTestUtilities.ValidPassword;
			});

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

		FactoryManager.EmailLinkFactory.Create(emailVerificationToken)
			.Returns(UsersTestUtilities.Link);

		string recipientEmail = "";
		FluentEmail.To(Arg.Do<string>(email => recipientEmail = email)).Returns(FluentEmail);
		FluentEmail.Subject(Arg.Any<string>()).Returns(FluentEmail);
		FluentEmail.Body(Arg.Any<string>(), true).Returns(FluentEmail);
		FluentEmail.SendAsync()
			  .Returns(callInfo =>
			  {
				  if (recipientEmail == UsersTestUtilities.EmailSendingErrorEmail)
					  return Task.FromResult(new SendResponse { ErrorMessages = new List<string> { "Email sending failed" } });

				  return Task.FromResult(new SendResponse());
			  });

		RepositoryManager.User.GetAllDoctorsAsync()
		   .Returns(Result<IEnumerable<User>>.Success(Doctors));
	}

	protected void SetupDoctorsResult(Result<IEnumerable<User>> result)
	{
		RepositoryManager.User.GetAllDoctorsAsync().Returns(result);
	}
}
