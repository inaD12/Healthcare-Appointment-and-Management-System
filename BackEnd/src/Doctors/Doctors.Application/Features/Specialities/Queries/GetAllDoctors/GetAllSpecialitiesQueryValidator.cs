using Doctors.Domain.Entities;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Specialities.Queries.GetAllDoctors;

public class GetAllSpecialitiesQueryValidator : AbstractValidator<GetAllSpecialitiesQuery>
{
	public GetAllSpecialitiesQueryValidator()
	{
		RuleFor(q => q.Name)
		   .MinimumLength(DoctorsBusinessConfiguration.SPECIALITY_MIN_LENGTH)
		   .MaximumLength(DoctorsBusinessConfiguration.SPECIALITY_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.Name));

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty)
			.WithMessage("SortPropertyName must be a valid property");
	}
	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Doctor).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
