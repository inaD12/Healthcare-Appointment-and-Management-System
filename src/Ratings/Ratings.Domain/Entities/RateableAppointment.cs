using Shared.Domain.Entities.Base;

namespace Ratings.Domain.Entities;

public sealed class RateableAppointment: BaseEntity
{
    public string DoctorId { get; private set; }
    public string PatientId { get; private set; }
    public bool IsConsumed { get; private set; }
    
    private RateableAppointment(string appointmentId, string doctorId, string patientId)
    {
        Id = appointmentId;
        DoctorId = doctorId;
        PatientId = patientId;
        IsConsumed = false;
    }

    public static RateableAppointment Create(string appointmentId, string doctorId, string patientId)
    {
        return new RateableAppointment(appointmentId, doctorId, patientId);
    }

    public void Consume()
    {
        IsConsumed = true;
    }
    
}
