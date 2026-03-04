using System.Data.Entity;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Infrastructure.Features.DBContexts;
using Shared.Domain.Exceptions;
using Shared.Infrastructure.Repositories;

namespace Ratings.Infrastructure.Features.Repositories;

public class DoctorRatingStatsRepository(RatingsDbContext context)
    : GenericRepository<DoctorRatingStats>(context), IDoctorRatingStatsRepository
{
    public async Task<DoctorRatingStats> GetOrCreateByIdAsync(
        string doctorId,
        CancellationToken cancellationToken = default)
    {
        var stats = await context.DoctorRatingStats
            .FirstOrDefaultAsync(x => x.Id == doctorId, cancellationToken);

        if (stats != null)
            return stats;

        stats = DoctorRatingStats.Create(doctorId);

        context.DoctorRatingStats.Add(stats);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return stats;
        }
        catch (ConcurrencyException)
        {
            return await context.DoctorRatingStats
                .FirstAsync(x => x.Id == doctorId, cancellationToken);
        }
    }
}