using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using Shared.Application.Abstractions;
using Shared.Application.Models;
using Shared.Domain.Entities.ValueObjects;

namespace Appointments.Application.Features.Appointments.Abstractions;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
	Task<PagedList<Appointment>?> GetAllAsync(AppointmentPagedListQuery query, CancellationToken cancellationToken = default);
	Task<bool> IsTimeSlotAvailableAsync(string doctorId, DateTimeRange dateTimeRange, CancellationToken cancellationToken = default);
	Task<List<Appointment>?> GetAppointmentsToCompleteAsync(DateTime currentTime, CancellationToken cancellationToken = default);
	Task<List<Appointment>> GetByDoctorAndDateAsync(string doctorUserId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}
