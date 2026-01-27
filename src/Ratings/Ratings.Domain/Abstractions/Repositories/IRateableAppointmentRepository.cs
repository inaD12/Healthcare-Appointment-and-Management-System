using Ratings.Domain.Entities;

namespace Ratings.Domain.Abstractions.Repositories;

public interface IRateableAppointmentRepository
{
    Task<RateableAppointment?> GetAsync(string appointmentId, CancellationToken ct);
    void AddAsync(RateableAppointment item, CancellationToken ct);
    Task ConsumeAsync(string appointmentId, CancellationToken ct);
}