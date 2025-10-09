using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.ValueObjects;

namespace Doctors.Domain.Events;

public sealed record DoctorAddedUnavailabilityDomainEvent(
    string DoctorId,
    DateTime Start,
    DateTime End,
    string Reason
) : IDomainEvent;
