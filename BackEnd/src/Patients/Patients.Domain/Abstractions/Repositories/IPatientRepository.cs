using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
}
