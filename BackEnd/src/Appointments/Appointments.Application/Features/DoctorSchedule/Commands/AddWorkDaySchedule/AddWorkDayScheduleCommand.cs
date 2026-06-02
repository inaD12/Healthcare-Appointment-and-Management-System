using Appointments.Domain.Entities;
using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.DoctorSchedule.Commands.AddWorkDaySchedule;

public sealed record AddWorkDayScheduleCommand(
    string DoctorUserId,
    DayOfWeek DayOfWeek,
    List<WorkTimeRange> WorkTimes ) : ICommand;
