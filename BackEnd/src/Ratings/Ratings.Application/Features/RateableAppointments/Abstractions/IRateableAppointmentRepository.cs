using Ratings.Domain.Entities;

namespace Ratings.Domain.Abstractions.Repositories;

public interface IRateableAppointmentRepository
{
    Task<RateableAppointment?> GetAsync(string appointmentId, CancellationToken ct);
    Task AddAsync(RateableAppointment item, CancellationToken ct);
    Task MarkAsRatedAsync(string appointmentId, CancellationToken ct);
    Task MarkAsNotRatedAsync(string appointmentId, CancellationToken ct);
}