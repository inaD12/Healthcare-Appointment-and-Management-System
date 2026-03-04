using Shared.Domain.Abstractions;

namespace Appointments.Domain.Events;

public sealed record AppointmentCanceledDomainEvent(
    string AppointmentId
) : IDomainEvent;
