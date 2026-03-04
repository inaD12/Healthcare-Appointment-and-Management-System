using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddUnavailability;

public class AddUnavailabilityCommandValidator : AbstractValidator<AddUnavailabilityCommand>
{
    public AddUnavailabilityCommandValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);  

        RuleFor(x => x.Start)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.End)
            .NotEmpty()
            .WithMessage("End time is required.")
            .GreaterThan(x => x.Start)
            .WithMessage("End time must be after start time.");

        RuleFor(x => x.Reason)
            .MaximumLength(AppointmentsBusinessConfiguration.REASON_MAX_LENGTH)
            .WithMessage("Reason must not exceed 200 characters.");
    }
}