using Appointments.Application.Appointments.Commands.CompleteAppointments;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests;


public class CompleteAppointmentsCommandHandlerTests
{
	private readonly IRepositoryManager _mockRepositoryManager;
	private readonly CompleteAppointmentsCommandHandler _handler;

	public CompleteAppointmentsCommandHandlerTests()
	{
		_mockRepositoryManager = Substitute.For<IRepositoryManager>();

		_handler = new CompleteAppointmentsCommandHandler(_mockRepositoryManager);
	}

	[Fact]
	public async Task Handle_ShouldCompleteAppointments_WhenThereArePendingAppointments()
	{
		// Arrange
		var now = DateTime.UtcNow;
		var appointments = new List<Appointment>
	{
			 new Appointment(
				"1",
				"1",
				"1",
				now,
				now.AddMinutes(15),
				AppointmentStatus.Scheduled),
			 new Appointment(
				"2",
				"2",
				"2",
				now,
				now.AddMinutes(15),
				AppointmentStatus.Scheduled)

	};

		var appointmentsResult = Result<List<Appointment>>.Success(appointments);

		_mockRepositoryManager.Appointment
			.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(appointmentsResult);

		_mockRepositoryManager.Appointment.SaveChangesAsync().Returns(Task.CompletedTask);

		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		appointments.Should().AllSatisfy(a => a.Status.Should().Be(AppointmentStatus.Completed));

		await _mockRepositoryManager.Appointment.Received(1).GetAppointmentsToCompleteAsync(Arg.Any<DateTime>());
		await _mockRepositoryManager.Appointment.Received(1).SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenRepositoryFailsToFetchAppointments()
	{
		// Arrange
		var appointmentsResult = Result<List<Appointment>>.Failure(Responses.InternalError);

		_mockRepositoryManager.Appointment
			.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(appointmentsResult);

		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(Responses.InternalError);

		// Verify that SaveChangesAsync was not called
		await _mockRepositoryManager.Appointment.DidNotReceive().SaveChangesAsync();
	}

	[Fact]
	public async Task Handle_ShouldDoNothing_WhenNoAppointmentsToComplete()
	{
		// Arrange
		var emptyAppointments = new List<Appointment>();
		var appointmentsResult = Result<List<Appointment>>.Success(emptyAppointments);

		_mockRepositoryManager.Appointment
			.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(appointmentsResult);

		var command = new CompleteAppointmentsCommand();
		var cancellationToken = CancellationToken.None;

		// Act
		var result = await _handler.Handle(command, cancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();

		await _mockRepositoryManager.Appointment.DidNotReceive().SaveChangesAsync();
	}
}
