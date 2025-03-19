using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Appointments.Domain.Infrastructure.Abstractions.Repository;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
	Task<PagedList<Appointment>?> GetAllAsync(AppointmentPagedListQuery query, CancellationToken cancellationToken = default);
	Task<bool> IsTimeSlotAvailableAsync(string doctorId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
	Task<AppointmentWithDetailsModel?> GetAppointmentWithUserDetailsAsync(string appointmentId);
	Task<List<Appointment>?> GetAppointmentsToCompleteAsync(DateTime currentTime);
}
