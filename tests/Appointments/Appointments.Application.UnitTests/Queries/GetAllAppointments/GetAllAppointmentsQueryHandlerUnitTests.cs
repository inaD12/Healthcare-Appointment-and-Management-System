using Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Queries.GetAllAppointments;

public class GetAllAppointmentsQueryHandlerUnitTests : BaseAppointmentsUnitTest
{
	private readonly GetAllAppointmentsQueryHandler _handler;

	public GetAllAppointmentsQueryHandlerUnitTests()
	{
		_handler = new GetAllAppointmentsQueryHandler(HAMSMapper, AppointmentRepository);
	}

	[Fact]
	public async Task Handle_ShouldReturnFaliure_WhenNoAppointmentsFound()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			AppointmentsTestUtilities.InvalidId,
			appointment.DoctorId,
			appointment.Status,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().BeEquivalentTo(ResponseList.NoAppointmentsFound);
	}

	[Fact]
	public async Task Handle_ShouldCallGetAllAsyncForQuery_WhenQueryIsValid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			appointment.Status,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		await AppointmentRepository
			.Received(1)
			.GetAllAsync(Arg.Is<AppointmentPagedListQuery>(q =>
																	q.PatientId == appointment.PatientId &&
																	q.DoctorId == appointment.DoctorId));
	}

	[Fact]
	public async Task Handle_ShouldReturnValidModel_WhenQueryIsValid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			appointment.Status,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue);

		// Act
		var result = await _handler.Handle(query, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.Items.Should().AllSatisfy(ri =>
		{
			ri.PatientId.Should().Be(appointment.PatientId);
			ri.DoctorId.Should().Be(appointment.DoctorId);
		});
	}
}
