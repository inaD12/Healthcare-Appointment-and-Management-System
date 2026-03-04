using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
{
	public CreateDoctorCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.TimeZoneId)
			.NotEmpty()
			.Must(IsValidTimeZone)
			.WithMessage("Invalid timezone ID.");

		RuleFor(x => x.Bio)
			.NotEmpty()
			.MinimumLength(DoctorsBusinessConfiguration.BIO_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.BIO_MAX_LENGTH);
		
		RuleFor(x => x.Specialities)
			.NotEmpty()
			.WithMessage("At least one speciality is required.");

		RuleForEach(x => x.Specialities)
			.NotEmpty().WithMessage("Speciality cannot be empty.")
			.MinimumLength(DoctorsBusinessConfiguration.SPECIALITY_MIN_LENGTH)
			.MaximumLength(DoctorsBusinessConfiguration.SPECIALITY_MAX_LENGTH);


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
