namespace Shared.Application.IntegrationEvents;

public sealed record AppointmentCreatedIntegrationEvent(
    string AppointmentId,
    string DoctorId,
    string PatientId,
    DateTime Start,
    DateTime End
);
