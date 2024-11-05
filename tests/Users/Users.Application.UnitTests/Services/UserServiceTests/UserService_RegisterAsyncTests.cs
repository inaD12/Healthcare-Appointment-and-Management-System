﻿using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Application.Services;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Entities;
using Users.Domain.Result;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class UserService_RegisterAsyncTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly IPasswordManager _mockPasswordManager;
		private readonly ITokenManager _mockTokenManager;
		private readonly IEmailVerificationSender _mockEmailVerificationSender;
		private readonly IFactoryManager _mockFactoryManager;

		private readonly RegisterReqDTO _registerReqDTO;
		private readonly UserService _userService;
		private readonly User _user;

		private string _salt = "salt";
		private readonly string _hashedPassword = "hashedPassword";

		public UserService_RegisterAsyncTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_mockPasswordManager = Substitute.For<IPasswordManager>();
			_mockTokenManager = Substitute.For<ITokenManager>();
			_mockEmailVerificationSender = Substitute.For<IEmailVerificationSender>();
			_mockFactoryManager = Substitute.For<IFactoryManager>();

			_userService = new UserService(
				_mockPasswordManager,
				_mockTokenManager,
				_mockRepositoryManager,
				_mockEmailVerificationSender,
				_mockFactoryManager
			);

			_registerReqDTO = new RegisterReqDTO
			{
				Email = "newuser@example.com",
				FirstName = "John",
				LastName = "Doe",
				DateOfBirth = DateTime.UtcNow.AddYears(-30),
				PhoneNumber = "1234567890",
				Address = "123 Main St",
				Password = "Password"
			};

			_user = new User("1", _registerReqDTO.Email, _hashedPassword, _salt, "UserRole", _registerReqDTO.FirstName, _registerReqDTO.LastName, _registerReqDTO.DateOfBirth, _registerReqDTO.PhoneNumber, _registerReqDTO.Address, true);
		}


		[Fact]
		public async Task RegisterAsync_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerReqDTO.Email)
				.Returns(Result<User>.Failure(Response.UserNotFound));

			_mockPasswordManager.HashPassword(_registerReqDTO.Password, out _salt).Returns(_hashedPassword);

			_mockFactoryManager.UserFactory.CreateUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<string>(), Arg.Any<string>())
				.Returns(_user);

			_mockEmailVerificationSender.SendEmailAsync(_user).Returns(Result.Success(Response.RegistrationSuccessful));

			// Act
			var result = await _userService.RegisterAsync(_registerReqDTO);

			// Assert
			result.IsSuccess.Should().BeTrue();
		}

		[Fact]
		public async Task RegisterAsync_ShouldReturnFailure_WhenEmailIsTaken()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerReqDTO.Email)
				.Returns(Result<User>.Success(_user));

			// Act
			var result = await _userService.RegisterAsync(_registerReqDTO);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.EmailTaken);
		}

		[Fact]
		public async Task RegisterAsync_ShouldReturnFailure_WhenEmailSendingFails()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerReqDTO.Email)
				.Returns(Result<User>.Failure(Response.UserNotFound)); // Email is not taken

			_mockPasswordManager.HashPassword(_registerReqDTO.Password, out _salt).Returns(_hashedPassword);

			_mockFactoryManager.UserFactory.CreateUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<string>(), Arg.Any<string>())
				.Returns(_user);

			_mockEmailVerificationSender.SendEmailAsync(_user).Returns(Result.Failure(Response.InternalError));

			// Act
			var result = await _userService.RegisterAsync(_registerReqDTO);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InternalError);
		}

		[Fact]
		public async Task RegisterAsync_ShouldReturnFailure_WhenExceptionOccurs()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(_registerReqDTO.Email)
				.Throws(new Exception("Some error"));

			// Act
			var result = await _userService.RegisterAsync(_registerReqDTO);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Response.InternalError);
		}
	}
}