using Doctors.Domain.Entities;
using Doctors.Domain.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Doctors.Domain.Abstractions.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(string userId,  CancellationToken cancellationToken = default);
    Task<PagedList<Doctor>?> GetAllAsync(DoctorPagedListQuery query, CancellationToken cancellationToken = default);
}
