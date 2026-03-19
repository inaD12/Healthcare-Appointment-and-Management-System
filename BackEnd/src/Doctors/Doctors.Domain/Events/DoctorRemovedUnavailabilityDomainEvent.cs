using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record DoctorRemovedUnavailabilityDomainEvent(
    string DoctorUserId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
