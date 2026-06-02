using Appointments.Domain.Enums;
using Shared.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Models;

public sealed record AppointmentPagedListQuery(
	string? PatientId,
	string? DoctorId,
	AppointmentStatus? Status,
	DateTime? FromTime,
	DateTime? ToTime,
	SortOrder SortOrder,
	string SortPropertyName,
	int Page,
	int PageSize);

