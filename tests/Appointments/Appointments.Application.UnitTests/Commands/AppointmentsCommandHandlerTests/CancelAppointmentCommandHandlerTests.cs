using Appointments.Application.Commands.Appointments.CancelAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class CancelAppointmentCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CancelAppointmentCommandHandler _handler;


	public CancelAppointmentCommandHandlerTests()
	{
		_handler = new CancelAppointmentCommandHandler(RepositoryMagager, JWTUserExtractor);
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenCalcelationIsSuccessful()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ValidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken.None);

		//Assert
		result.IsSuccess.Should().BeTrue();

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(AppointmentsTestUtilities.ValidId);
		await JWTUserExtractor.Received(1).GetUserIdFromTokenAsync();
		await RepositoryMagager.Appointment.Received(1).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), Domain.Enums.AppointmentStatus.Cancelled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenAppointmentDoesntExist()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.WrongIdFromTokenId);

		//Act
		var result = await _handler.Handle(command, CancellationToken.None);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.CannotCancelOthersAppointment);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		await JWTUserExtractor.Received(1).GetUserIdFromTokenAsync();
		await RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), Domain.Enums.AppointmentStatus.Cancelled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_AppointmentIsNotFound()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.InvalidId);

		//Act
		var result = await _handler.Handle(command, CancellationToken.None);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.AppointmentNotFound);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		await JWTUserExtractor.Received(0).GetUserIdFromTokenAsync();
		await RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), Domain.Enums.AppointmentStatus.Cancelled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenJWTExtractorError()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.JWTExtractorInternalErrorId);

		//Act
		var result = await _handler.Handle(command, CancellationToken.None);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		await JWTUserExtractor.Received(1).GetUserIdFromTokenAsync();
		await RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Any<Appointment>(), Domain.Enums.AppointmentStatus.Cancelled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenStatusChangingFails()
	{
		//Arrange
		var command = new CancelAppointmentCommand(AppointmentsTestUtilities.ChangeStatusInternalErrorId);

		//Act
		var result = await _handler.Handle(command, CancellationToken.None);

		//Assert
		result.IsFailure.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		await JWTUserExtractor.Received(1).GetUserIdFromTokenAsync();
		await RepositoryMagager.Appointment.Received(1).ChangeStatusAsync(Arg.Any<Appointment>(), Domain.Enums.AppointmentStatus.Cancelled);
	}
}
