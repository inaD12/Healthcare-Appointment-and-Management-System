using Ratings.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Ratings.Domain.Abstractions.Repositories;

public interface IRatingRepository : IGenericRepository<Rating>
{
    Task<bool> ExistsForAppointmentAsync(string appointmentId, CancellationToken cancellationToken = default);
}
