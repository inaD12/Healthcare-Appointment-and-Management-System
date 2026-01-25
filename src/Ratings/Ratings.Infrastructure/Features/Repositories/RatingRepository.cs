using System.Data.Entity;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Ratings.Infrastructure.Features.Repositories;

public class RatingRepository(RatingsDbContext context)
    : GenericRepository<Rating>(context), IRatingRepository
{
    public async Task<bool> ExistsForAppointmentAsync(
        string appointmentId,
        CancellationToken cancellationToken = default)
    {
        return await context.Ratings
            .AsNoTracking()
            .AnyAsync(r => r.AppointmentId == appointmentId, cancellationToken);
    }
}