using Doctors.Application.Features.Doctors.Commands.AddWorkDaySchedule;
using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.ChangeWorkDaySchedule;

public class ChangeWorkDayScheduleCommandValidator: AbstractValidator<ChangeWorkDayScheduleCommand>
{
    public ChangeWorkDayScheduleCommandValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);  
        
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