using Doctors.Application.Features.Doctors.Models;
using Shared.Application.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RemoveWorkDaySchedule;

public sealed record RemoveWorkDayScheduleCommand(
    string UserId,
    DayOfWeek DayOfWeek) : ICommand;
