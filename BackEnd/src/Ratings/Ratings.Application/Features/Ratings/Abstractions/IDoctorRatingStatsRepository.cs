using Ratings.Domain.Entities;
using Shared.Application.Abstractions;

namespace Ratings.Domain.Abstractions.Repositories;

public interface IDoctorRatingStatsRepository : IGenericRepository<DoctorRatingStats>
{
    Task<DoctorRatingStats> GetOrCreateByIdAsync(string doctorId, CancellationToken cancellationToken = default);
}
