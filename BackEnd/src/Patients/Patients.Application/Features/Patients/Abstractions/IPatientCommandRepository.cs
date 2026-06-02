using Patients.Domain.Entities;
using Shared.Application.Abstractions;

namespace Patients.Application.Features.Patients.Abstractions;

public interface IPatientCommandRepository : IGenericRepository<Patient>
{
}
