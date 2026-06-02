using Appointments.Domain.Entities;
using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.ChangeWorkDaySchedule;

public sealed record ChangeWorkDayScheduleCommand(
    string DoctorUserId,
    DayOfWeek DayOfWeek,
    List<WorkTimeRange> WorkTimes ) : ICommand;
