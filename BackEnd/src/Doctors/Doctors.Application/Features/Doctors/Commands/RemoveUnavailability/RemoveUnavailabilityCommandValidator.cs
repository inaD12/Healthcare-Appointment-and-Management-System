using Doctors.Application.Features.Doctors.Commands.AddUnavailability;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.RemoveUnavailability;

public class RemoveUnavailabilityCommandValidator : AbstractValidator<RemoveUnavailabilityCommand>
{
    public RemoveUnavailabilityCommandValidator()
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