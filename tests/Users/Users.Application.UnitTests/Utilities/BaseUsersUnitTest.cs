using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Users.Mappings;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

public abstract class BaseUsersUnitTest : BaseSharedUnitTest
{
	protected IEmailVerificationLinkFactory EmailLinkFactory { get; }
	protected IEmailVerificationTokenFactory EmailVerificationTokenFactory { get; }
	protected IUserRepository UserRepository { get; }
	protected IPasswordManager PasswordManager { get; }
	protected ITokenFactory TokenManager { get; }
	protected IEventBus EventBus { get; }
	protected IEmailConfirmationTokenPublisher EmailConfirmationTokenPublisher { get; }

	protected readonly List<User> Doctors;

	public BaseUsersUnitTest() : base(
		new HAMSMapper(
			new Mapper(
				new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<UserCommandProfile>();
				}))),
		Substitute.For<IUnitOfWork>())
	{
		EmailLinkFactory = Substitute.For<IEmailVerificationLinkFactory>();
		EmailVerificationTokenFactory = Substitute.For<IEmailVerificationTokenFactory>();
		UserRepository = Substitute.For<IUserRepository>();
		PasswordManager = Substitute.For<IPasswordManager>();
		TokenManager = Substitute.For<ITokenFactory>();
		EventBus = Substitute.For<IEventBus>();
		EmailConfirmationTokenPublisher = Substitute.For<IEmailConfirmationTokenPublisher>();

		var user = new User(
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
			)
		{
			Id = UsersTestUtilities.ValidId
		};

		var doctor = new User(
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
			)
		{
			Id = UsersTestUtilities.ValidId
		};


		var takenUser = new User(
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
			)
		{
			Id = UsersTestUtilities.TakenId
		};

		var emailErrorUser = new User(
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
					)
		{
			Id = UsersTestUtilities.ValidId
		};


		var emailVerificationToken = new EmailVerificationToken(
			UsersTestUtilities.TakenId,
			UsersTestUtilities.CurrentDate,
			UsersTestUtilities.SoonDate,
			takenUser
			)
		{
			Id = UsersTestUtilities.ValidId
		};

		Doctors = new List<User> { doctor };

		//FactoryManager.UserFactory.CreateUser(
		//	Arg.Any<string>(),
		//	Arg.Any<string>(),
		//	Arg.Any<string>(),
		//	Arg.Any<string>(),
		//	Arg.Any<string>(),
		//	Arg.Any<DateTime>(),
		//	Arg.Any<string>(),
		//	Arg.Any<string>(),
		//	Arg.Any<Roles>()
		//	).Returns(callInfo =>
		//	{
		//		var email = callInfo.ArgAt<string>(0);

		//		if (email == UsersTestUtilities.EmailSendingErrorEmail)
		//			return emailErrorUser;
		//		return user;
		//	});

		UserRepository.GetByEmailAsync(
			Arg.Any<string>())
				.Returns(callInfo =>
				{
					var email = callInfo.ArgAt<string>(0);

					if (email == UsersTestUtilities.TakenEmail)
						return Result<User>.Success(takenUser);
					return Result<User>.Failure(Responses.UserNotFound);

				});

		UserRepository.GetByIdAsync(
			Arg.Any<string>())
				.Returns(callInfo =>
				{
					var id = callInfo.ArgAt<string>(0);

					if (id == UsersTestUtilities.ValidId)
						return Result<User>.Success(user);
					return Result<User>.Failure(Responses.UserNotFound);
				});

		UserRepository.DeleteByIdAsync(
			Arg.Any<string>())
				.Returns(Result.Success());

		PasswordManager.HashPassword(UsersTestUtilities.ValidPassword)
				.Returns(new PasswordHashResult(UsersTestUtilities.ValidPasswordHash, UsersTestUtilities.ValidSalt));

		PasswordManager.VerifyPassword(
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<string>())
			.Returns(callInfo =>
			{
				var password = callInfo.ArgAt<string>(0);
				return password == UsersTestUtilities.ValidPassword;
			});

		EventBus.PublishAsync(Arg.Any<string>, CancellationToken.None)
			.Returns(Task.CompletedTask);

		TokenManager.CreateToken(Arg.Any<string>())
			.Returns(new TokenResult(UsersTestUtilities.ValidId));

		EmailVerificationTokenFactory.CreateToken(
			UsersTestUtilities.TakenId,
			Arg.Any<DateTime>(),
			Arg.Any<DateTime>()
			)
		  .Returns(emailVerificationToken);

		EmailLinkFactory.Create(emailVerificationToken)
			.Returns(UsersTestUtilities.Link);

		string recipientEmail = "";
		//FluentEmail.To(Arg.Do<string>(email => recipientEmail = email)).Returns(FluentEmail);
		//FluentEmail.Subject(Arg.Any<string>()).Returns(FluentEmail);
		//FluentEmail.Body(Arg.Any<string>(), true).Returns(FluentEmail);
		//FluentEmail.SendAsync()
		//	  .Returns(callInfo =>
		//	  {
		//		  if (recipientEmail == UsersTestUtilities.EmailSendingErrorEmail)
		//			  return Task.FromResult(new SendResponse { ErrorMessages = new List<string> { "Email sending failed" } });

		//		  return Task.FromResult(new SendResponse());
		//	  });

	}
}
