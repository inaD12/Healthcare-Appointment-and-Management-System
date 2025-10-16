using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.AddExtraAvailability;

public class AddExtraAvailabilityCommandValidator : AbstractValidator<AddExtraAvailabilityCommand>
{
    public AddExtraAvailabilityCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);  

        RuleFor(x => x.Start)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.End)
            .NotEmpty()
            .WithMessage("End time is required.")
            .GreaterThan(x => x.Start)
            .WithMessage("End time must be after start time.");

        RuleFor(x => x.Reason)
            .MaximumLength(DoctorsBusinessConfiguration.REASON_MAX_LENGTH)
            .WithMessage("Reason must not exceed 200 characters.");
    }
}