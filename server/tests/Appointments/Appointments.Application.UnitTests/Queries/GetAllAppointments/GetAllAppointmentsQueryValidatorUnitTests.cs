using Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Utilities;
using FluentValidation.TestHelper;
using Shared.Domain.Utilities;

namespace Appointments.Application.UnitTests.Queries.GetAllAppointments;

public class GetAllAppointmentsQueryValidatorUnitTests : BaseAppointmentsUnitTest
{
	private readonly GetAllAppointmentsQueryValidator _validator;

	public GetAllAppointmentsQueryValidatorUnitTests()
	{
		_validator = new GetAllAppointmentsQueryValidator();
	}

	[Theory]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForPatientId_WhenPatientIdLengthIsInvalid(int length)
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			SharedTestUtilities.GetString(length),
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.PatientId);
	}

	[Theory]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForDoctorId_WhenDoctorIdLengthIsInvalid(int length)
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			SharedTestUtilities.GetString(length),
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.DoctorId);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForStatus_WhenStatusIsInvalid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			(AppointmentStatus)10,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Status);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForFromTime_WhenFromTimeIsAheadOfToTime()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.FromTime);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForToTime_WhenToTimeIsBehindFromTime()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.ToTime);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForSortPropertyName_WhenSortPropertyNameIsInvalid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.InvalidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.SortPropertyName);
	}

	[Fact]
	public void TestValidate_ShouldHaveAnErrorForSortPropertyName_WhenSortPropertyNameIsNull()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			null!,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldHaveValidationErrorFor(m => m.SortPropertyName);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelIsValid()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			appointment.PatientId,
			appointment.DoctorId,
			AppointmentsTestUtilities.ValidAppointmentStatus,
			AppointmentsTestUtilities.PastDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelParametersAreNull()
	{
		// Arrange
		var appointment = GetAppointment();
		var query = new GetAllAppointmentsQuery(
			null!,
			null!,
			null!,
			null!,
			null!,
			AppointmentsTestUtilities.ValidSortOrderProperty,
			AppointmentsTestUtilities.ValidSortPropertyName,
			AppointmentsTestUtilities.ValidPageValue,
			AppointmentsTestUtilities.ValidPageSizeValue
		);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}
