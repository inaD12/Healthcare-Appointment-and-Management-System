using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record DoctorRemovedExtraAvailabilityDomainEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
