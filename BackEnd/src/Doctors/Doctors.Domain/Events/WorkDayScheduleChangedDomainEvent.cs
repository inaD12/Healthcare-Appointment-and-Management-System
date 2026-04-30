using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record WorkDayScheduleChangedDomainEvent(
    string DoctorUserId,
    DayOfWeek DayOfWeek,
    IReadOnlyCollection<WorkTimeRange> WorkTimes
) : IDomainEvent;
