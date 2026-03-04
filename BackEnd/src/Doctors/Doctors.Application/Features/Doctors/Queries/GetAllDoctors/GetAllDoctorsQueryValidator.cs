using Doctors.Domain.Entities;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Queries.GetAllDoctors;

public class GetAllDoctorsQueryValidator : AbstractValidator<GetAllDoctorsQuery>
{
	public GetAllDoctorsQueryValidator()
	{
		RuleFor(q => q.Speciality)
		   .MinimumLength(DoctorsBusinessConfiguration.SPECIALITY_MIN_LENGTH)
		   .MaximumLength(DoctorsBusinessConfiguration.SPECIALITY_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.Speciality));

		RuleFor(x => x.TimeZoneId)
			.Must(IsValidTimeZone!)
			.When(q => !string.IsNullOrEmpty(q.TimeZoneId))
			.WithMessage("Invalid timezone ID.");

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty)
			.WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(Doctor).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
	
	private static bool IsValidTimeZone(string timeZoneId)
	{
		try
		{
			TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
