using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Shared.Domain.Utilities;
using Shared.Infrastructure.Clock;
using Users.Application.Features.Users.Identity;
using Users.Application.Features.Users.Mappings;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Infrastructure.Models;
using Users.Domain.Responses;
using Users.Domain.Utilities;

public abstract class BaseUsersUnitTest : BaseSharedUnitTest
{
	protected IUserRepository UserRepository { get; }
	protected IIdentityProviderService IdentityProviderService { get; }
	protected IEmailVerificationTokenRepository EmailVerificationTokenRepository { get; }
	protected IDateTimeProvider DateTimeProvider { get; }

	public BaseUsersUnitTest() : base(
		new HAMSMapper(
			new Mapper(
				new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<UserCommandProfile>();
					cfg.AddProfile<UserQueryProfile>();
				}))),
		Substitute.For<IUnitOfWork>())
	{
		IdentityProviderService = Substitute.For<IIdentityProviderService>();
		UserRepository = Substitute.For<IUserRepository>();
		EmailVerificationTokenRepository = Substitute.For<IEmailVerificationTokenRepository>();
		DateTimeProvider = Substitute.For<IDateTimeProvider>();

		DateTimeProvider.UtcNow.Returns(UsersTestUtilities.CurrentDate);
		
		IdentityProviderService.RegisterUserAsync(
			Arg.Any<UserModel>(),
			CancellationToken).Returns(Result<string>.Success(UsersTestUtilities.ValidIdentityId));
	}

	public User GetUser()
	{
		var user = User.Create(
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.EMAIL_MAX_LENGTH, UsersBusinessConfiguration.EMAIL_MIN_LENGTH),
			Roles.Patient,
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH, UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH),
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH, UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH),
			UsersTestUtilities.PastDate,
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.ID_MAX_LENGTH, UsersBusinessConfiguration.ID_MIN_LENGTH),
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress
			);

		var usersList = new List<User> { user };

		var pagedList = new PagedList<User>(
			usersList,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			usersList.Count);

		UserRepository.GetByEmailAsync(user.Email)
		.Returns(user);

		UserRepository.GetByIdAsync(user.Id)
		.Returns(user);

		UserRepository.GetAllAsync(Arg.Is<UserPagedListQuery>(q =>
																				q.Email == user.Email &&
																				q.FirstName == user.FirstName &&
																				q.LastName == user.LastName),
																				CancellationToken)
			.Returns(pagedList);

		IdentityProviderService.RegisterUserAsync(
			new UserModel(user.Email,UsersTestUtilities.ValidPassword , user.FirstName, user.LastName),
			CancellationToken).Returns(Result<string>.Failure(ResponseList.EmailTaken));
		
		return user;
	}

	public EmailVerificationToken GetToken(bool emailVerified = false)
	{
		var user = GetUser();
		if (emailVerified)
			user.VerifyEmail();

		var emailVerificationToken = EmailVerificationToken.Create(
			user.Id,
			UsersTestUtilities.CurrentDate,
			UsersTestUtilities.SoonDate,
			user
			);

		EmailVerificationTokenRepository.GetByIdAsync(emailVerificationToken.Id)
			.Returns(emailVerificationToken);

		return emailVerificationToken;
	}
}
