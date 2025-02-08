using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;

namespace Appointments.Domain.Repositories
{
	public interface IAppointmentRepository : IGenericRepository<Appointment>
	{
		Task<Result<bool>> IsTimeSlotAvailableAsync(string doctorId, DateTime requestedStartTime, DateTime requestedEndTime);
		Task<Result> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus);
		Task<Result<AppointmentWithDetailsDTO>> GetAppointmentWithUserDetailsAsync(string appointmentId);
		Task<Result<List<Appointment>>> GetAppointmentsToCompleteAsync(DateTime currentTime);
		Task SaveChangesAsync();
	}
}
