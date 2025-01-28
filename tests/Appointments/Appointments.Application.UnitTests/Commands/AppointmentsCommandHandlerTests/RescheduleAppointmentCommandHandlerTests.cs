using Appointments.Application.Appoints.Commands.RescheduleAppointment;
using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class RescheduleAppointmentCommandHandlerTests
{
	private readonly IRepositoryManager _mockRepositoryManager;
	private readonly IJWTUserExtractor _mockJwtUserExtractor;
	private readonly IAppointmentCommandHandlerHelper _mockHelper;
	private readonly RescheduleAppointmentCommandHandler _handler;

	private readonly RescheduleAppointmentCommand command;
	private readonly AppointmentWithDetailsDTO appointmentWithDetailsDTO;
	private readonly string AppointmentId;
	private readonly string DoctorId;
	private readonly string PatientId;
	private readonly Appointment Appointment;

	public RescheduleAppointmentCommandHandlerTests()
	{
		_mockRepositoryManager = Substitute.For<IRepositoryManager>();
		_mockJwtUserExtractor = Substitute.For<IJWTUserExtractor>();
		_mockHelper = Substitute.For<IAppointmentCommandHandlerHelper>();

		AppointmentId = "AppId";
		DoctorId = "DocId";
		PatientId = "PatId";
		Appointment = new Appointment(
			AppointmentId,
			PatientId,
			DoctorId,
			DateTime.UtcNow,
			DateTime.UtcNow.AddHours(1),
			AppointmentStatus.Scheduled
			);

		_handler = new RescheduleAppointmentCommandHandler(
			_mockRepositoryManager,
			_mockJwtUserExtractor,
			_mockHelper);

		command = new RescheduleAppointmentCommand
		(
			AppointmentId,
			DateTime.UtcNow.AddHours(1),
			AppointmentDuration.OneHour
		);

		appointmentWithDetailsDTO = new AppointmentWithDetailsDTO
		{
			AppointmentId = AppointmentId,
			ScheduledStartTime = DateTime.UtcNow,
			ScheduledEndTime = DateTime.UtcNow.AddMinutes(60),
			Status = AppointmentStatus.Scheduled,
			DoctorEmail = "DocEmail",
			PatientEmail = "PatEmail",
			DoctorId = DoctorId,
			PatientId = PatientId,
			Appointment = Appointment
		};
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentNotFound()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.GetAppointmentWithUserDetailsAsync(command.AppointmentID)
			.Returns(Result<AppointmentWithDetailsDTO>.Failure(Responses.AppointmentNotFound));

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.AppointmentNotFound);

		await _mockJwtUserExtractor.DidNotReceiveWithAnyArgs().GetUserIdFromTokenAsync();
		await _mockHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIdCannotBeExtracted()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.GetAppointmentWithUserDetailsAsync(command.AppointmentID)
			.Returns(Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTO));

		_mockJwtUserExtractor
			.GetUserIdFromTokenAsync()
			.Returns(Result<string>.Failure(Responses.InternalError));

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		await _mockHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIsNotAuthorized()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.GetAppointmentWithUserDetailsAsync(command.AppointmentID)
			.Returns(Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTO));

		var unauthorizedUserId = "UnaothUser";
		_mockJwtUserExtractor
			.GetUserIdFromTokenAsync()
			.Returns(Result<string>.Success(unauthorizedUserId));

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.CannotRescheduleOthersAppointment);

		await _mockHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenHelperFailsToCreateAppointment()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.GetAppointmentWithUserDetailsAsync(command.AppointmentID)
			.Returns(Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTO));

		_mockJwtUserExtractor
			.GetUserIdFromTokenAsync()
			.Returns(Result<string>.Success(DoctorId));

		_mockHelper
			.CreateAppointment(
			appointmentWithDetailsDTO.DoctorEmail,
			appointmentWithDetailsDTO.PatientEmail,
			command.ScheduledStartTime.ToUniversalTime(),
			command.Duration)
			.Returns(Result.Failure(Responses.InternalError));

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);
	}

	[Fact]
	public async Task Handle_ShouldRescheduleAppointment_WhenAllStepsSucceed()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.GetAppointmentWithUserDetailsAsync(command.AppointmentID)
			.Returns(Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTO));

		_mockJwtUserExtractor
			.GetUserIdFromTokenAsync()
			.Returns(Result<string>.Success(DoctorId));

		_mockHelper
			.CreateAppointment(
			appointmentWithDetailsDTO.DoctorEmail,
			appointmentWithDetailsDTO.PatientEmail,
			command.ScheduledStartTime.ToUniversalTime(),
			command.Duration)
			.Returns(Result.Success());

		_mockRepositoryManager.Appointment
			.ChangeStatusAsync(Appointment, AppointmentStatus.Rescheduled)
			.Returns(Result.Success());

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await _mockHelper.Received(1).CreateAppointment(
			appointmentWithDetailsDTO.DoctorEmail,
			appointmentWithDetailsDTO.PatientEmail,
			command.ScheduledStartTime.ToUniversalTime(),
			command.Duration);

		await _mockRepositoryManager.Appointment.Received(1)
			.ChangeStatusAsync(Appointment, AppointmentStatus.Rescheduled);
	}
}