using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointmentById;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandlerUnitTests: BaseAppointmentsUnitTest
{
	private readonly GetAppointmentByIdQueryHandler _handler;

	public GetAppointmentByIdQueryHandlerUnitTests()
	{
		_handler = new GetAppointmentByIdQueryHandler(AppointmentRepository, HAMSMapper);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenAppointmentNotFound()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAppointmentByIdQuery(AppointmentsTestUtilities.InvalidId);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.AppointmentNotFound);
	}

	[Fact]
	public async Task Handle_ShouldCallGetByIdAsyncForQuery_WhenQueryIsValid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAppointmentByIdQuery(appointment.Id);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await AppointmentRepository.Received(1).GetByIdAsync(appointment.Id);
	}

	[Fact]
	public async Task Handle_ShouldReturnValidModel_WhenQueryIsValid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAppointmentByIdQuery(appointment.Id);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Match<AppointmentQueryViewModel>(a =>
			a.Id == appointment.Id &&
			a.PatientId == appointment.PatientId &&
			a.DoctorId == appointment.DoctorId &&
			a.Status == appointment.Status &&
			a.Duration == appointment.Duration);
	}
}
