using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Helpers;

public class AppointmentCommandHandlerHelperTests : BaseAppointmentsUnitTest
{
	private readonly AppointmentCommandHandlerHelper _helper;

	public AppointmentCommandHandlerHelperTests()
	{
		_helper = new AppointmentCommandHandlerHelper(RepositoryMagager, FactoryMagager); 
	
	}
	[Fact]
	public async Task CreateAppointment_ShouldReturnFailure_WhenTimeSlotNotAvailable()
	{
		// Arrange
		var doctorId = AppointmentsTestUtilities.TimeSlotUnavailableId;
		var patientId = AppointmentsTestUtilities.PatientId;
		var startTime = AppointmentsTestUtilities.SoonDate;

		// Act
		var result = await _helper.CreateAppointment(
			doctorId,
			patientId,
			startTime,
			AppointmentDuration.OneHour
			);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.TimeSlotNotAvailable);

		FactoryMagager.Appointment.DidNotReceiveWithAnyArgs().Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());
		await RepositoryMagager.Appointment.DidNotReceiveWithAnyArgs().AddAsync(default);
	}

	[Fact]
	public async Task CreateAppointment_ShouldReturnSuccess_WhenAllStepsSucceed()
	{
		//Arrange
		var doctorId = AppointmentsTestUtilities.DoctorId;
		var patientId = AppointmentsTestUtilities.PatientId;
		var startTime = AppointmentsTestUtilities.SoonDate;

		// Act
		var result = await _helper.CreateAppointment(
			doctorId,
			patientId,
			startTime,
			AppointmentDuration.OneHour
			);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Response.Should().BeEquivalentTo(Responses.AppointmentCreated);

		FactoryMagager.Appointment
			.Received(1).Create(
				patientId,
				doctorId,
				startTime,
				startTime.AddHours(1)
			);

		await RepositoryMagager.Appointment
			.Received(1).AddAsync(Arg.Any<Appointment>());
	}
}
