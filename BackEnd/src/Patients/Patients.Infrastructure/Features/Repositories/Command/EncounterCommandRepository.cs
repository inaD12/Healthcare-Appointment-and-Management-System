using System.Data.Entity;
using Patients.Application.Features.Encounters.Abstractions;
using Patients.Domain.Entities;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Repositories;

namespace Patients.Infrastructure.Features.Repositories.Command;

public class EncounterCommandQueryRepository(PatientsCommandDbContext context): GenericRepository<Encounter>(context), IEncounterCommandRepository
{
    public async Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default)
    {
        return await context.Encounters
            .SingleOrDefaultAsync(e => e.AppointmentId == appointmentId, cancellationToken);
    }
}