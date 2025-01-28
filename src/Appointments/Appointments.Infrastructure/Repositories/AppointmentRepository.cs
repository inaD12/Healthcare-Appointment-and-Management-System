using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Repositories;
using Appointments.Domain.Responses;
using Appointments.Infrastructure.DBContexts;
using Contracts.Results;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Appointments.Infrastructure.Repositories
{
	internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
	{
		private readonly AppointmentsDBContext _context;
		public AppointmentRepository(AppointmentsDBContext context) : base(context)
		{
			_context = context;
		}

		public async Task<Result<bool>> IsTimeSlotAvailableAsync(string doctorId, DateTime requestedStartTime, DateTime requestedEndTime)
		{
			try
			{
				bool isSlotTaken = await _context.Appointments
					.AnyAsync(appointment =>
						appointment.DoctorId == doctorId &&
						appointment.Status == AppointmentStatus.Scheduled &&
						appointment.ScheduledStartTime <= requestedEndTime &&
						appointment.ScheduledEndTime >= requestedStartTime);

				return Result<bool>.Success(!isSlotTaken);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in IsTimeSlotAvailableAsync() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<bool>.Failure(Responses.InternalError);
			}
		}

		public async Task<Result> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus)
		{
			try
			{
				appointment.Status = newStatus;

				_context.SaveChanges();

				return Result.Success(Responses.Ok);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in ChangeStatus() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Responses.InternalError);
			}
		}

		public async Task<Result<AppointmentWithDetailsDTO>> GetAppointmentWithUserDetailsAsync(string appointmentId)
		{
			try
			{
				var result = await (
				from appointment in _context.Appointments
				join doctor in _context.UserData on appointment.DoctorId equals doctor.UserId
				join patient in _context.UserData on appointment.PatientId equals patient.UserId
				where appointment.Id == appointmentId
				select new AppointmentWithDetailsDTO
				{
					AppointmentId = appointment.Id,
					ScheduledStartTime = appointment.ScheduledStartTime,
					ScheduledEndTime = appointment.ScheduledEndTime,
					Status = appointment.Status,
					DoctorEmail = doctor.Email,
					PatientEmail = patient.Email,
					DoctorId = doctor.UserId,
					PatientId = patient.UserId,
					Appointment = appointment

				}
			).FirstOrDefaultAsync();

				if (result == null)
					return Result<AppointmentWithDetailsDTO>.Failure(Responses.AppointmentNotFound);

				return Result<AppointmentWithDetailsDTO>.Success(result);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetAppointmentWithUserDetailsAsync() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<AppointmentWithDetailsDTO>.Failure(Responses.InternalError);
			}
		}

		public async Task<Result<List<Appointment>>> GetAppointmentsToCompleteAsync(DateTime currentTime)
		{
			try
			{
				var res = await _context.Appointments
				.Where(a => a.ScheduledEndTime <= currentTime && a.Status == AppointmentStatus.Scheduled)
				.ToListAsync();

				if (res == null)
				{
					return Result<List<Appointment>>.Failure(Responses.AppointmentNotFound);
				}

				return Result<List<Appointment>>.Success(res);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetAppointmentsToCompleteAsync() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<List<Appointment>>.Failure(Responses.InternalError);
			}
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
