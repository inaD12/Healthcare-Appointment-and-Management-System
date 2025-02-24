using FluentAssertions;
using NSubstitute;
using Shared.Domain.Results;
using Users.Application.Features.Users.Queries.GetAllDoctors;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Queries.UsersQueriesTests;

public class GetAllDoctorsCommandHandlerTests : BaseUsersUnitTest
{
	private readonly GetAllDoctorsQueryHandler _handler;

	public GetAllDoctorsCommandHandlerTests()
	{
		_handler = new GetAllDoctorsQueryHandler(RepositoryManager);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenDoctorsAreAvailable()
	{
		// Arrange
		var query = new GetAllDoctorsQuery();

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().HaveCount(Doctors.Count);
		result.Value!.Select(d => d.Email).Should().BeEquivalentTo(Doctors.Select(d => d.Email));

		await RepositoryManager.User.Received(1).GetAllDoctorsAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
	{
		// Arrange
		SetupDoctorsResult(Result<IEnumerable<User>>.Failure(Responses.UserNotFound));

		var query = new GetAllDoctorsQuery();

		// Act
		var result = await _handler.Handle(query, CancellationToken.None);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UserNotFound);

		await RepositoryManager.User.Received(1).GetAllDoctorsAsync();
	}
}

