using FluentAssertions;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Application.IntegrationTests.Utilities;
using Users.Domain.Responses;
using Users.Domain.Utilities;

namespace Users.Application.IntegrationTests.Users.Queries;

public class GetAllUsersQueryHandlerIntegrationTests : BaseUsersIntegrationTest
{
	public GetAllUsersQueryHandlerIntegrationTests(UsersIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.EMAIL_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.EMAIL_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenEmailLengthIsInvalid(int length)
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			SharedTestUtilities.GetString(length),
			user.Roles.First(),
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenRoleIsInvalid()
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			UsersTestUtilities.InvalidRole,
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenFirstNameLengthIsInvalid(int length)
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			SharedTestUtilities.GetString(length),
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenLastNameLengthIsInvalid(int length)
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			user.FirstName,
			SharedTestUtilities.GetString(length),
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenPhoneNumberLengthIsInvalid(int length)
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			user.FirstName,
			user.LastName,
			SharedTestUtilities.GetString(length),
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(UsersBusinessConfiguration.ADRESS_MIN_LENGTH - 1)]
	[InlineData(UsersBusinessConfiguration.ADRESS_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenAddressLengthIsInvalid(int length)
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			SharedTestUtilities.GetString(length),
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenSortPropertyNameIsInvalid()
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.InvalidSortPropertyName
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenSortPropertyNameIsNull()
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			user.Roles.First(),
			user.FirstName,
			user.LastName,
			user.PhoneNumber,
			user.Address,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			null!
		);

		// Act
		var action = async () => await Sender.Send(query, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenNoUsersFound()
	{
		// Arrange
		var query = new GetAllUsersQuery(
			UsersTestUtilities.ValidEmail,
			Role.Patient,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			false,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeFalse();
		response.Response.Should().BeEquivalentTo(ResponseList.NoUsersFound);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyEmailIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var query = new GetAllUsersQuery(
			user.Email,
			null!,
			null!,
			null!,
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyRoleIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			user.Roles.First(),
			null!,
			null!,
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyFirstNameIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			user.FirstName,
			null!,
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyLastNameIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			user.LastName,
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyPhoneNumberIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			null!,
			user.PhoneNumber,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyAddressIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			null!,
			null!,
			user.Address,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndOnlyEmailVerifiedIsSearched()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			null!,
			null!,
			null!,
			user.EmailVerified,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndFirstNameIsNotNulll()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			SharedTestUtilities.GetHalfStartString(user.FirstName),
			null!,
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}

	[Fact]
	public async Task Send_ShouldReturnCorrectResponse_WhenUsersExistAndLastNameIsNotNulll()
	{
		// Arrange
		var user = await CreateUserAsync();
		var user2 = await CreateUserAsync(UsersTestUtilities.ValidAddEmail);
		var query = new GetAllUsersQuery(
			null!,
			null!,
			null!,
			SharedTestUtilities.GetHalfStartString(user.LastName),
			null!,
			null!,
			null!,
			UsersTestUtilities.ValidSortOrderProperty,
			UsersTestUtilities.ValidPageValue,
			UsersTestUtilities.ValidPageSizeValue,
			UsersTestUtilities.ValidSortPropertyName
		);

		// Act
		var response = await Sender.Send(query, CancellationToken);

		// Assert
		response.IsSuccess.Should().BeTrue();
		response.Value.Should().NotBeNull();
		response.Value.Items.Should().Contain(c =>
											 c.Id == user.Id &&
											 c.FirstName == user.FirstName &&
											 c.LastName == user.LastName &&
											 c.Email == user.Email &&
											 c.Roles.SequenceEqual(user.Roles) &&
											 c.Address == user.Address &&
											 c.PhoneNumber == user.PhoneNumber)
									.And.Contain(c =>
											 c.Id == user2.Id &&
											 c.FirstName == user2.FirstName &&
											 c.LastName == user2.LastName &&
											 c.Email == user2.Email &&
											 c.Roles.SequenceEqual(user2.Roles) &&
											 c.Address == user2.Address &&
											 c.PhoneNumber == user2.PhoneNumber);
	}
}
