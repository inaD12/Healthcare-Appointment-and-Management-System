using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.ValueObjects;

namespace Doctors.Domain.Events;

public sealed record DoctorRemovedUnavailabilityDomainEvent(
    string DoctorId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
