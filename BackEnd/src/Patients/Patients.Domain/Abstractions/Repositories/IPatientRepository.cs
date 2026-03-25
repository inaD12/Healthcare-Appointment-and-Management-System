using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Patients.Domain.Abstractions.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    IQueryable<PatientListItemDto> GetAll();
    Task<PatientHeaderDto> GetHeaderAsync(string patientId);
}
