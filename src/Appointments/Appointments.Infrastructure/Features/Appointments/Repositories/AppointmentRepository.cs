using Appointments.Domain.Entities;
using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Infrastructure.Models;
using Appointments.Domain.Responses;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.Appointments.Repositories;

internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
	private readonly AppointmentsDBContext _context;
	public AppointmentRepository(AppointmentsDBContext context) : base(context)
	{
		_context = context;
	}

	public async Task<Result<PagedList<Appointment>>> GetAllAsync(AppointmentPagedListQuery query, CancellationToken cancellationToken = default)
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
			return Result<PagedList<Appointment>>.Failure(ResponseList.NoAppointmentsFound);

		var appointments = await PagedList<Appointment>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
		return Result<PagedList<Appointment>>.Success(appointments);
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

	public async Task<Result<AppointmentWithDetailsModel>> GetAppointmentWithUserDetailsAsync(string appointmentId)
	{
		var result = await (
		from appointment in _context.Appointments
		join doctor in _context.UserData on appointment.DoctorId equals doctor.UserId
		join patient in _context.UserData on appointment.PatientId equals patient.UserId
		where appointment.Id == appointmentId
		select new AppointmentWithDetailsModel
		{
			AppointmentId = appointment.Id,
			ScheduledStartTime = appointment.Duration.Start,
			ScheduledEndTime = appointment.Duration.End,
			Status = appointment.Status,
			DoctorEmail = doctor.Email,
			PatientEmail = patient.Email,
			DoctorId = doctor.UserId,
			PatientId = patient.UserId,
			Appointment = appointment

		}
		).FirstOrDefaultAsync();

		if (result == null)
			return Result<AppointmentWithDetailsModel>.Failure(ResponseList.AppointmentNotFound);

		return Result<AppointmentWithDetailsModel>.Success(result);
	}

	public async Task<Result<List<Appointment>>> GetAppointmentsToCompleteAsync(DateTime currentTime)
	{
		var res = await _context.Appointments
		.Where(a => a.Duration.End <= currentTime && a.Status == AppointmentStatus.Scheduled)
		.ToListAsync();

		return Result<List<Appointment>>.Success(res);
	}
}
