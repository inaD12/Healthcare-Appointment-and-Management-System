using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
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
		_handler = new CancelAppointmentCommandHandler(RepositoryMagager, JWTParser, UnitOfWork);
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
		JWTParser.Received(1).GetIdFromToken();
		RepositoryMagager.Appointment.Received(1).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), AppointmentStatus.Cancelled);
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
		result.Response.Should().BeEquivalentTo(ResponseList.CannotCancelOthersAppointment);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		JWTParser.Received(1).GetIdFromToken();
		RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), AppointmentStatus.Cancelled);
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
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotFound);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		JWTParser.Received(0).GetIdFromToken();
		RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Is<Appointment>(a => a.Id == command.AppointmentId), AppointmentStatus.Cancelled);
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
		result.Response.Should().BeEquivalentTo(ResponseList.InternalError);

		await RepositoryMagager.Appointment.Received(1).GetByIdAsync(command.AppointmentId);
		JWTParser.Received(1).GetIdFromToken();
		RepositoryMagager.Appointment.Received(0).ChangeStatusAsync(Arg.Any<Appointment>(), AppointmentStatus.Cancelled);
	}
}
