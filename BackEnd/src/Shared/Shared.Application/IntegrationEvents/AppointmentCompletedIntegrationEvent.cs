namespace Shared.Application.IntegrationEvents;

public sealed record AppointmentCompletedIntegrationEvent(
    string AppointmentId,
    string DoctorId,
    string PatientId
);
