﻿using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Enums;
using Shared.Domain.Models;
using Shared.Domain.Utilities;
using Shared.Infrastructure.Clock;
using Users.Application.Features.Users.Mappings;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Infrastructure.Models;
using Users.Domain.Utilities;

public abstract class BaseUsersUnitTest : BaseSharedUnitTest
{
	protected IUserRepository UserRepository { get; }
	protected IPasswordManager PasswordManager { get; }
	protected ITokenFactory TokenFactory { get; }
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
		UserRepository = Substitute.For<IUserRepository>();
		PasswordManager = Substitute.For<IPasswordManager>();
		TokenFactory = Substitute.For<ITokenFactory>();
		EmailVerificationTokenRepository = Substitute.For<IEmailVerificationTokenRepository>();
		DateTimeProvider = Substitute.For<IDateTimeProvider>();

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

		DateTimeProvider.UtcNow.Returns(UsersTestUtilities.CurrentDate);
	}
	public User GetUser()
	{
		return GetUser(out _);
	}

	public User GetUser(out string password)
	{
		var user = User.Create(
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.EMAIL_MAX_LENGTH, UsersBusinessConfiguration.EMAIL_MIN_LENGTH),
			UsersTestUtilities.ValidPasswordHash,
			UsersTestUtilities.ValidSalt,
			Roles.Patient,
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH, UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH),
			SharedTestUtilities.GetAverageString(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH, UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH),
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress
			);
		password = UsersTestUtilities.ValidPassword;

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

		PasswordManager.HashPassword(password)
			.Returns(new PasswordHashResult(user.PasswordHash, user.Salt));

		PasswordManager.VerifyPassword(password, user.PasswordHash, user.Salt)
			.Returns(true);

		TokenFactory.CreateToken(user.Id, user.Role)
			.Returns(new TokenResult(UsersTestUtilities.Token));

		return user;
	}

	public EmailVerificationToken GetToken(bool emailVerified = false)
	{
		var user = GetUser(out _);
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
