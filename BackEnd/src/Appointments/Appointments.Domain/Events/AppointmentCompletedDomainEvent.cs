using Shared.Domain.Abstractions;

namespace Appointments.Domain.Events;

public sealed record AppointmentCompletedDomainEvent(
    string AppointmentId,
    string DoctorId,
    string PatientId
) : IDomainEvent;
