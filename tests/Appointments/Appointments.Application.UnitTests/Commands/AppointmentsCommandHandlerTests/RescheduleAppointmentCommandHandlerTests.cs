using Appointments.Application.Commands.Appointments.RescheduleAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;

public class RescheduleAppointmentCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly RescheduleAppointmentCommandHandler _handler;

	public RescheduleAppointmentCommandHandlerTests()
	{
		_handler = new RescheduleAppointmentCommandHandler(
			RepositoryMagager,
			JWTUserExtractor,
			AppointmentCommandHandlerHelper);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentNotFound()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand
		(
			AppointmentsTestUtilities.InvalidId,
			DateTime.UtcNow.AddHours(1),
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.AppointmentNotFound);

		await JWTUserExtractor.DidNotReceiveWithAnyArgs().GetUserIdFromTokenAsync();
		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIdCannotBeExtracted()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand
		(
			AppointmentsTestUtilities.JWTExtractorInternalErrorId,
			DateTime.UtcNow.AddHours(1),
			AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenUserIsNotAuthorized()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.UnauthUserId,
		DateTime.UtcNow.AddHours(1),
		AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.CannotRescheduleOthersAppointment);

		await AppointmentCommandHandlerHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenHelperFailsToCreateAppointment()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.HelperInternalErrorId,
		DateTime.UtcNow.AddHours(1),
		AppointmentDuration.OneHour
		);

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
		var command = new RescheduleAppointmentCommand
		(
		AppointmentsTestUtilities.ValidId,
		DateTime.UtcNow.AddHours(1),
		AppointmentDuration.OneHour
		);

		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await AppointmentCommandHandlerHelper.Received(1).CreateAppointment(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<AppointmentDuration>());

		await RepositoryMagager.Appointment.Received(1)
			.ChangeStatusAsync(default, AppointmentStatus.Rescheduled);
	}
}