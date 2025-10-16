using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.AddWorkDaySchedule;

public sealed record AddWorkDayScheduleCommand(
    string UserId,
    DayOfWeek DayOfWeek,
    List<WorkTimeRangeDto> WorkTimes ) : ICommand;
