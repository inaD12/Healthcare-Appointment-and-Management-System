using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Doctors.Domain.Infrastructure.Abstractions.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(string userId,  CancellationToken cancellationToken = default);
}
