using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Utilities;
using FluentValidation.TestHelper;
using Shared.Domain.Utilities;

namespace Appointments.Application.UnitTests.Commands.CreateAppointment;

public class CreateAppointmentCommandValidatorUnitTests : BaseAppointmentsUnitTest
{
	private readonly CreateAppointmentCommandValidator _validator;

	public CreateAppointmentCommandValidatorUnitTests()
	{
		_validator = new CreateAppointmentCommandValidator();
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForPatientEmail_WhenPatientEmailIsNull()
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			null!,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PatientEmail);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPatientEmail_WhenPatientEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PatientEmail);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForPatientEmail_WhenPatientEmailIsAnInvalidEmail()
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PatientEmail);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForDoctorEmail_WhenDoctorEmailIsNull()
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			null!,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.DoctorEmail);
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForDoctorEmail_WhenDoctorEmailLengthIsInvalid(int length)
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.DoctorEmail);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForDoctorEmail_WhenDoctorEmailIsAnInvalidEmail()
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.InvalidEmail,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.DoctorEmail);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForScheduledStartTime_WhenScheduledStartTimeIsInThePast()
	{
		// Arrange
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
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
		var command = new CreateAppointmentCommand(
			AppointmentsTestUtilities.PatientEmail,
			AppointmentsTestUtilities.DoctorEmail,
			AppointmentsTestUtilities.FutureDate,
			AppointmentDuration.OneHour
		);

		// Act
		var result = _validator.TestValidate(command);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
