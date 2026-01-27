using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Ratings.Infrastructure.Features.Repositories;

public sealed class RateableAppointmentStore : IRateableAppointmentRepository
{
    private readonly RatingsDbContext _context;

    public RateableAppointmentStore(RatingsDbContext context)
    {
        _context = context;
    }

    public Task<RateableAppointment?> GetAsync(string appointmentId, CancellationToken ct)
    {
        return _context.RateableAppointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId, ct);
    }

    public void AddAsync(RateableAppointment item, CancellationToken ct)
    {
        _context.RateableAppointments.Add(item);
    }

    public async Task ConsumeAsync(string appointmentId, CancellationToken ct)
    {
        var entity = await _context.RateableAppointments
            .Where(x => x.Id == appointmentId && !x.IsConsumed)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.IsConsumed, true),
                ct);

    }
}