using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;

namespace Patients.Application.Features.Patients.Queries;

public sealed class PatientQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PatientListItemDto> GetPatients(
        [Service] IPatientRepository repo)
        => repo.GetAll();

    public IQueryable<PatientHeaderDto> GetPatientHeader(
        string patientId,
        [Service] IPatientRepository repo)
        => repo.GetHeader(patientId);
}