using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetById;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Queries.GetUserById;

public class GetUserByIdQueryHandlerUnitTests : BaseUsersUnitTest
{
	private readonly GetUserByIdQueryHandler _handler;

	public GetUserByIdQueryHandlerUnitTests()
	{
		_handler = new GetUserByIdQueryHandler(UserRepository, HAMSMapper);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
	{
		// Arrange
		var user = GetUser();
		var query = new GetUserByIdQuery(UsersTestUtilities.InvalidId);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Handle_ShouldCallGetByIdAsyncForQuery_WhenQueryIsValid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetUserByIdQuery(user.Id);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository.Received(1).GetByIdAsync(user.Id);
	}

	[Fact]
	public async Task Handle_ShouldReturnValidModel_WhenQueryIsValid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetUserByIdQuery(user.Id);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Match<UserQueryViewModel>(a =>
			a.Id == user.Id &&
			a.Email == user.Email &&
			a.FirstName == user.FirstName &&
			a.LastName == user.LastName &&
			a.PhoneNumber == user.PhoneNumber &&
			a.Address == user.Address &&
			a.EmailVerified == user.EmailVerified);
	}
}
