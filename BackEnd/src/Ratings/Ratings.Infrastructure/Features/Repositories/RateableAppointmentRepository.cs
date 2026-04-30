using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Ratings.Infrastructure.Features.Repositories;

public sealed class RateableAppointmentsRepository(RatingsDbContext context) : IRateableAppointmentRepository
{
    public Task<RateableAppointment?> GetAsync(string appointmentId, CancellationToken ct)
    {
        return context.RateableAppointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId, ct);
    }

    public async Task AddAsync(RateableAppointment item, CancellationToken ct)
    {
        await context.RateableAppointments.AddAsync(item, ct);
    }

    public async Task MarkAsRatedAsync(string appointmentId, CancellationToken ct)
    {
        var entity = await context.RateableAppointments
            .Where(x => x.Id == appointmentId && !x.IsConsumed)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.IsConsumed, true),
                ct);
    }
    
    public async Task MarkAsNotRatedAsync(string appointmentId, CancellationToken ct)
    {
        var entity = await context.RateableAppointments
            .Where(x => x.Id == appointmentId && x.IsConsumed)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.IsConsumed, false),
                ct);
    }
}