using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Application.Features.Patients.Abstractions;

public interface IPatientCommandRepository : IGenericRepository<Patient>
{
}
