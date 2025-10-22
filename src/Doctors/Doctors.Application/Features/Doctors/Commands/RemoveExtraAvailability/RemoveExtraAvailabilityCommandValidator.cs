using Doctors.Application.Features.Doctors.Commands.AddExtraAvailability;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.RemoveExtraAvailability;

public class RemoveExtraAvailabilityCommandValidator : AbstractValidator<RemoveExtraAvailabilityCommand>
{
    public RemoveExtraAvailabilityCommandValidator()
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
    }
}