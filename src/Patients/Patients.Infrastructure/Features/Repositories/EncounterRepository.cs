using Microsoft.EntityFrameworkCore;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories;

public class EncounterRepository(PatientsDbContext context) : GenericRepository<Encounter>(context), IEncounterRepository
{
    public async Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default)
    {
        var res = await context.Encounters.SingleOrDefaultAsync(e => e.AppointmentId == appointmentId, cancellationToken);

        return res;
    }
}