using Patients.Infrastructure.Features.ReadModels.Enums;

namespace Patients.Infrastructure.Features.ReadModels;

public sealed class AppointmentProjection
{
    public string Id { get; set; } = default!;
    
    public string PatientId { get; set; } = default!;
    public string DoctorId { get; set; } = default!;

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public AppointmentStatus Status { get; set; }
}