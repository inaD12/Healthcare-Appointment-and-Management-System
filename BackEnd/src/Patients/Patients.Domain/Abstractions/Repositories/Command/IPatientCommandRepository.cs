using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories.Command;

public interface IPatientCommandRepository : IGenericRepository<Patient>
{
}
