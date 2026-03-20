using Appointments.Domain.Entities;
using Appointments.Domain.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.ValueObjects;
using Shared.Domain.Models;

namespace Appointments.Domain.Abstractions;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
	Task<PagedList<Appointment>?> GetAllAsync(AppointmentPagedListQuery query, CancellationToken cancellationToken = default);
	Task<bool> IsTimeSlotAvailableAsync(string doctorId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
	Task<List<Appointment>?> GetAppointmentsToCompleteAsync(DateTime currentTime, CancellationToken cancellationToken = default);
	Task<List<Appointment>> GetByDoctorAndDateAsync(string doctorUserId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}
