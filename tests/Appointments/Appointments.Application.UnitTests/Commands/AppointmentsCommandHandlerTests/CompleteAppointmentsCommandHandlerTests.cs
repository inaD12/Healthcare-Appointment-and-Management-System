using Appointments.Application.Features.Commands.Appointments.CompleteAppointments;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Responses;
using FluentAssertions;
using NSubstitute;
using Shared.Domain.Results;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;


public class CompleteAppointmentsCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CompleteAppointmentsCommandHandler _handler;

	public CompleteAppointmentsCommandHandlerTests()
	{
		_handler = new CompleteAppointmentsCommandHandler(UnitOfWork, DateTimeProvider, AppointmentRepository);
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
		await AppointmentRepository.Received(1).GetAppointmentsToCompleteAsync(Arg.Any<DateTime>());
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenRepositoryFailsToFetchAppointments()
	{
		// Arrange
		SetupGetAppointmentsToCompleteResult(Result<List<Appointment>>.Failure(ResponseList.InternalError));
		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.InternalError);
		await UnitOfWork.DidNotReceive().SaveChangesAsync();
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
		await UnitOfWork.DidNotReceive().SaveChangesAsync();
	}
}

