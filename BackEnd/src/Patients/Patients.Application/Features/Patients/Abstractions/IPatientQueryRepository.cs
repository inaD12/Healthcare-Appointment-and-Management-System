using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Entities;
using Shared.Application.Abstractions;

namespace Patients.Application.Features.Patients.Abstractions;

public interface IPatientQueryRepository : IGenericReadRepository<Patient>
{
    Task<PatientHeaderDto> GetHeaderAsync(string patientId, CancellationToken cancellationToken = default );
}
