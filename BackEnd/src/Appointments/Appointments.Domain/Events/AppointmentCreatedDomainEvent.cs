using Shared.Domain.Abstractions;

namespace Appointments.Domain.Events;

public sealed record AppointmentCreatedDomainEvent(
    string AppointmentId,
    string DoctorId,
    string PatientId,
    DateTime Start,
    DateTime End
) : IDomainEvent;
