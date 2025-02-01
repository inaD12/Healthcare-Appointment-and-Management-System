using Appointments.Application.Appointments.Commands.CompleteAppointments;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;


public class CompleteAppointmentsCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CompleteAppointmentsCommandHandler _handler;

	public CompleteAppointmentsCommandHandlerTests()
	{
		_handler = new CompleteAppointmentsCommandHandler(RepositoryMagager);
	}

	[Fact]
	public async Task Handle_ShouldCompleteAppointments_WhenThereArePendingAppointments()
	{
		// Arrange
		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		SceduledAppointmentList.Select(d => d.Status).Should().AllBeEquivalentTo(AppointmentStatus.Completed);
		await RepositoryMagager.Appointment.Received(1).GetAppointmentsToCompleteAsync(Arg.Any<DateTime>());
		await RepositoryMagager.Appointment.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenRepositoryFailsToFetchAppointments()
	{
		// Arrange
		SetupGetAppointmentsToCompleteResult(Result<List<Appointment>>.Failure(Responses.InternalError));
		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);
		await RepositoryMagager.Appointment.DidNotReceive().SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldDoNothing_WhenNoAppointmentsToComplete()
	{
		// Arrange
		SetupGetAppointmentsToCompleteResult(Result<List<Appointment>>.Success(new List<Appointment>()));
		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await RepositoryMagager.Appointment.DidNotReceive().SaveChangesAsync();
	}
}

