using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RemoveWorkDaySchedule;

public sealed record RemoveWorkDayScheduleCommand(
    string DoctorId,
    DayOfWeek DayOfWeek) : ICommand;
