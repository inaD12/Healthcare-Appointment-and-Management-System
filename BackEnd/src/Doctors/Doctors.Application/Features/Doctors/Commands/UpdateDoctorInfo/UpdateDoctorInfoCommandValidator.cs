using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;

public class UpdateDoctorInfoCommandValidator : AbstractValidator<UpdateDoctorInfoCommand>
{
    public UpdateDoctorInfoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.NewBio)
            .MinimumLength(DoctorsBusinessConfiguration.BIO_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.BIO_MAX_LENGTH)
            .When(x => !string.IsNullOrWhiteSpace(x.NewBio));

        RuleFor(x => x.NewTimeZoneId)
            .Must(IsValidTimeZone!)
            .When(x => !string.IsNullOrWhiteSpace(x.NewTimeZoneId))
            .WithMessage("Invalid timezone ID.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.NewBio) || !string.IsNullOrWhiteSpace(x.NewTimeZoneId))
            .WithMessage("At least one field must be changed.");
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