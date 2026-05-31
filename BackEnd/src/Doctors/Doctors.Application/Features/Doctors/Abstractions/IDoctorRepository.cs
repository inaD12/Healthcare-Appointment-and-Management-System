using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Entities;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Doctors.Application.Features.Doctors.Abstractions;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(string userId,  CancellationToken cancellationToken = default);
    Task<PagedList<Doctor>?> GetAllAsync(DoctorPagedListQuery query, CancellationToken cancellationToken = default);
}
