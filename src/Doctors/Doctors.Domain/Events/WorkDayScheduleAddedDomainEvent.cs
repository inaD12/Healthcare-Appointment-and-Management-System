using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record WorkDayScheduleAddedDomainEvent(
    string DoctorId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<WorkTimeRange> WorkTimes
) : IDomainEvent;
