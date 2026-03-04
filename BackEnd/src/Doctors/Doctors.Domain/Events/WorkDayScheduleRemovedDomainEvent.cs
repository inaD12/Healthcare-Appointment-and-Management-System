using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record WorkDayScheduleRemovedDomainEvent(
    string DoctorId,
    DayOfWeek DayOfWeek
) : IDomainEvent;
