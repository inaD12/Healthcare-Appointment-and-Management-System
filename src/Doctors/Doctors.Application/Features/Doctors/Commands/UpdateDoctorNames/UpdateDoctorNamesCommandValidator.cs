using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorNames;

public class UpdateDoctorNamesCommandValidator : AbstractValidator<UpdateDoctorNamesCommand>
{
    public UpdateDoctorNamesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);

        RuleFor(x => x.FirstName)
            .MinimumLength(DoctorsBusinessConfiguration.FIRSTNAME_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.FIRSTNAME_MAX_LENGTH)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));
        
        RuleFor(x => x.LastName)
            .MinimumLength(DoctorsBusinessConfiguration.LASTTNAME_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.LASTNAME_MAX_LENGTH)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.FirstName) || !string.IsNullOrWhiteSpace(x.LastName))
            .WithMessage("At least one field must be changed.");
    }
}