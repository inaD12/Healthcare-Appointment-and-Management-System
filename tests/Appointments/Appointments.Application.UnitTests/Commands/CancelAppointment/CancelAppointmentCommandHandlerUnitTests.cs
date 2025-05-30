using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.CancelAppointment;

public class CancelAppointmentCommandHandlerUnitTests : BaseAppointmentsUnitTest
{
	private readonly CancelAppointmentCommandHandler _handler;


	public CancelAppointmentCommandHandlerUnitTests()
	{
		_handler = new CancelAppointmentCommandHandler(UnitOfWork, DateTimeProvider, AppointmentRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenAppointmentIsNotFound()
	{
		//Arrange
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.InvalidId,
			AppointmentsTestUtilities.DoctorId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotFound);
	}


	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenIdsDontMatch()
	{
		//Arrange
		var appointment = GetAppointment();
		var command = new CancelAppointmentCommand(
			appointment.Id,
			AppointmentsTestUtilities.InvalidId);

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
		var appointment = GetAppointment(true);
		var command = new CancelAppointmentCommand(
			appointment.Id,
			appointment.DoctorId);

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
		var appointment = GetAppointment();
		var command = new CancelAppointmentCommand(
			appointment.Id,
			appointment.DoctorId);

		DateTimeProvider.UtcNow.Returns(AppointmentsTestUtilities.FutureDate);

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
		var appointment = GetAppointment();
		var command = new CancelAppointmentCommand(
			appointment.Id,
			appointment.DoctorId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeTrue();
		await AppointmentRepository.Received(1).GetByIdAsync(appointment.Id);
	}


	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenEverythingIsCorrect()
	{
		//Arrange
		var appointment = GetAppointment();
		var command = new CancelAppointmentCommand(
			appointment.Id,
			appointment.DoctorId);

		//Act
		var result = await _handler.Handle(command, CancellationToken);

		//Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

}
