using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.DoctorSchedule.Commands.ChangeWorkDaySchedule;

public class ChangeWorkDayScheduleCommandValidator: AbstractValidator<ChangeWorkDayScheduleCommand>
{
    public ChangeWorkDayScheduleCommandValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);  
        
        RuleFor(x => x.WorkTimes)
            .NotEmpty()
            .WithMessage("At least one work time range is required.");

        RuleForEach(x => x.WorkTimes).ChildRules(times =>
        {
            times.RuleFor(t => t.Start)
                .LessThan(t => t.End)
                .WithMessage("Start must be before End.");
        });
    }
}