using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveWorkDaySchedule;

public class RemoveWorkDayScheduleCommandValidator: AbstractValidator<RemoveWorkDayScheduleCommand>
{
    public RemoveWorkDayScheduleCommandValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
            .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);  
    }
}