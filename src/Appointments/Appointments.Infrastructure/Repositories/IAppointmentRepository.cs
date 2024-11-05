using Appointments.Domain.Entities;
using Appointments.Domain.Result;

namespace Appointments.Infrastructure.Repositories
{
	public interface IAppointmentRepository : IGenericRepository<Appointment>
	{
		Task<Result<bool>> IsTimeSlotAvailableAsync(string doctorId, DateTime requestedStartTime, DateTime requestedEndTime);
	}
}
