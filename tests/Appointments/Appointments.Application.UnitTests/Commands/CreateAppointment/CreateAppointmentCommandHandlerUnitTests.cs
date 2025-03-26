using Appointments.Application.Features.Appointments.Commands.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.CreateAppointment;

public class CreateAppointmentCommandHandlerUnitTests : BaseAppointmentsUnitTest
{
	private readonly CreateAppointmentCommandHandler _handler;

	public CreateAppointmentCommandHandlerUnitTests()
	{
		_handler = new CreateAppointmentCommandHandler(
			HAMSMapper,
			UnitOfWork,
			UserDataRepository,
			AppointmentRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenDoctorNotFound()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.DoctorNotFound);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIsNotADoctor()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.UserIsNotADoctor);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenPatientNotFound()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.PatientNotFound);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenTimeSlotNotAvailable()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		AppointmentRepository.IsTimeSlotAvailableAsync(appointment.DoctorId, Arg.Any<DateTimeRange>()).Returns(false);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.TimeSlotNotAvailable);
	}

	[Fact]
	public async Task Handle_ShouldCallGetUserDataByEmailForPatient_WhenEverythingIsCorrect()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await UserDataRepository.Received(1).GetUserDataByEmailAsync(command.PatientEmail);
	}

	[Fact]
	public async Task Handle_ShouldCallGetUserDataByEmailForDoctor_WhenEverythingIsCorrect()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await UserDataRepository.Received(1).GetUserDataByEmailAsync(command.DoctorEmail);
	}

	[Fact]
	public async Task Handle_ShouldCallIsTimeSlotAvailable_WhenEverythingIsCorrect()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
	public async Task Handle_ShouldCallAddForAppointment_WhenEverythingIsCorrect()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
	public async Task Handle_ShouldCallSaveChanges_WhenEverythingIsCorrect()
	{
		// Arrange
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
		var appointment = GetAppointment();
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
