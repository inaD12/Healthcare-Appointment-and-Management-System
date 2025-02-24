namespace Appointments.Application.Features.Appointments.Models;

public class DoctorPatientIdModel
{
	public DoctorPatientIdModel(string doctorId, string patientId)
	{
		DoctorId = doctorId;
		PatientId = patientId;
	}

	public string DoctorId { get; set; }
	public string PatientId { get; set; }
}

