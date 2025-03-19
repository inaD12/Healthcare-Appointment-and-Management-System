using Appointments.Domain.Entities.Enums;
using Shared.API.Models.Requests;

namespace Appointments.API.Appointments.Models.Requests;

public class GetAllAppointmentsRequest : CollectionReadRequest
{
	public string? PatientId { get; set; } = string.Empty;
	public string? DoctorId { get; set; } = string.Empty;
	public AppointmentStatus? Status { get; set; }
	public DateTime? FromTime { get; set; }
	public DateTime? ToTime { get; set; }
}
