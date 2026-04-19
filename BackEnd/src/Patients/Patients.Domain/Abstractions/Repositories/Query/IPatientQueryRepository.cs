using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories.Query;

public interface IPatientQueryRepository : IGenericReadRepository<Patient>
{
    Task<PatientHeaderDto> GetHeaderAsync(string patientId);
}
