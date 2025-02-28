using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
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
			JWTParser,
			AppointmentService,
			HAMSMapper,
			UnitOfWork);
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

		JWTParser.DidNotReceiveWithAnyArgs().GetIdFromToken();
		await AppointmentService.DidNotReceiveWithAnyArgs().CreateAppointment(Arg.Any<CreateAppointmentModel>());
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

		await AppointmentService.DidNotReceiveWithAnyArgs().CreateAppointment(Arg.Any<CreateAppointmentModel>());
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

		await AppointmentService.DidNotReceiveWithAnyArgs().CreateAppointment(Arg.Any<CreateAppointmentModel>());
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

		await AppointmentService.Received(1).CreateAppointment(Arg.Any<CreateAppointmentModel>());

		RepositoryMagager.Appointment.Received(1)
			.ChangeStatusAsync(default, AppointmentStatus.Rescheduled);
	}
}