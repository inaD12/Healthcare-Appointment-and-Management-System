using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories.Command;

public interface IEncounterCommandRepository : IGenericRepository<Encounter>
{
    Task<Encounter?> GetByAppointmentId(string appointmentId, CancellationToken cancellationToken = default);
}
