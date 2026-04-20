using Doctors.Domain.Entities;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Specialities.Queries.GetAllDoctors;

public class GetAllSpecialitiesQueryValidator : AbstractValidator<GetAllSpecialitiesQuery>
{
	public GetAllSpecialitiesQueryValidator()
	{
		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty)
			.WithMessage("SortPropertyName must be a valid property");
	}
	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Speciality).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
