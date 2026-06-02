using Ratings.Domain.Entities;
using Shared.Application.Abstractions;

namespace Ratings.Application.Features.Ratings.Abstractions;

public interface IDoctorRatingStatsRepository : IGenericRepository<DoctorRatingStats>
{
    Task<DoctorRatingStats> GetOrCreateByIdAsync(string doctorId, CancellationToken cancellationToken = default);
}
