using Doctors.Domain.Entities;
using Doctors.Domain.Infrastructure.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Doctors.Domain.Infrastructure.Abstractions.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(string userId,  CancellationToken cancellationToken = default);
    Task<PagedList<Doctor>?> GetAllAsync(DoctorPagedListQuery query, CancellationToken cancellationToken = default);
}
