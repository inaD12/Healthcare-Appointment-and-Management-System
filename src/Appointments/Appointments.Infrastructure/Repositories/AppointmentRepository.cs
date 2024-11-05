using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Result;
using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Appointments.Infrastructure.Repositories
{
	internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
	{
		private readonly AppDBContext _context;
		public AppointmentRepository(AppDBContext context) : base(context)
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
						appointment.ScheduledStartTime < requestedEndTime &&
						appointment.ScheduledEndTime > requestedStartTime);

				return Result<bool>.Success(!isSlotTaken);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in IsTimeSlotAvailableAsync() in AppointmentRepository: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<bool>.Failure(Response.InternalError);
			}
		}
	}
}
