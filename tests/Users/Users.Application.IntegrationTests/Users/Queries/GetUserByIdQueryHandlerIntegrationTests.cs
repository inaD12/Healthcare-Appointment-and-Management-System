using FluentAssertions;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetById;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Queries;

public class GetUserByIdQueryHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public GetUserByIdQueryHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenIdIsNull()
	{
		// Arrange
		var query = new GetUserByIdQuery(null!);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var query = new GetUserByIdQuery(SharedTestUtilities.GetString(length));

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenUserNotFound()
	{
		// Arrange
		var query = new GetUserByIdQuery(UsersTestUtilities.InvalidId);

		// Act
		var result = await Sender.Send(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserNotFound);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectModel_WhenUserFound()
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetUserByIdQuery(user.Id);

		// Act
		var result = await Sender.Send(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Match<UserQueryViewModel>(a =>
			a.Id == user.Id &&
			a.FirstName == user.FirstName &&
			a.LastName == user.LastName &&
			a.Email == user.Email &&
			a.PhoneNumber == user.PhoneNumber &&
			a.Address == user.Address &&
			a.Roles.SequenceEqual(user.Roles) &&
			a.EmailVerified == user.EmailVerified
		);
	}
}