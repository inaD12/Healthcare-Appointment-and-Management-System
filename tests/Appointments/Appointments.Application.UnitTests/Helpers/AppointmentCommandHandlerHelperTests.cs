using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Helpers;

public class AppointmentCommandHandlerHelperTests
{
	private readonly IRepositoryManager _mockRepositoryManager;
	private readonly IFactoryManager _mockFactoryManager;
	private readonly AppointmentCommandHandlerHelper _helper;

	private readonly string DoctorId;
	private readonly string PatientId;
	private readonly static DateTime StartTime;
	private readonly static AppointmentDuration Duration;
	private readonly DateTime EndTime;
	private readonly string AppointmentId;
	private readonly Appointment Appointment;

	public AppointmentCommandHandlerHelperTests()
	{
		_mockRepositoryManager = Substitute.For<IRepositoryManager>();
		_mockFactoryManager = Substitute.For<IFactoryManager>();

		_helper = new AppointmentCommandHandlerHelper(_mockRepositoryManager, _mockFactoryManager);

		DoctorId = Guid.NewGuid().ToString();
		PatientId = Guid.NewGuid().ToString();
		DateTime StartTime = DateTime.UtcNow.AddHours(1);
		AppointmentDuration Duration = AppointmentDuration.ThirtyMinutes;
		DateTime EndTime = StartTime.AddMinutes((int)Duration);
		AppointmentId = "AppId";
		Appointment = new Appointment(
			AppointmentId,
			PatientId,
			DoctorId,
			StartTime,
			EndTime,
			AppointmentStatus.Scheduled
			);
	}

	[Fact]
	public async Task CreateAppointment_ShouldReturnFailure_WhenIsTimeSlotAvailableFails()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.IsTimeSlotAvailableAsync(DoctorId, StartTime, StartTime.AddMinutes((int)Duration))
			.Returns(Result<bool>.Failure(Responses.InternalError));

		// Act
		var result = await _helper.CreateAppointment(DoctorId, PatientId, StartTime, Duration);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		_mockFactoryManager.Appointment.DidNotReceiveWithAnyArgs().Create(default, default, default, default);
		await _mockRepositoryManager.Appointment.DidNotReceiveWithAnyArgs().AddAsync(default);
	}

	[Fact]
	public async Task CreateAppointment_ShouldReturnFailure_WhenTimeSlotNotAvailable()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.IsTimeSlotAvailableAsync(DoctorId, StartTime, StartTime.AddMinutes((int)Duration))
			.Returns(Result<bool>.Success(false));

		// Act
		var result = await _helper.CreateAppointment(DoctorId, PatientId, StartTime, Duration);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.TimeSlotNotAvailable);

		_mockFactoryManager.Appointment.DidNotReceiveWithAnyArgs().Create(default, default, default, default);
		await _mockRepositoryManager.Appointment.DidNotReceiveWithAnyArgs().AddAsync(default);
	}

	[Fact]
	public async Task CreateAppointment_ShouldReturnSuccess_WhenAllStepsSucceed()
	{
		// Arrange
		_mockRepositoryManager.Appointment
			.IsTimeSlotAvailableAsync(DoctorId, StartTime, EndTime)
			.Returns(Result<bool>.Success(true));

		_mockFactoryManager.Appointment
			.Create(PatientId, DoctorId, StartTime, EndTime)
			.Returns(Appointment);

		// Act
		var result = await _helper.CreateAppointment(DoctorId, PatientId, StartTime, Duration);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.AppointmentCreated);

		_mockFactoryManager.Appointment.Received(1).Create(PatientId, DoctorId, StartTime, EndTime);
		await _mockRepositoryManager.Appointment.Received(1).AddAsync(Appointment);
	}
}
