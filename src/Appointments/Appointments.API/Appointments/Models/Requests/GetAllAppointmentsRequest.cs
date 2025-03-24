using Appointments.Domain.Entities.Enums;
using Shared.API.Models.Requests;
using Shared.Domain.Enums;

namespace Appointments.API.Appointments.Models.Requests;

public record GetAllAppointmentsRequest
(
	string PatientId = "",
	string DoctorId = "",
	AppointmentStatus? Status = null,
	DateTime? FromTime = null,
	DateTime? ToTime = null,
	SortOrder SortOrder = SortOrder.ASC,
	string SortPropertyName = "Id",
	int Page = 1,
	int PageSize = 10
) : CollectionReadRequest(SortOrder, SortPropertyName, Page, PageSize);

