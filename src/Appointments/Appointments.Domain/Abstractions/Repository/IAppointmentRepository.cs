using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;

namespace Appointments.Domain.Abstractions.Repository;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
	Task<bool> IsTimeSlotAvailableAsync(string doctorId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
	Task<Result<AppointmentWithDetailsModel>> GetAppointmentWithUserDetailsAsync(string appointmentId);
	Task<Result<List<Appointment>>> GetAppointmentsToCompleteAsync(DateTime currentTime);
}
