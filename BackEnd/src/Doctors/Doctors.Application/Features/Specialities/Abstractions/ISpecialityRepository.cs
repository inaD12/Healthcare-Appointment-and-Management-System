using Doctors.Application.Features.Specialities.Dtos;
using Doctors.Application.Features.Specialities.Models;
using Doctors.Domain.Entities;
using Pgvector;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Doctors.Application.Features.Specialities.Abstractions;

public interface ISpecialityRepository : IGenericRepository<Speciality>
{
    Task<Speciality?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<(List<Speciality> Found, List<string> Missing)> GetByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
    Task<List<SpecialityMatch>?> GetNearestAsync(Vector embedding, CancellationToken cancellationToken = default);
    Task<PagedList<Speciality>?> GetAllAsync(SpecialityPagedListQuery query, CancellationToken cancellationToken = default);
}
