using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Doctors.Domain.Infrastructure.Abstractions.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
}
