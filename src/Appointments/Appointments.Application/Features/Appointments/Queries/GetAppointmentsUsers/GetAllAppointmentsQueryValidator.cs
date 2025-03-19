using Appointments.Domain.Entities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

public class GetAllAppointmentsQueryValidator : AbstractValidator<GetAllAppointmentsQuery>
{
	public GetAllAppointmentsQueryValidator()
	{
		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Appointment).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
