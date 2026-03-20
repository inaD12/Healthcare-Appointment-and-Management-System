using System.Data.Entity;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Domain.Models;
using Ratings.Infrastructure.Features.DBContexts;
using Shared.Domain.Models;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;

namespace Ratings.Infrastructure.Features.Repositories;

public class RatingsRepository(RatingsDbContext context)
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
    
    public async Task<PagedList<Rating>> GetAllAsync(
        RatingPagedListQuery query,
        CancellationToken cancellationToken = default)
    {
        var entitiesQuery = context.Ratings
            .Where(r =>
                (string.IsNullOrEmpty(query.DoctorId) || r.DoctorId == query.DoctorId) &&
                (string.IsNullOrEmpty(query.PatientId) || r.PatientId == query.PatientId) &&
                (string.IsNullOrEmpty(query.AppointmentId) || r.AppointmentId == query.AppointmentId) &&
                (!query.MinScore.HasValue || r.Score >= query.MinScore.Value) &&
                (!query.MaxScore.HasValue || r.Score <= query.MaxScore.Value)
            );

        if (string.IsNullOrEmpty(query.SortPropertyName))
        {
            entitiesQuery = entitiesQuery.OrderByDescending(r => r.CreatedAt);
        }
        else
        {
            entitiesQuery = entitiesQuery.ApplySorting(query.SortPropertyName, query.SortOrder);
        }

        return await PagedList<Rating>.CreateAsync(
            entitiesQuery,
            query.Page,
            query.PageSize,
            cancellationToken
        );
    }
}