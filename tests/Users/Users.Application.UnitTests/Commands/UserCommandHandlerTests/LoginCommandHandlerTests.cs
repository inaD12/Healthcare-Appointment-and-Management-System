using Xunit;
using NSubstitute;
using Users.Application.Managers.Interfaces;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Domain.Entities;
using Users.Domain.DTOs.Responses;
using FluentAssertions;
using Contracts.Enums;
using Contracts.Results;
using Users.Domain.Responses;
using Users.Application.Commands.Users.LoginUser;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class LoginCommandHandlerTests
	{
		private readonly LoginUserCommandHandler _commandHandler;
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly IPasswordManager _mockPasswordManager;
		private readonly ITokenManager _mockTokenManager;

		string Email = "test@example.com";
		string Password = "ValidPassword123";
		private readonly User _testUser;
		private readonly TokenDTO _tokenDTO;

		public LoginCommandHandlerTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_mockPasswordManager = Substitute.For<IPasswordManager>();
			_mockTokenManager = Substitute.For<ITokenManager>();

			_commandHandler = new LoginUserCommandHandler(_mockRepositoryManager, _mockPasswordManager, _mockTokenManager);

			_testUser = new User("21212", "test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);

			_tokenDTO = new TokenDTO("someToken");
		}

		[Fact]
		public async Task Handle_ShouldReturnSuccess_WhenLoginIsSuccessful()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(Email)
				.Returns(Result<User>.Success(_testUser));

			_mockPasswordManager.VerifyPassword(Password, _testUser.PasswordHash, _testUser.Salt)
				.Returns(true);

			_mockTokenManager.CreateToken(_testUser.Id)
				.Returns(_tokenDTO);

			var command = new LoginUserCommand<TokenDTO>(Email,Password);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			result.Value.Should().BeEquivalentTo(_tokenDTO);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenPasswordIsIncorrect()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(Email)
				.Returns(Result<User>.Success(_testUser));

			_mockPasswordManager.VerifyPassword(Password, _testUser.PasswordHash, _testUser.Salt)
				.Returns(false);

			var command = new LoginUserCommand<TokenDTO>(Email, Password);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.IncorrectPassword);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByEmailAsync(Email)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			var command = new LoginUserCommand<TokenDTO>(Email, Password);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
		}
	}

}
