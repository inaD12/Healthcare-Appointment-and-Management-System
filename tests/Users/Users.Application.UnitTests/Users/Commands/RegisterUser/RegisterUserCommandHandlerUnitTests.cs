﻿using FluentAssertions;
using NSubstitute;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Domain.Utilities;
using Xunit;

namespace Users.Application.UnitTests.Users.Commands.RegisterUser;

public class RegisterUserCommandHandlerTests : BaseUsersUnitTest
{
	private readonly RegisterUserCommandHandler _commandHandler;

	public RegisterUserCommandHandlerTests()
	{
		_commandHandler = new RegisterUserCommandHandler(
			PasswordManager,
			HAMSMapper,
			UnitOfWork,
			UserRepository
			);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenEmailIsTaken()
	{
		// Arrange
		var existingUser = GetUser();
		var command = new RegisterUserCommand(
			existingUser.Email,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.EmailTaken);
	}

	[Fact]
	public async Task Handle_ShouldCallHashPassword_WhenModelIsCorrect()
	{
		//Arramge
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		PasswordManager.Received(1).HashPassword(command.Password);
	}

	[Fact]
	public async Task Handle_ShouldCallAddAsync_WhenModelIsCorrect()
	{
		//Arramge
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UserRepository.Received(1).AddAsync(Arg.Is<User>(u =>
			u.Email == command.Email &&
			u.FirstName == command.FirstName &&
			u.LastName == command.LastName &&
			u.DateOfBirth == command.DateOfBirth &&
			u.PhoneNumber == command.PhoneNumber &&
			u.Address == command.Address &&
			u.Role == command.Role));	
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenModelIsCorrect()
	{
		//Arramge
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnCorrectModel_WhenRegistrationIsSuccessful()
	{
		//Arramge
		var command = new RegisterUserCommand(
			UsersTestUtilities.ValidEmail,
			UsersTestUtilities.ValidPassword,
			UsersTestUtilities.ValidFirstName,
			UsersTestUtilities.ValidLastName,
			UsersTestUtilities.PastDate,
			UsersTestUtilities.ValidPhoneNumber,
			UsersTestUtilities.ValidAdress,
			Roles.Patient
		);

		// Act
		var result = await _commandHandler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.Id.Should().NotBeNullOrEmpty();
		await UserRepository.Received(1).AddAsync(Arg.Is<User>(a =>
			a.Id == result.Value!.Id));
	}
}
