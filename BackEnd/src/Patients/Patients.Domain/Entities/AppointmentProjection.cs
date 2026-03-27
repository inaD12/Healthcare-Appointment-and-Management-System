using Patients.Domain.Enums;

namespace Patients.Domain.Entities;

public sealed class AppointmentProjection
{
    public string Id { get; set; } = default!;
    
    public string PatientId { get; set; } = default!;
    public string DoctorId { get; set; } = default!;

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public AppointmentStatus Status { get; set; }
}