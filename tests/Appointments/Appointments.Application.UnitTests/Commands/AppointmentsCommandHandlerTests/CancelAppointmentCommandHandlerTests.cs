﻿using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;
using Shared.Domain.Responses;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class CancelAppointmentCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CancelAppointmentCommandHandler _handler;


	public CancelAppointmentCommandHandlerTests()
	{
		_handler = new CancelAppointmentCommandHandler(JWTParser, UnitOfWork, DateTimeProvider, AppointmentRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenAppointmentIsNotFound()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.InvalidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotFound);
	}


	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenUserIdIsNotFound()
	{
		//Arrange
		JWTParser.GetIdFromToken().Returns(string.Empty);
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(SharedResponses.JWTNotFound);
	}


	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenIdsDontMatch()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.WrongIdFromTokenId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.CannotCancelOthersAppointment);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenAppointmentIsNotScheduled()
	{
		//Arrange
		AppointmentRepository.GetByIdAsync(Arg.Any<string>()).Returns(AppointmentCanceled);
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotScheduled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenAppointmentAlreadyStarted()
	{
		//Arrange
		DateTimeProvider.UtcNow.Returns(AppointmentsTestUtilities.FutureDate);
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentAlreadyStarted);
	}

	[Fact]
	public async Task Handle_ShouldCallGetById_WhenEverythingIsCorrect()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeTrue();
		await AppointmentRepository.Received(1).GetByIdAsync(AppointmentsTestUtilities.ValidId);
	}

	[Fact]
	public async Task Handle_ShouldCallJWTParser_WhenEverythingIsCorrect()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeTrue();
		JWTParser.Received(1).GetIdFromToken();
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenEverythingIsCorrect()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

}
