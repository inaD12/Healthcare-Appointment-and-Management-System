using Ratings.Domain.Entities;
using Ratings.Domain.Models;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Ratings.Domain.Abstractions.Repositories;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task<Rating?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default);
    Task<PagedList<Rating>> GetAllAsync(RatingPagedListQuery query, CancellationToken cancellationToken = default);
}
