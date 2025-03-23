using Appointments.Application.Features.Commands.Appointments.CompleteAppointments;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;


public class CompleteAppointmentsCommandHandlerTests : BaseAppointmentsUnitTest
{
	private readonly CompleteAppointmentsCommandHandler _handler;

	public CompleteAppointmentsCommandHandlerTests()
	{
		_handler = new CompleteAppointmentsCommandHandler(UnitOfWork, DateTimeProvider, AppointmentRepository);
	}

	[Fact]
	public async Task Handle_ShouldNotSaveChanges_WhenNoAppointmentsToComplete()
	{
		// Arrange
		AppointmentRepository.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>()).Returns(new List<Appointment>());
		var command = new CompleteAppointmentsCommand();

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(0).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldCompleteAppointments_WhenThereArePendingAppointments()
	{
		// Arrange
		var command = new CompleteAppointmentsCommand();

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		ScheduledAppointmentList.Select(d => d.Status).Should().AllBeEquivalentTo(AppointmentStatus.Completed);
	}

	[Fact]
	public async Task Handle_ShouldCallSaveChanges_WhenThereArePendingAppointments()
	{
		// Arrange
		var command = new CompleteAppointmentsCommand();

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await UnitOfWork.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldCallGetAppointmentsToCompleteAsync()
	{
		// Arrange
		var command = new CompleteAppointmentsCommand();

		// Act
		var result = await _handler.Handle(command, CancellationToken);

		// Assert
		await AppointmentRepository.Received(1).GetAppointmentsToCompleteAsync(Arg.Any<DateTime>());
	}
}

