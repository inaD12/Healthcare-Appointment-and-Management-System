using Appointments.Domain.Entities.Enums;
using Shared.Domain.Enums;

namespace Appointments.Domain.Infrastructure.Models;

public sealed record AppointmentPagedListQuery(
	string PatientId,
	string DoctorId,
	AppointmentStatus? Status,
	DateTime? FromTime,
	DateTime? ToTime,
	SortOrder SortOrder,
	string SortPropertyName,
	int Page,
	int PageSize);

