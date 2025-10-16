using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.ChangeWorkDaySchedule;

public sealed record ChangeWorkDayScheduleCommand(
    string UserId,
    DayOfWeek DayOfWeek,
    List<WorkTimeRangeDto> WorkTimes ) : ICommand;
