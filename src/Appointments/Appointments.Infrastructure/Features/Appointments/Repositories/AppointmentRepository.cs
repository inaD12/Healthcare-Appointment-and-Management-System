using Appointments.Domain.Abstractions.Repository;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.ValueObjects;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.Appointments.Repositories;

internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
	private readonly AppointmentsDBContext _context;
	public AppointmentRepository(AppointmentsDBContext context) : base(context)
	{
		_context = context;
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
