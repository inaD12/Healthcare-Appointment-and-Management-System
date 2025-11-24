using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record DoctorRemovedUnavailabilityDomainEvent(
    string DoctorId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
