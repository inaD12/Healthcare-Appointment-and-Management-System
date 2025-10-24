using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record DoctorRemovedExtraAvailabilityDomainEvent(
    string DoctorId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
