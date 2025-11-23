using Appointments.Domain.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Models;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Models;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.Repositories;

internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
	private readonly AppointmentsDBContext _context;
	public AppointmentRepository(AppointmentsDBContext context) : base(context)
	{
		_context = context;
	}

	public async Task<PagedList<Appointment>?> GetAllAsync(AppointmentPagedListQuery query, CancellationToken cancellationToken = default)
	{
		var entitiesQuery = _context.Appointments
			.Where(u =>
				(string.IsNullOrEmpty(query.DoctorId) || u.DoctorId == query.DoctorId) &&
				(string.IsNullOrEmpty(query.PatientId) || u.PatientId == query.PatientId) &&
				(!query.Status.HasValue || u.Status == query.Status!.Value) &&
				(!query.FromTime.HasValue || u.Duration.End >= query.FromTime) &&
				(!query.ToTime.HasValue || u.Duration.Start <= query.ToTime)
			).ApplySorting(query.SortPropertyName, query.SortOrder);

		if (entitiesQuery == null)
			return null!;

		var appointments = await PagedList<Appointment>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
		return appointments;
	}
	public async Task<bool> IsTimeSlotAvailableAsync(string doctorId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default)
	{
		bool isSlotTaken = await _context.Appointments
			.AnyAsync(appointment =>
				appointment.DoctorId == doctorId &&
				appointment.Status == AppointmentStatus.Scheduled &&
				appointment.Duration.Start <= dateTimeRange.End &&
				appointment.Duration.End >= dateTimeRange.Start,
				cancellationToken);

		return !isSlotTaken;
	}

	public async Task<List<Appointment>?> GetAppointmentsToCompleteAsync(DateTime currentTime, CancellationToken cancellationToken = default)
	{
		var res = await _context.Appointments
		.Where(a => a.Duration.End <= currentTime && a.Status == AppointmentStatus.Scheduled)
		.ToListAsync(cancellationToken);

		return res;
	}
}
