using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;
using Shared.Domain.Responses;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class RescheduleAppointmentCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly RescheduleAppointmentCommandHandler _handler;

	public RescheduleAppointmentCommandHandlerTests()
	{
		_handler = new RescheduleAppointmentCommandHandler(
			JWTParser,
			HAMSMapper,
			UnitOfWork,
			AppointmentRepository,
			DateTimeProvider);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentNotFound()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand
		(
			AppointmentsTestUtilities.InvalidId,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotFound);

		JWTParser.DidNotReceiveWithAnyArgs().GetIdFromToken();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIdIsNull()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(string.Empty);

		var command = new RescheduleAppointmentCommand
		(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(SharedResponses.JWTNotFound);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIsNotAuthorized()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.InvalidId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.CannotRescheduleOthersAppointment);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenTimeSlotNotAvailable()
	{
		// Arrange
		AppointmentRepository.IsTimeSlotAvailableAsync(Arg.Any<string>(), Arg.Any<DateTimeRange>()).Returns(false);
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.TimeSlotNotAvailable);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentNotScheduled()
	{
		// Arrange
		AppointmentRepository.GetAppointmentWithUserDetailsAsync(Arg.Any<string>()).Returns(AppointmentWithDetailsDTOCanceled);
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotScheduled);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentAlreadyStarted()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.FutureDate,
		AppointmentDuration.OneHour
		);

		DateTimeProvider.UtcNow.Returns(command.ScheduledStartTime.AddMinutes(1));

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentAlreadyStarted);
	}

	[Fact]
	public async Task Handle_ShouldCallJWTParser_WhenRequestIsValid()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		JWTParser.Received(1).GetIdFromToken();
	}

	[Fact]
	public async Task Handle_ShouldCallIsTimeSlotAvailableAsync_WhenRequestIsValid()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await AppointmentRepository.Received(1).IsTimeSlotAvailableAsync(
			AppointmentsTestUtilities.DoctorId,
			Arg.Is<DateTimeRange>
			(a => a.Start == command.ScheduledStartTime &&
			a.End == command.ScheduledStartTime.AddMinutes((int)command.Duration)));
	}

	[Fact]
	public async Task Handle_ShouldAddNewAppointment_WhenRequestIsValid()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await AppointmentRepository.Received(1).AddAsync(Arg.Is<Appointment>(a =>
			a.PatientId == AppointmentsTestUtilities.PatientId &&
			a.DoctorId == AppointmentsTestUtilities.DoctorId &&
			a.Duration.Start == command.ScheduledStartTime &&
			a.Duration.End == command.ScheduledStartTime.AddMinutes((int)command.Duration) &&
			a.Status == AppointmentStatus.Scheduled));
	}

	[Fact]
	public async Task Handle_ShouldRescheduleAppointment_WhenAllStepsSucceed()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		Assert.True(Appointment.Status == AppointmentStatus.Rescheduled);
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenAllStepsSucceed()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnValidModel_WhenAllStepsSucceed()
	{
		// Arrange
		JWTParser.GetIdFromToken().Returns(AppointmentsTestUtilities.DoctorId);

		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		AppointmentsTestUtilities.CurrentDate,
		AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		result.Value!.Id.Should().NotBeNullOrEmpty();
		await AppointmentRepository.Received(1).AddAsync(Arg.Is<Appointment>(a =>
			a.Id == result.Value.Id));
	}
}