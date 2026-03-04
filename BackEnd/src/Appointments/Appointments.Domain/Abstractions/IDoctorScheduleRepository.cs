using Appointments.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Appointments.Domain.Abstractions;

public interface IDoctorScheduleRepository : IGenericRepository<DoctorSchedule>
{ }
