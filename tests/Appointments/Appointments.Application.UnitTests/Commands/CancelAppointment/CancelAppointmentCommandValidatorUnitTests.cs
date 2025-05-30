using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Utilities;
using FluentValidation.TestHelper;
using Shared.Domain.Utilities;

namespace Appointments.Application.UnitTests.Commands.CancelAppointment;

public class CancelAppointmentCommandValidatorUnitTests : BaseAppointmentsUnitTest
{
	private readonly CancelAppointmentCommandValidator _validator;

	public CancelAppointmentCommandValidatorUnitTests()
	{
		_validator = new CancelAppointmentCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForAppointmentId_WhenAppointmentIdIsNull()
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			null!,
			AppointmentsTestUtilities.ValidId
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.AppointmentId);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH- 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForAppointmentId_WhenAppointmentIdLengthIsInvalid(int length)
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.ValidId
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
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			null!
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
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			SharedTestUtilities.GetString(length)
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.UserId);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenCommandIsValid()
	{
		// Arrange
		var command = new CancelAppointmentCommand(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
