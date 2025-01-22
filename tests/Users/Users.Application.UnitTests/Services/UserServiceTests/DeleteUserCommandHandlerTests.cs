using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Users.Application.Commands.Users.DeleteUser;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Services.UserServiceTests
{
	public class DeleteUserCommandHandlerTests
	{
		private readonly DeleteUserCommandHandler _commandHandler;
		private readonly IRepositoryManager _mockRepositoryManager = Substitute.For<IRepositoryManager>();

		private readonly string _existingUserId = "existingUserId";
		private readonly string _nonExistentUserId = "nonExistentUserId";

		private readonly User _testUser;

		public DeleteUserCommandHandlerTests()
		{
			_commandHandler = new DeleteUserCommandHandler(_mockRepositoryManager);

			_testUser = new User(_existingUserId, "test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);
		}

		[Fact]
		public async Task Handle_ShouldReturnSuccess_WhenUserExists()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_existingUserId)
				.Returns(Result<User>.Success(_testUser));
			_mockRepositoryManager.User.DeleteUserAsync(_existingUserId)
				.Returns(Task.CompletedTask);

			var command = new DeleteUserCommand(_existingUserId);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsSuccess.Should().BeTrue();
			_mockRepositoryManager.User.Received(1).DeleteUserAsync(_existingUserId);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenUserDoesNotExist()
		{
			// Arrange
			_mockRepositoryManager.User.GetUserByIdAsync(_nonExistentUserId)
				.Returns(Result<User>.Failure(Responses.UserNotFound));

			var command = new DeleteUserCommand(_nonExistentUserId);

			// Act
			var result = await _commandHandler.Handle(command, CancellationToken.None);

			// Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.UserNotFound);
			_mockRepositoryManager.User.DidNotReceive().DeleteUserAsync(Arg.Any<string>());
		}
	}
}
