namespace Patients.API.Patients.Models.Requests;

public sealed record StartEncounterRequest(
    string PatientId,
    string DoctorId,
    string AppointmentId);