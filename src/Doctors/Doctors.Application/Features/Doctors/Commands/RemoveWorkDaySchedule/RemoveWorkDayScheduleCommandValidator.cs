using Doctors.Domain.Utilities;
using FluentValidation;

namespace Doctors.Application.Features.Doctors.Commands.RemoveWorkDaySchedule;

public class RemoveWorkDayScheduleCommandValidator: AbstractValidator<RemoveWorkDayScheduleCommand>
{
    public RemoveWorkDayScheduleCommandValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .MinimumLength(DoctorsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(DoctorsBusinessConfiguration.ID_MAX_LENGTH);  
    }
}