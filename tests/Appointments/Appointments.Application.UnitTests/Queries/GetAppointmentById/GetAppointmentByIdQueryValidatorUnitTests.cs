using Appointments.Application.Features.Appointments.Queries.GetAllAppointmentById;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.UnitTests.Utilities;
using Appointments.Domain.Utilities;
using FluentValidation.TestHelper;
using Shared.Domain.Utilities;

namespace Appointments.Application.UnitTests.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryValidatorUnitTests: BaseAppointmentsUnitTest
{
	private readonly GetAppointmentByIdQueryValidator _validator;

	public GetAppointmentByIdQueryValidatorUnitTests()
	{
		_validator = new GetAppointmentByIdQueryValidator();
	}

	[Theory]
	[InlineData(default(int))]
	[InlineData(AppointmentsBusinessConfiguration.ID_MIN_LENGTH - 1)]
	[InlineData(AppointmentsBusinessConfiguration.ID_MAX_LENGTH + 1)]
	public void TestValidate_ShouldHaveAnErrorForId_WhenIdLengthIsInvalid(int length)
	{
		// Arrange
		var query = new GetAppointmentByIdQuery(SharedTestUtilities.GetString(length));
		// Act
		var result = _validator.TestValidate(query);
		// Assert
		result.ShouldHaveValidationErrorFor(m => m.Id);
	}

	[Fact]
	public void TestValidate_ShouldNotHaveAnyValidationsErrors_WhenModelIsValid()
	{
		// Arrange
		var query = new GetAppointmentByIdQuery(AppointmentsTestUtilities.ValidId);

		// Act
		var result = _validator.TestValidate(query);

		// Assert
		result.ShouldNotHaveValidationErrorFor(m => m.Id);
	}
}
