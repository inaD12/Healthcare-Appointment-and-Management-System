using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Contracts.Results;

namespace Appointments.Domain.Repositories
{
	public interface IAppointmentRepository : IGenericRepository<Appointment>
	{
		Task<Result<bool>> IsTimeSlotAvailableAsync(string doctorId, DateTime requestedStartTime, DateTime requestedEndTime);
		Task<Result> ChangeStatusAsync(Appointment appointment, AppointmentStatus newStatus);
		Task<Result<AppointmentWithDetailsDTO>> GetAppointmentWithUserDetailsAsync(string appointmentId);
		Task<List<Appointment>> GetAppointmentsToCompleteAsync(DateTime currentTime);
		Task SaveChangesAsync();
	}
}
