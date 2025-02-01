using Appointments.Application.Appoints.Commands.CreateAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class CreateAppointmentCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CreateAppointmentCommandHandler _handler;

	public CreateAppointmentCommandHandlerTests()
	{
		_handler = new CreateAppointmentCommandHandler(RepositoryMagager, AppointmentCommandHandlerHelper);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenDoctorNotFound()
	{
		// Arrange
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.DoctorNotFound);

		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIsNotADoctor()
	{
		// Arrange
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.UserIsNotADoctor);

		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenPatientNotFound()
	{
		// Arrange
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.PatientNotFound);

		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldCallHelperAndReturnResult_WhenDoctorAndPatientExist()
	{
		// Arrange
		var command = new CreateAppointmentCommand
		(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.CurrentDate,
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await AppointmentCommandHandlerHelper.Received(1).CreateAppointment(
			AppointmentsTestUtilities.DoctorId,
			AppointmentsTestUtilities.PatientId,
			Arg.Any<DateTime>(),
			Arg.Any<AppointmentDuration>()
		);
	}
}
