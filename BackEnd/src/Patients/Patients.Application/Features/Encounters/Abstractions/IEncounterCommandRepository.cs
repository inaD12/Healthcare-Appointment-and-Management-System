using Patients.Domain.Entities;
using Shared.Application.Abstractions;

namespace Patients.Application.Features.Encounters.Abstractions;

public interface IEncounterCommandRepository : IGenericRepository<Encounter>
{
    Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default);
}
