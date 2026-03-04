using Appointments.Domain.Entities;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.ChangeWorkDaySchedule;

public sealed record ChangeWorkDayScheduleCommand(
    string DoctorId,
    DayOfWeek DayOfWeek,
    List<WorkTimeRange> WorkTimes ) : ICommand;
