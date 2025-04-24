using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.IntegrationTests.Utilities;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using FluentAssertions;
using Shared.Domain.Exceptions;
using Shared.Domain.Utilities;

namespace Appointments.Application.IntegrationTests.Commands.CancelAppointment;

public class CancelAppointmentCommandHandlerIntegrationTests : BaseAppointmentsIntegrationTest
{
	public CancelAppointmentCommandHandlerIntegrationTests(AppointmentsIntegrationTestWebAppFactory integrationTestWebAppFactory) : base(integrationTestWebAppFactory)
	{
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenAppointmentIdIsNull()
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			null!,
			AppointmentsTestUtilities.ValidId
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenAppointmentIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.ValidId
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldThrowValidationException_WhenUserIdIsNull()
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			null!
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Theory]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public async Task Send_ShouldThrowValidationException_WhenUserIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			SharedTestUtilities.GetString(length)
		);

		// Act
		var action = async () => await Sender.Send(command, CancellationToken);

		// Assert
		await action
			.Should()
			.ThrowAsync<HAMSValidationException>();
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenAppointmentIsNotFound()
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.InvalidId,
			AppointmentsTestUtilities.ValidId
		);

		// Act
		var result =  await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().Be(ResponseList.AppointmentNotFound);
	}

	[Fact]
	public async Task Send_ShouldReturnFailure_WhenAppointmentIsNotScheduled()
	{
		// Arrange
		var appointment = await CreateAppointmentAsync(true);
		var command = new CancelAppointmentCommand(
			appointment.Id,
			appointment.PatientId
		);

		// Act
		var result = await Sender.Send(command, CancellationToken);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.Response.Should().Be(ResponseList.AppointmentNotScheduled);
	}
}
