using Shared.Domain.Abstractions;

namespace Appointments.Application.Features.DoctorSchedule.Abstractions;

public interface IDoctorScheduleRepository : IGenericRepository<Domain.Entities.DoctorSchedule>
{ }
