using Appointments.Domain.Entities;
using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

public class GetAllAppointmentsQueryValidator : AbstractValidator<GetAllAppointmentsQuery>
{
	public GetAllAppointmentsQueryValidator()
	{
		RuleFor(q => q.PatientId)
		   .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
		   .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.PatientId));

		RuleFor(q => q.DoctorId)
		   .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
		   .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.PatientId));

		RuleFor(q => q.Status)
			.IsInEnum()
			.When(q => q.Status.HasValue);

		RuleFor(q => q.FromTime)
			.LessThanOrEqualTo(q => q.ToTime)
			.When(q => q.FromTime.HasValue && q.ToTime.HasValue);

		RuleFor(q => q.ToTime)
			.GreaterThanOrEqualTo(q => q.FromTime)
			.When(q => q.FromTime.HasValue && q.ToTime.HasValue);

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Appointment).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
