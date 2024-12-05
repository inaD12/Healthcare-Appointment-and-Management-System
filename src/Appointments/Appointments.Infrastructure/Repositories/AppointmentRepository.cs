using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Result;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
				return Result<bool>.Failure(Response.InternalError);
			}
		}

		public async Task<Result> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus)
		{
			try
			{
				appointment.Status = newStatus;

				_context.SaveChanges();

				return Result.Success(Response.Ok);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in ChangeStatus() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}

		public async Task<Result<AppointmentWithDetailsDTO>> GetAppointmentWithUserDetailsAsync(string appointmentId)
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
					PatientId = patient.UserId
				}
			).FirstOrDefaultAsync();

			if (result == null)
				return Result<AppointmentWithDetailsDTO>.Failure(Response.AppointmentNotFound);

			return Result<AppointmentWithDetailsDTO>.Success(result);
		}


	}
}
