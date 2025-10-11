using Shared.Domain.Abstractions;

namespace Doctors.Domain.Events;

public sealed record DoctorAddedExtraAvailabilityDomainEvent(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason
) : IDomainEvent;
