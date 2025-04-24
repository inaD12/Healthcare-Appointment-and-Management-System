using FluentAssertions;
using NSubstitute;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Domain.Infrastructure.Models;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandlerUnitTests : BaseUsersUnitTest
{
	private readonly GetAllUsersQueryHandler _handler;

	public GetAllUsersQueryHandlerUnitTests()
	{
		_handler = new GetAllUsersQueryHandler(UserRepository, HAMSMapper);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenNoUsersFound()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			UsersTestUtilities.InvalidEmail,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.NoUsersFound);
	}

	[Fact]
	public async Task Handle_ShouldCallGetAllAsyncForQuery_WhenQueryIsValid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository
			.Received(1)
			.GetAllAsync(Arg.Is<UserPagedListQuery>(q =>
																	q.Email == user.Email &&
																	q.Role == user.Role &&
																	q.FirstName == user.FirstName &&
																	q.LastName == user.LastName &&
																	q.PhoneNumber == user.PhoneNumber &&
																	q.Address == user.Address &&
																	q.EmailVerified == user.EmailVerified),
																	CancellationToken);
	}

	[Fact]
	public async Task Handle_ShouldReturnValidModel_WhenQueryIsValid()
	{
		// Arrange
		var user = GetUser();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Role,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.Items.Should().AllSatisfy(ri =>
		{
			ri.Email.Should().Be(user.Email);
			ri.Role.Should().Be(user.Role);
			ri.FirstName.Should().Be(user.FirstName);
			ri.LastName.Should().Be(user.LastName);
			ri.PhoneNumber.Should().Be(user.PhoneNumber);
			ri.Address.Should().Be(user.Address);
			ri.EmailVerified.Should().Be(user.EmailVerified);
		});
	}
}
