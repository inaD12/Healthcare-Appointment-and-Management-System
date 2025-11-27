using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.RemoveWorkDaySchedule;

public sealed record RemoveWorkDayScheduleCommand(
    string DoctorId,
    DayOfWeek DayOfWeek) : ICommand;
