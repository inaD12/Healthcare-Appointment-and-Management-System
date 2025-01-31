using Contracts.Enums;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using Users.Application.Managers.Interfaces;
using Users.Application.Queries.Users.GetAllDoctors;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Xunit;

namespace Users.Application.UnitTests.Queries.UsersQueriesTests;

public class GetAllDoctorsCommandHandlerTests
{
	private readonly IRepositoryManager _mockRepositoryManager;
	private readonly GetAllDoctorsQueryHandler _handler;

	private readonly User _user;
	private readonly User _user2;
	private readonly List<User> _users;

	public GetAllDoctorsCommandHandlerTests()
	{
		_mockRepositoryManager = Substitute.For<IRepositoryManager>();
		_handler = new GetAllDoctorsQueryHandler(_mockRepositoryManager);

		_user = new User("21212", "test@example.com", "hashedPassword", "salt", Roles.Patient, "John", "Doe", DateTime.UtcNow, "1234567890", "Address", true);
		_user2 = new User("21212", "test@exddample.com", "hashedPassword", "salt", Roles.Patient, "Bob", "Doe", DateTime.UtcNow, "1234567890", "Address", true);
		_users = new List<User>
		{
			_user,
			_user2,
		};
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenDoctorsAreAvailable()
	{
		//Arrange
		_mockRepositoryManager.User.GetAllDoctorsAsync()
			.Returns(Result<IEnumerable<User>>.Success(_users));

		var query = new GetAllDoctorsQuery();

		//Act
		var result = await _handler.Handle(query, CancellationToken.None);

		//Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().HaveCount(_users.Count);
		result.Value.Select(d => d.firstName).Should().BeEquivalentTo(_users.Select(d => d.FirstName));

		await _mockRepositoryManager.User.Received(1).GetAllDoctorsAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenReposotoryFails()
	{
		//Arrange
		_mockRepositoryManager.User.GetAllDoctorsAsync()
			.Returns(Result<IEnumerable<User>>.Failure(Responses.UserNotFound));

		var query = new GetAllDoctorsQuery();

		//Act
		var result = await _handler.Handle(query, CancellationToken.None);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.UserNotFound);

		await _mockRepositoryManager.User.Received(1).GetAllDoctorsAsync();
	}
}
