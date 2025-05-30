using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Utilities;
using FluentValidation.TestHelper;
using Shared.Domain.Utilities;

namespace Appointments.Application.UnitTests.Commands.RescheduleAppointment;

public class RescheduleAppointmentCommandValidatorUnitTests : BaseAppointmentsUnitTest
{
	private readonly RescheduleAppointmentCommandValidator _validator;

	public RescheduleAppointmentCommandValidatorUnitTests()
	{
		_validator = new RescheduleAppointmentCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForAppointmentId_WhenAppointmentIdIsNull()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			null!,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.AppointmentId);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForAppointmentId_WhenAppointmentIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.AppointmentId);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForUserId_WhenUserIdIsNull()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			null!,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.UserId);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForUserId_WhenUserIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.UserId);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForScheduledStartTime_WhenScheduledStartTimeIsInThePast()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.PastDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.ScheduledStartTime);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForDuration_WhenDurationIsInvalid()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.FutureDate,
			(AppointmentDuration)999
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Duration);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
	{
		// Arrange
		var command = new RescheduleAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
