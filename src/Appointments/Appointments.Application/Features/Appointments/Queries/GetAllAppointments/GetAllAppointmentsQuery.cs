using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities.Enums;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsUsers;

public sealed record GetAllAppointmentsQuery(
	string? PatientId,
	string? DoctorId,
	AppointmentStatus? Status,
	DateTime? FromTime,
	DateTime? ToTime,
	SortOrder? SortOrder,
	string SortPropertyName,
	int Page,
	int PageSize) : IQuery<AppointmentPaginatedQueryViewModel>;
