using Doctors.Domain.Dtos;
using Doctors.Domain.Entities;
using Pgvector;
using Shared.Domain.Abstractions;

namespace Doctors.Domain.Abstractions.Repositories;

public interface ISpecialityRepository : IGenericRepository<Speciality>
{
    Task<Speciality?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<(List<Speciality> Found, List<string> Missing)> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<List<SpecialityMatch>?> GetNearestAsync(Vector embedding, CancellationToken cancellationToken = default);
}
